using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "Level2"; // Nama scene Level 2
    public float delayBeforeSceneChange = 2f; // Delay sebelum pindah scene

    [Header("UI Feedback (Optional)")]
    public GameObject levelCompleteUI; // UI yang muncul saat level selesai

    private bool levelCompleted = false;

    void Start()
    {
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
    }

    void Update()
    {
        if (!levelCompleted)
        {
            CheckLevelCompletion();
        }
    }

    void CheckLevelCompletion()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");

        if (enemies.Length == 0 && buildings.Length == 0)
        {
            CompleteLevelAndLoadNext();
        }
    }

    void CompleteLevelAndLoadNext()
    {
        levelCompleted = true;

        // Tampilkan UI level complete jika ada
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }

        // Optional: Tambahkan efek suara atau animasi
        Debug.Log("Level Complete! Loading next level...");

        // Pindah ke scene berikutnya setelah delay
        Invoke(nameof(LoadNextScene), delayBeforeSceneChange);
    }

    void LoadNextScene()
    {
        // Cek apakah scene exists dalam build settings
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError($"Scene '{nextSceneName}' tidak ditemukan dalam Build Settings!");
            // Alternatif: load scene berdasarkan index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    // Method tambahan untuk debugging
    public void ForceCompleteLevel()
    {
        CompleteLevelAndLoadNext();
    }

    // Method untuk mendapatkan jumlah enemy dan building yang tersisa
    public int GetRemainingEnemies()
    {
        return GameObject.FindGameObjectsWithTag("enemy").Length;
    }

    public int GetRemainingBuildings()
    {
        return GameObject.FindGameObjectsWithTag("Building").Length;
    }
}