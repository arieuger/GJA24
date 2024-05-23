using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void OnContinueButton()
    {
        GameManager.Playing = true;
        GameManager.Instance.ContinueGame();
    }

    public void OnRestartButton()
    {
        GameManager.Playing = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnStartButton()
    {
        GameManager.Playing = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
}
