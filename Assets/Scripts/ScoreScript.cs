using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}
    public static int score; //stores the score
    public TMP_Text scoreText;  //ui text display



    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        score = 0; // sets score to zero at the beginning of the game
        UpdateScoreUI(); // update the UI with the initial score
    }

    public void AddPoints(int points)
    {
        score += points; // adds points to the score
        UpdateScoreUI(); // updates the UI every time the score caanges

        if(HManConnection.Instance != null)
        {
            HManConnection.Instance.updateResistance(score);
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString(); // display the score in the UI
    }
}