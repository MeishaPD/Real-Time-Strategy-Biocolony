using UnityEngine;

public class MeatCollectible : MonoBehaviour
{
    [Header("Meat Settings")]
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private string virusTag = "virus";

    [Header("Visual Feedback")]
    [SerializeField] private GameObject collectEffect;
    [SerializeField] private AudioClip collectSound;

    private AudioSource audioSource;

    void Start()
    {
        if (collectSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = collectSound;
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(virusTag))
        {
            HealthVirus healthComponent = other.GetComponent<HealthVirus>();
            if (healthComponent != null)
            {
                CollectMeat(healthComponent);
            }
        }
    }

    private void CollectMeat(HealthVirus healthVirus)
    {
        healthVirus.Heal(healAmount);

        if (audioSource != null && collectSound != null)
        {
            audioSource.Play();
        }

        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        Debug.Log($"Meat collected! Healed for {healAmount} HP");

        Destroy(gameObject);
    }
}