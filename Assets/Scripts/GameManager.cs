using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image pauseBackgroundImage;
    [SerializeField] private GameObject menuPanel;

    // MUSIC
    [SerializeField] private AudioSource mainMusic;
    [SerializeField] private AudioSource pauseMenuMusic;

    // SOUNDS
    [SerializeField] private AudioSource pauseMenuSound;
    
    [HideInInspector] public static bool Playing = true;

    private float _timer = 241f; 
    private const float PauseMenuSpeed = 2135f;
    
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        pauseMenuMusic.ignoreListenerPause = true;
        pauseMenuSound.ignoreListenerPause = true;
        StartCoroutine(UtilsClass.StartMusic(mainMusic));
    }

    void Update()
    {

        if (_timer <= 0.1f)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ManagePauseButton();
        }
        
        if (Playing) {
            _timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timer / 60F);
            int seconds = Mathf.FloorToInt(_timer % 60F);
            string timeText = minutes.ToString ("0") + ":" + seconds.ToString ("00");
            timerText.text = timeText;

            if (_timer <= 5)
            {
                EndGame();
            }
        }
    }

    public void EndGame(bool win = false)
    {
        StartCoroutine(PlayerMovement.Instance.PlayerEndGame());
        FindObjectsByType<Police>(FindObjectsSortMode.None).ToList().ForEach(p => StartCoroutine(p.PoliceEndGame()));
        StartCoroutine(LoadCredits(win));
    }

    private IEnumerator LoadCredits(bool win = false)
    {
        float remainingTime = 3.5f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(win ? 3 : 4);
    }

    private void ManagePauseButton()
    {
        Playing = !Playing;
        if (!Playing)
        {
            PauseGame();
        }
        else
        {
            ContinueGame();
        }
    }

    private void PauseGame()
    {
        StartCoroutine(HideGameCo());
        StartCoroutine(BringMenuPanelCo());
    }

    public void ContinueGame()
    {
        pauseMenuSound.Play();
        StartCoroutine(ShowGameCo());
        StartCoroutine(UpMenuPanelCo());
    }
    
    public IEnumerator UpMenuPauseAndRestartOrMenu(bool isRestart)
    {
        pauseMenuSound.Play();
        UtilsClass.StopMusic(pauseMenuMusic);
        
        float actualYPos = menuPanel.transform.localPosition.y; 
        while (menuPanel.transform.localPosition.y < 1075f)
        {
            actualYPos += PauseMenuSpeed * Time.unscaledDeltaTime;
            menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
            yield return null;
        }
        
        actualYPos = 1075f;
        menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
        
        Playing = true;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        
        if (isRestart) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else SceneManager.LoadScene(0);
    }
    
    private IEnumerator UpMenuPanelCo()
    {

        UtilsClass.StopMusic(pauseMenuMusic);
        
        float actualYPos = menuPanel.transform.localPosition.y; 
        while (menuPanel.transform.localPosition.y < 1075f)
        {
            actualYPos += PauseMenuSpeed * Time.unscaledDeltaTime;
            menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
            yield return null;
        }
        
        actualYPos = 1075f;
        menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private IEnumerator BringMenuPanelCo()
    {
        Time.timeScale = 0; 
        AudioListener.pause = true;
        menuPanel.SetActive(true);

        StartCoroutine(UtilsClass.StartMusic(pauseMenuMusic, true));
        pauseMenuSound.Play();
        
        float actualYPos = menuPanel.transform.localPosition.y; 
        while (menuPanel.transform.localPosition.y > 90f)
        {
            actualYPos -= PauseMenuSpeed * Time.unscaledDeltaTime;
            menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
            yield return null;
        }

        actualYPos = 90f;
        menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
    }

    private IEnumerator HideGameCo()
    {
        pauseBackgroundImage.gameObject.SetActive(true);
        float actualAlpha = pauseBackgroundImage.color.a;
        while (pauseBackgroundImage.color.a < 188f/255f)
        {
            actualAlpha += 0.1f;
            pauseBackgroundImage.color = new Color(pauseBackgroundImage.color.r, pauseBackgroundImage.color.g,
                pauseBackgroundImage.color.b, actualAlpha);
            yield return null;
        }
    }

    private IEnumerator ShowGameCo()
    {
        
        float actualAlpha = pauseBackgroundImage.color.a;
        while (pauseBackgroundImage.color.a > 0)
        {
            actualAlpha -= 0.1f;
            pauseBackgroundImage.color = new Color(pauseBackgroundImage.color.r, pauseBackgroundImage.color.g,
                pauseBackgroundImage.color.b, actualAlpha);
            yield return null;
        }

        actualAlpha = 0;
        pauseBackgroundImage.color = new Color(pauseBackgroundImage.color.r, pauseBackgroundImage.color.g,
            pauseBackgroundImage.color.b, actualAlpha);
        pauseBackgroundImage.gameObject.SetActive(false);

    }
}
