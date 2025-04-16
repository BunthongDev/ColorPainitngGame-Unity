using UnityEngine;

public class FreeDraw : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private LineRenderer linePrefab;
    private LineRenderer currentLineRenderer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CreateLine();
        else if (Input.GetMouseButton(0))
            PaintOnCanvas();
    }

    private void CreateLine()
    {
        Vector3 startPos = GetMouseWorldPosition();

        // Create a new line and set the initial position immediately
        currentLineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
        currentLineRenderer.positionCount = 1;
        currentLineRenderer.SetPosition(0, startPos);
    }

    private void PaintOnCanvas()
    {
        if (currentLineRenderer == null)
            return;

        Vector3 newPos = GetMouseWorldPosition();
        AddPoint(newPos);
    }

    private void AddPoint(Vector3 worldPos)
    {
        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, worldPos);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Adjust this if needed based on your scene depth
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}



