using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable { 

    Bounds ObjectBounds { get; }
    void Delete();
    void DragPosition(Vector2 delta);
    void Move();
    void DragSize(Vector2 newSize);
    void Resize(Vector2 newSize);
    

}
