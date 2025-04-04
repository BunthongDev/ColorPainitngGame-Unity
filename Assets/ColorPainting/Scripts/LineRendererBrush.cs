using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    
    [Header(" Elements ")]
    [SerializeField] private LineRenderer linePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CreateLine();
    }
    
    private void CreateLine()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
        
        // we did not hit anything
        if (hit.collider == null)
            return;
            
        LineRenderer lineInstance = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
        lineInstance.SetPosition(0, Vector3.zero);
        
        lineInstance.positionCount++;
        lineInstance.SetPosition(1, hit.point);
        
    }
}
