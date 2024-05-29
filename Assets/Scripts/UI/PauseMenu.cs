using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // SOUNDS
    [SerializeField] private AudioSource clickSound;

    public void OnEnable() {        
        clickSound.ignoreListenerPause = true;
    }

    public void OnContinueButton()
    {
        clickSound.ignoreListenerPause = false;
        GameManager.Playing = true;
        GameManager.Instance.ContinueGame();
    }

    public void OnRestartButton()
    {
        clickSound.ignoreListenerPause = false;
        StartCoroutine(GameManager.Instance.UpMenuPauseAndRestartOrMenu(true));
    }

    public void OnStartButton()
    {
        clickSound.ignoreListenerPause = false;
        StartCoroutine(GameManager.Instance.UpMenuPauseAndRestartOrMenu(false));
    }

    public void OnHoverButton()
    {
        clickSound.Play();
    }
    
}
