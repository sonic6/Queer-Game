using UnityEngine;
using TMPro;

public class LevelTime : MonoBehaviour
{
    [SerializeField] float myMinutes;
    [SerializeField] float totalSeconds;
    [SerializeField] GameObject pauseMenu;
    
    [SerializeField] TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        totalSeconds = myMinutes*60;
    }

    void TimeDisplay()
    {
            totalSeconds = totalSeconds - 1f * (Time.deltaTime);
        
        
        string minutes = ((int)(totalSeconds/60)).ToString();
        string seconds = ((int)((totalSeconds/60 - (int)(totalSeconds/60)) * 60)).ToString();
        
        timeText.text = minutes + ":" + seconds;
    }

    // Update is called once per frame
    void Update()
    {
        TimeDisplay();
        CheckPauseMenu();
        if (totalSeconds <= 0 && Time.timeScale != 0)
            WinOrLose.LoseGame();
    }
    
    void CheckPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
}
