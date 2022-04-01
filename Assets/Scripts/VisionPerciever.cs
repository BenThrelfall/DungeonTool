using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionPerciever : MonoBehaviour, IRequiresDependancy {

    public int rayCount;
    public float viewDistance;
    public float extraAmount = 1f;
    public LayerMask visionLayerMask;

    //public MeshFilter memoryMeshFilter;
    public MeshFilter extraMeshFilter;
    MeshFilter meshFilter;
    Transform trans;


    Vector3 previousPosition;

    IVisionUpdateEventHandler visionEventHandler;

    private void Awake() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    void Start() {
        previousPosition = transform.position;
        FetchComponents();
        UpdateVision();
    }

    private void OnEnable() {
        visionEventHandler.VisionUpdate += UpdateVision;
    }

    private void OnDisable() {
        visionEventHandler.VisionUpdate -= UpdateVision;
    }

    private void UpdateVision() {

        Mesh mesh = new Mesh();
        Mesh extraMesh = new Mesh();
        Mesh memoryMesh = new Mesh();
        meshFilter.mesh = mesh;
        extraMeshFilter.mesh = extraMesh;

        Vector3[] extraVertices = new Vector3[rayCount * 2 + 2];
        Vector2[] extraUvs = new Vector2[extraVertices.Length];
        int[] extraTriangles = new int[rayCount * 2 * 3];

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        Vector3 origin = trans.position;
        vertices[0] = Vector3.zero;

        float angleIncrease = (Mathf.PI * 2) / rayCount;
        float angle = 0f;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++) {
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            var raycast = Physics2D.Raycast(origin, direction, viewDistance, visionLayerMask);
            Vector3 vertex = raycast.collider is null ? direction * viewDistance : ((Vector3)raycast.point - origin);
            vertices[vertexIndex] = vertex;
            extraVertices[(vertexIndex - 1) * 2] = vertex;
            extraVertices[(vertexIndex - 1) * 2 + 1] = vertex + direction * extraAmount;

            if (i > 0) {

                extraTriangles[triangleIndex * 2] = vertexIndex * 2 - 4;
                extraTriangles[triangleIndex * 2 + 1] = vertexIndex * 2 - 3;
                extraTriangles[triangleIndex * 2 + 2] = vertexIndex * 2 - 2;

                extraTriangles[triangleIndex * 2 + 3] = vertexIndex * 2 - 2;
                extraTriangles[triangleIndex * 2 + 4] = vertexIndex * 2 - 3;
                extraTriangles[triangleIndex * 2 + 5] = vertexIndex * 2 - 1;


                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        extraMesh.vertices = extraVertices;
        extraMesh.uv = extraUvs;
        extraMesh.triangles = extraTriangles;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        //DoMemory(memoryMesh, origin); Too expensive and doesn't really work
    }

    //private void DoMemory(Mesh memoryMesh, Vector3 origin) {
    //    bool hasPrevMem = memoryMeshFilter.mesh != null;

    //    CombineInstance[] combine = new CombineInstance[hasPrevMem ? 2 : 1];
    //    combine[0].subMeshIndex = 0;
    //    combine[0].mesh = meshFilter.sharedMesh;
    //    combine[0].transform = Matrix4x4.identity;

    //    if (hasPrevMem) {
    //        combine[1].subMeshIndex = 0;
    //        combine[1].mesh = memoryMeshFilter.sharedMesh;
    //        combine[1].transform = Matrix4x4.TRS(previousPosition - origin, Quaternion.identity, Vector3.one);
    //    }

    //    memoryMesh.CombineMeshes(combine);

    //    memoryMeshFilter.mesh = memoryMesh;
    //}

    private void LateUpdate() {
        if (previousPosition != trans.position) {
            UpdateVision();
            previousPosition = trans.position;
        }
    }

    private void FetchComponents() {
        meshFilter = GetComponent<MeshFilter>();
        trans = GetComponent<Transform>();
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        visionEventHandler = serviceCollection.GetService<IVisionUpdateEventHandler>();
    }
}
