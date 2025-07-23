using UnityEngine;
using TMPro;
public class TimerController : MonoBehaviour
{
    public float timeRemaining = 300f; // 5 minutes
    public bool timerRunning = true;
    public TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTimerUI(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);
            }
            else
            {
                timerRunning = false;
                timeRemaining = 0;
                TimerEnded();
            }
        }
    }

     void UpdateTimerUI(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        Debug.Log("Time's up!");
        // Add fail/game-over logic here
    }
    
}
