using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void RestartScene()
    {
        ResetVariables();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int sceneindex)
    {
        ResetVariables();
        SceneManager.LoadScene(sceneindex);
    }

    //This resets important variables that need to reset between scenes
    public static void ResetVariables()
    {
        Time.timeScale = 1;
        PlayerMovement.MoveActiveState = true;
        FollowerCounter.followers = 0;
        Verses.extraStrength = 0;
        InfoDealer.cardsInHand.Clear();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
