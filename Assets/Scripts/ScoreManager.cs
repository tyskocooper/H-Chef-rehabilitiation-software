using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}
    public static int score; //stores the score
    public TMP_Text scoreText;  //ui text display



    void Awake()
    {
              if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;  //allows me to decouple the score from the ui so I can have a two score ui's for ingame and game over

    }

    void Start()
    {
        Debug.Log("ScoreManager Start() called - resetting score to 0. Called by: " + gameObject.name);
        score = 0; // sets score to zero at the beginning of the game
        UpdateScoreUI(); // update the UI with the initial score
    }

    public void AddPoints(int points)
    {
        score += points; // adds points to the score
        UpdateScoreUI(); // updates the UI every time the score caanges
    }

    void UpdateScoreUI()
    {
        scoreText.text = "" + score.ToString(); // display the score in the UI
    }



}