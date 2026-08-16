using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Starts the game
    public void onStartClick()
    {
        SceneManager.LoadScene("Game");
        
    }

    // Exits the game
    public void OnExitClick()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
        
    }
}
