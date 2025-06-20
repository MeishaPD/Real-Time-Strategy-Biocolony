using UnityEngine;

public class PointToClick: MonoBehaviour
{
    [SerializeField] private float m_Duration = 1f;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private AnimationCurve m_ScaleCurve;

    private Vector3 m_InitialScale;

    private float m_Timer;
    private float m_ScaleTimer;

    void Start()
    {
        m_InitialScale = transform.localScale;
    }

    void Update()
    {
        m_Timer += Time.deltaTime;
        m_ScaleTimer += Time.deltaTime;
        m_ScaleTimer %= 1f;

        float scaleMultiplier = m_ScaleCurve.Evaluate(m_ScaleTimer);
        transform.localScale = m_InitialScale * scaleMultiplier;

        if (m_Timer >= m_Duration * 0.8f)
        {
            float fadeProgress = (m_Timer - m_Duration * 0.8f) / (m_Duration * 0.1f);
            if (m_SpriteRenderer != null)
            {
                m_SpriteRenderer.color = new Color(1f, 1f, 1f, 1f - fadeProgress);
            }
        }

        if (m_Timer >= m_Duration)
        {
            Destroy(gameObject);
        }
    }
}
