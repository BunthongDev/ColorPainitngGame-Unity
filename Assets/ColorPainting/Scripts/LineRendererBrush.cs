using UnityEngine;

public class FreeDraw : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private LineRenderer linePrefab;
    private LineRenderer currentLineRenderer;
    private Camera mainCamera;

    [Header(" Settings ")]
    [SerializeField] private Color brushColor = Color.red;
    [SerializeField] private float distanceThreshold = 0.1f;

    private Vector3 lastPoint;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
        }
        else if (Input.GetMouseButton(0))
        {
            TryAddPoint();
        }
    }

    private void CreateNewLine()
    {
        Vector3 startPos = GetMouseWorldPosition();

        currentLineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
        currentLineRenderer.positionCount = 1;
        currentLineRenderer.SetPosition(0, startPos);

        // Set line color
        currentLineRenderer.startColor = brushColor;
        currentLineRenderer.endColor = brushColor;

        // Set line width
        lastPoint = startPos;
    }

    private void TryAddPoint()
    {
        if (currentLineRenderer == null)
            return;

        Vector3 newPos = GetMouseWorldPosition();
        float distance = Vector3.Distance(lastPoint, newPos);

        if (distance >= distanceThreshold)
        {
            currentLineRenderer.positionCount++;
            currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, newPos);
            lastPoint = newPos;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Set this based on camera depth and canvas setup
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
