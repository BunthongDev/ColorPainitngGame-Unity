using System.Collections.Generic;
using UnityEngine;

public class MeshBrush : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private MeshFilter meshFilterPrefab;

    [Header(" Settings ")]
    [SerializeField] private float brushSize;
    [SerializeField] private float distanceThreshold;
    private Vector2 lastClickedPosition;
    private Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CreateMesh();
        else if (Input.GetMouseButton(0))
            Paint();
    }

    private void CreateMesh()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);

        // We did not hit anything
        if (hit.collider == null)
            return;

        mesh = new Mesh();

        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        // Setting up our vertices
        vertices.Add(hit.point + (Vector3.up + Vector3.right) * brushSize / 2);
        vertices.Add(vertices[0] + Vector3.down * brushSize);
        vertices.Add(vertices[1] + Vector3.left * brushSize);
        vertices.Add(vertices[2] + Vector3.up * brushSize);

        // Configure UVs
        uvs.Add(Vector2.one);
        uvs.Add(Vector2.right);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.up);

        // Setting up our triangles
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        // Specify the mesh position
        float zPosition = transform.childCount * .01f;
        Vector3 position = Vector3.back * zPosition; 

        MeshFilter meshFilterInstance = Instantiate(meshFilterPrefab, position, Quaternion.identity, transform);
        meshFilterInstance.sharedMesh = mesh;

        meshFilterInstance.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

        // Save the last clicked position
        lastClickedPosition = Input.mousePosition;
    }

    private void Paint()
    {
        if (Vector2.Distance(lastClickedPosition, Input.mousePosition) < distanceThreshold)
            return;

        // Save the last clicked position
        lastClickedPosition = Input.mousePosition;

        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);

        // We did not hit anything
        if (hit.collider == null)
            return;

        // The amount of vertices we have before drawing the quad
        int startIndex = mesh.vertices.Length;

        // Setting up our vertices
        vertices.Add(hit.point + (Vector3.up + Vector3.right) * brushSize / 2);
        vertices.Add(vertices[startIndex + 0] + Vector3.down * brushSize);
        vertices.Add(vertices[startIndex + 1] + Vector3.left * brushSize);
        vertices.Add(vertices[startIndex + 2] + Vector3.up * brushSize);

        // Configure UVs
        uvs.Add(Vector2.one);
        uvs.Add(Vector2.right);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.up);

        // Setting up our triangles
        triangles.Add(startIndex + 0);
        triangles.Add(startIndex + 1);
        triangles.Add(startIndex + 2);

        triangles.Add(startIndex + 0);
        triangles.Add(startIndex + 2);
        triangles.Add(startIndex + 3);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        // Reassign the mesh to the mesh filter
        transform.GetChild(transform.childCount - 1).GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
