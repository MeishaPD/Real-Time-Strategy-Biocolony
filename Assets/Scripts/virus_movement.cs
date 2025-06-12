using UnityEngine;

public class virus_movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public bool smoothRotation = false;

    [Header("Movement Behavior")]
    public bool stopOnReachTarget = true;
    public float stopDistance = 0.1f;
    public bool continuousMovement = false;

    [Header("Visual Feedback")]
    public bool showTargetLine = true;
    public LineRenderer lineRenderer;
    public Color lineColor = Color.red;

    // [SerializeField] private PointToClick m_PointToClickPrefab;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Vector3 lastPosition;

    void Start()
    {
        // Set posisi target awal ke posisi virus
        targetPosition = transform.position;
        lastPosition = transform.position;

        // Setup LineRenderer jika belum ada
        if (showTargetLine && lineRenderer == null)
        {
            SetupLineRenderer();
        }
    }

    void Update()
    {
        HandleMouseInput();
        MoveToTarget();
        UpdateVisualFeedback();
    }

    void HandleMouseInput()
    {
        // Deteksi klik mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Konversi posisi mouse ke world position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; // Pertahankan Z position untuk 2D

            // Instantiate(m_PointToClickPrefab, mousePosition, Quaternion.identity);

            // Set target position dan mulai movement
            SetTargetPosition(mousePosition);
        }

        // Jika continuous movement aktif, update target saat mouse ditekan
        if (continuousMovement && Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            SetTargetPosition(mousePosition);
        }
    }

    void MoveToTarget()
    {
        if (!isMoving) return;

        // Hitung jarak ke target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Cek apakah sudah sampai di target
        if (stopOnReachTarget && distanceToTarget <= stopDistance)
        {
            isMoving = false;
            return;
        }

        // Gerakkan virus ke target
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotasi virus menghadap arah movement (opsional) - DISABLED untuk mencegah rotasi
        // if (smoothRotation && direction != Vector3.zero)
        // {
        //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //     Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // }
    }

    void UpdateVisualFeedback()
    {
        // Update line renderer untuk menunjukkan arah target
        if (showTargetLine && lineRenderer != null && isMoving)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetPosition);
        }
        else if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void SetTargetPosition(Vector3 newTarget)
    {
        targetPosition = newTarget;
        isMoving = true;
    }

    void SetupLineRenderer()
    {
        // Buat LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.sortingOrder = -1; // Agar muncul di belakang sprite
    }


    // Method publik untuk kontrol eksternal
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void MoveToPosition(Vector3 position)
    {
        SetTargetPosition(position);
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    // Untuk debugging
    void OnDrawGizmos()
    {
        // Gambar target position
        if (isMoving)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, 0.2f);

            // Gambar garis ke target
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPosition);
        }
    }
}