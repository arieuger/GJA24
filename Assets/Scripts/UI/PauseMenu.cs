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
        StartCoroutine(GameManager.Instance.UpMenuPauseAndRestartOrMenu(true));
    }

    public void OnStartButton()
    {
        StartCoroutine(GameManager.Instance.UpMenuPauseAndRestartOrMenu(false));
    }
    
}
