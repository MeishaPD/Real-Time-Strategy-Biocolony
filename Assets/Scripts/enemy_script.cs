using UnityEngine;

public class enemy_script : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 5f;
    public LayerMask virusLayer;

    [Header("Chase Settings")]
    public float chaseSpeed = 3f;

    [Header("Patrol Settings")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 3f;

    private Vector3 startPosition;
    private bool movingRight = true;
    private Transform targetVirus;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        DetectVirus();

        if (targetVirus != null)
        {
            MoveTowardsVirus();
        }
        else
        {
            Patrol();
        }
    }

    void DetectVirus()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, virusLayer);

        if (hits.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            Transform closestVirus = null;

            foreach (Collider2D hit in hits)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestVirus = hit.transform;
                }
            }

            targetVirus = closestVirus;
        }
        else
        {
            targetVirus = null;
        }
    }

    void MoveTowardsVirus()
    {
        if (targetVirus == null) return;

        Vector3 direction = (targetVirus.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
    }

    void Patrol()
    {
        float moveDir = movingRight ? 1f : -1f;
        transform.position += new Vector3(moveDir, 0f, 0f) * patrolSpeed * Time.deltaTime;

        if (Vector2.Distance(transform.position, startPosition) >= patrolDistance)
        {
            movingRight = !movingRight;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
