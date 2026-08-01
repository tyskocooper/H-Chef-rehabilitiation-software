using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onStartClick()
    {
        SceneManager.LoadScene("Game");
        
    }

    // Update is called once per frame
    public void OnExitClick()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
        
    }
}
