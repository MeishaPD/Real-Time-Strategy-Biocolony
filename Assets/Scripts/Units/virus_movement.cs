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
        targetPosition = transform.position;
        lastPosition = transform.position;

        if (showTargetLine && lineRenderer == null)
        {
            SetupLineRenderer();
        }
    }

    void Update()
    {
        MoveToTarget();
        UpdateVisualFeedback();
    }

    void MoveToTarget()
    {
        if (!isMoving) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (stopOnReachTarget && distanceToTarget <= stopDistance)
        {
            isMoving = false;
            return;
        }

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
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.sortingOrder = -1;
    }

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

    void OnDrawGizmos()
    {
        if (isMoving)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, 0.2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPosition);
        }
    }
}