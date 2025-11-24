using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    // Assign the SINGLE image that has the Radial 360 setup
    public Image timerBackground;

    // Assign the TextMeshPro component for the number display
    //public TextMeshProUGUI timeDisplay;

    public float totalTime = 60f; // The duration of the countdown
    private float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        // Initialize the timer state
        StartTimer();
    }

    public void StartTimer()
    {
        currentTime = totalTime;
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (currentTime > 0)
            {
                // 1. Decrease the time based on the time passed since the last frame
                currentTime -= Time.deltaTime;

                // 2. Calculate the normalized time remaining (0.0 to 1.0)
                float fillRatio = currentTime / totalTime;

                // 3. Update the Fill Amount of the single circular image
                // As the time decreases, the Fill Amount also decreases, showing the arc fade.
                timerBackground.fillAmount = fillRatio;

                // 4. Update the text display
                // Use Mathf.FloorToInt() to show whole seconds, or Mathf.CeilToInt() to round up.
                //timeDisplay.text = Mathf.FloorToInt(currentTime).ToString() + "s";
            }
            else
            {
                // Timer has finished
                currentTime = 0;
                isTimerRunning = false;
                timerBackground.fillAmount = 0f; // Ensure the circle is fully gone
                //timeDisplay.text = "TIME UP!";

                // **TODO:** Add your "Time Up" logic here (e.g., LoadScene, TriggerEvent)
            }
        }
    }
}