using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

/// <summary>
/// Tool for selecting board elements and acting upon selected elements
/// </summary>
public class SelectTool : MonoBehaviour {

    private enum SelectMode {
        None,
        Selecting,
        Moving,
        Resizing
    };

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    LayerMask mapLayerMask;

    [SerializeField]
    TextMeshProUGUI layerIndicator;

    [SerializeField]
    LayerMask tokenLayerMask;

    bool mapMaskActive = false;

    [SerializeField]
    GameObject selectObject;
    SpriteRenderer selectRenderer;
    Bounds selectionBounds;

    [SerializeField]
    GameObject dragArea;
    SpriteRenderer dragRenderer;

    const float dragThreshold = 0.2f;

    Vector2 startPoint;
    Vector2 boundsStartPoint;
    Vector2 diag;

    HashSet<ISelectable> selectedObjects = new HashSet<ISelectable>();

    SelectMode mode;


    private void Start() {
        selectRenderer = selectObject.GetComponent<SpriteRenderer>();
        dragRenderer = dragArea.GetComponent<SpriteRenderer>();
        DeselectAll();
    }

    public void DoSelection() {
        SelectedAreaInputs();
        EmptySpaceInputs();

        RegularInputs();
    }

    /// <summary>
    /// Inputs that are tested unconditionally
    /// </summary>
    private void RegularInputs() {
        
        if (Input.GetKeyDown(KeyCode.Delete)) {
            DeleteSelectedItems();
        }

        if (Input.GetKeyDown(KeyCode.Minus)) {
            DownSizeSelectedItems();
        }

        if (Input.GetKeyDown(KeyCode.Equals)) {
            UpSizeSelectedItems();
        }

        if (Input.GetKeyDown(KeyCode.Insert)) {
            layerMask = mapMaskActive ? tokenLayerMask : mapLayerMask;
            if (mapMaskActive) layerIndicator.text = "Token";
            else layerIndicator.text = "Map";
            mapMaskActive = !mapMaskActive;
        }

        if (Input.GetMouseButtonUp(0)) {
            mode = SelectMode.None;
            dragArea.SetActive(false);
        }
    }

    public void UpSizeSelectedItems() {
        foreach (var item in selectedObjects) {
            item.ResizeWithSnapping(item.ObjectBounds.size + Vector3.one);
        }
    }

    public void DownSizeSelectedItems() {
        foreach (var item in selectedObjects) {
            if (item.ObjectBounds.size.x == 1) continue;
            item.ResizeWithSnapping(item.ObjectBounds.size - Vector3.one);
        }
    }

    public void DeleteSelectedItems() {
        foreach (var item in selectedObjects) {
            item.Delete();
        }
        DeselectAll();
    }

    /// <summary>
    /// Inputs that are only tested if the mouse is over the selected area or
    /// if the tool is in Moving mode and not in any other mode except None
    /// </summary>
    private void SelectedAreaInputs() {

        if (!(mode == SelectMode.Moving) && !(mode == SelectMode.None)) return;
        if (!(mode == SelectMode.Moving) && !selectionBounds.Contains(MousePos())) return;

        if (Input.GetMouseButtonDown(0)) {
            StartMoveMode();
        }

        if (Input.GetMouseButton(0)) {
            Vector2 diff = MousePos() - startPoint;
            selectionBounds.center = (Vector3)boundsStartPoint + (Vector3)diff;
            selectObject.transform.position = selectionBounds.center;
            selectRenderer.size = new Vector2(selectionBounds.size.x, selectionBounds.size.y);
            foreach (var item in selectedObjects) {
                item.DragPosition(diff);
            }
        }

        if (Input.GetMouseButtonUp(0)) {

            Vector2 diff = MousePos() - startPoint;
            Vector2 roundDiff = new Vector2(Mathf.Round(diff.x), Mathf.Round(diff.y));

            selectionBounds.center = (Vector3)boundsStartPoint + (Vector3)roundDiff;
            selectObject.transform.position = selectionBounds.center;
            selectRenderer.size = new Vector2(selectionBounds.size.x, selectionBounds.size.y);
            foreach (var item in selectedObjects) {
                item.DragPosition(roundDiff);
            }

            foreach (var item in selectedObjects) {
                item.Move();
            }
        }
        

    }

