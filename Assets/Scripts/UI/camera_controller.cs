using UnityEngine;

public class camera_controller : MonoBehaviour
{
    [Header("Camera Movement Settings")]
    public float panSpeed = 1f;  // coba mulai dengan nilai kecil dulu
    public bool enablePanning = true;

    [Header("Camera Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minZoom = 1f;
    public float maxZoom = 10f;
    public bool enableZoom = true;

    [Header("Camera Bounds (Optional)")]
    public bool useBounds = false;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private Camera cam;
    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            cam.orthographic = true;
        }
    }

    void LateUpdate()
    {
        HandleMouseInput();
        HandleZoom();
    }

    void HandleMouseInput()
    {
        if (!enablePanning) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Simpan posisi world saat mulai drag
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPosition;

            Vector3 newPosition = transform.position + difference * panSpeed;

            if (useBounds)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            }

            newPosition.z = transform.position.z;
            transform.position = newPosition;

            // Update dragOrigin tiap frame supaya kamera dan mouse world tetap sinkron
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void HandleZoom()
    {
        if (!enableZoom) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            float newSize = cam.orthographicSize - scrollInput * zoomSpeed;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            cam.orthographicSize = newSize;
        }
    }

    public void SetCameraPosition(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }

    public void SetZoom(float zoomLevel)
    {
        cam.orthographicSize = Mathf.Clamp(zoomLevel, minZoom, maxZoom);
    }

    public void ResetCamera()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
        cam.orthographicSize = 5f;
    }
}
