using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 60f;


    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                

                GameOver();
            
                timerText.color = Color.red;
            }
         }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        
        }
        private void GameOver()
    {
       var serviceOver = FindAnyObjectByType<ServiceOver>(FindObjectsInactive.Include);
        serviceOver.ServiceComplete(ScoreManager.score);
    }
}