    private void StartMoveMode() {
        startPoint = MousePos();
        boundsStartPoint = selectionBounds.center;
        mode = SelectMode.Moving;
    }

    private void EmptySpaceInputs() {

        if (!(mode == SelectMode.Selecting) && !(mode == SelectMode.None)) return;
        if (!(mode == SelectMode.Selecting) && selectionBounds.Contains(MousePos())) return;

        if (Input.GetMouseButtonDown(0)) {
            mode = SelectMode.Selecting;
            startPoint = MousePos();
            if(!Input.GetKey(KeyCode.LeftControl)) DeselectAll();
            var selected = Physics2D.OverlapBox(startPoint, Vector2.one * dragThreshold, 0f, layerMask);
            if (selected != null) {
                var selectable = selected.GetComponent<ISelectable>();
                if (selectable == null) throw new System.Exception("Object on selectable layer has no interface");
                Select(selectable);
                StartMoveMode();
                return;
            }
        }

        if (Input.GetMouseButton(0)) {
            diag = startPoint - MousePos();

            if (diag.sqrMagnitude > dragThreshold) {
                dragArea.SetActive(true);
                dragArea.transform.position = startPoint - diag * 0.5f;
                dragRenderer.size = diag;
                List<Collider2D> selected = new List<Collider2D>();
                var filter = new ContactFilter2D();
                filter.SetLayerMask(layerMask);
                Physics2D.OverlapArea(startPoint, MousePos(), filter, selected);

                foreach (var item in selected) {
                    var selectable = item.GetComponent<ISelectable>();
                    if (selectable == null) throw new System.Exception("Object on selectable layer has no interface");
                    Select(selectable);
                }

            }

        }

    }

    /// <summary>
    /// Adds an item to the selected objects collection and updates
    /// the selected area bounds and the object that visually shows 
    /// the selected area. Will deselect objects if ctrl is being pressed.
    /// </summary>
    /// <param name="selectable"></param>
    private void Select(ISelectable selectable) {

        if (selectedObjects.Contains(selectable)) {
            if (Input.GetKey(KeyCode.LeftControl)) {
                Deselect(selectable);
                return;
            }
            else {
                return;
            }
        }

        selectObject.SetActive(true);
        if (selectedObjects.Count == 0) selectionBounds.center = selectable.ObjectBounds.center;

        selectedObjects.Add(selectable);
        selectionBounds.Encapsulate(selectable.ObjectBounds);
        selectObject.transform.position = selectionBounds.center;
        selectRenderer.size = new Vector2(selectionBounds.size.x, selectionBounds.size.y);
    }

    /// <summary>
    /// Removes an object from the selected objects and 
    /// recalculates the bounds updaing the bounds and the 
    /// visual display object
    /// </summary>
    /// <param name="selectable"></param>
    void Deselect(ISelectable selectable) {
        selectedObjects.Remove(selectable);

        //TODO: Find a less wasteful way
        RecalculateBounds();

    }

    /// <summary>
    /// Recalculate bounds from scratch and update
    /// the visual display object
    /// </summary>
    private void RecalculateBounds() {
        if (selectedObjects.Count == 0) return;
        selectionBounds.extents = Vector3.zero;
        selectionBounds.center = selectedObjects.First().ObjectBounds.center;

        foreach (var item in selectedObjects) {
            selectionBounds.Encapsulate(item.ObjectBounds);
        }

    }

    void DeselectAll() {
        selectObject.SetActive(false);
        selectionBounds.extents = Vector3.zero;
        selectedObjects.Clear();
    }

    Vector2 MousePos() {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

}
