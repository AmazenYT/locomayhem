using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float borderSize = 20f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;

    [Header("Map Bounds")]
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minY = -50f;
    [SerializeField] private float maxY = 50f;

    private Camera cam;
    private Vector3 dragOrigin;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMovement();
        HandleDrag();
        HandleZoom();
        ClampPosition();
    }

    private void HandleMovement()
    {
        Vector3 movement = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0f);

        // Edge Scrolling
        if (Input.mousePosition.x < borderSize)
            movement.x = -1;

        if (Input.mousePosition.x > Screen.width - borderSize)
            movement.x = 1;

        if (Input.mousePosition.y < borderSize)
            movement.y = -1;

        if (Input.mousePosition.y > Screen.height - borderSize)
            movement.y = 1;

        transform.position += movement.normalized * moveSpeed * Time.deltaTime;
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            difference.z = 0;

            transform.position += difference;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        cam.orthographicSize -= scroll * zoomSpeed * Time.deltaTime * 10f;

        cam.orthographicSize = Mathf.Clamp(
            cam.orthographicSize,
            minZoom,
            maxZoom);
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}