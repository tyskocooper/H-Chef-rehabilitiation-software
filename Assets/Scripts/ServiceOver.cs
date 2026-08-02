using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class ServiceOver : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public TMP_Text finalScoreText;

    public string mainMenuScene = "MainMenu";

    private string currentSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameOverCanvas.SetActive(false);
        currentSceneName = SceneManager.GetActiveScene().name;
        
    }

    // Update is called once per frame
    public void ServiceComplete(int finalScore)
    {
        gameOverCanvas.SetActive(true);
        finalScoreText.text = $"{finalScore}";

        Time.timeScale = 0f; // pauses game when timer reaches 0
        }

    public void OnRestartPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void OnExitPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
