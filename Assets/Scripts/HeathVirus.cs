using UnityEngine;
using UnityEngine.UI;

public class HealthVirus : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;

    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _damageAmount = 10f;
    [SerializeField] private string _enemyTag = "enemy"; // Ganti layer → tag

    private void Awake()
    {
        _currentHealth = _maxHealth;
        UpdateHealthBar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            TakeDamage(_damageAmount);
        }
    }

    private void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        if (_currentHealth <= 0f)
        {
            Die();
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (_healthBarFill != null)
        {
            _healthBarFill.fillAmount = _currentHealth / _maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Virus died.");
        Destroy(gameObject);
    }
}