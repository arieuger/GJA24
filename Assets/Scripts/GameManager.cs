using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image pauseBackgroundImage;
    [SerializeField] private GameObject menuPanel; 
    
    [HideInInspector] public static bool Playing = true;
    
    private float _timer = 244f;
    private const float PauseMenuSpeed = 2135f;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Playing = !Playing;
            PauseGame();
        }
        
        if (Playing) {
            _timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timer / 60F);
            int seconds = Mathf.FloorToInt(_timer % 60F);
            string timeText = minutes.ToString ("00") + ":" + seconds.ToString ("00");
            timerText.text = timeText;
        }
    }

    private void PauseGame()
    {
        if (!Playing)
        {
            StartCoroutine(HideGameCo());
            StartCoroutine(BringMenuPanelCo());
        }
        else
        {
            StartCoroutine(ShowGameCo());
            StartCoroutine(UpMenuPanelCo());
        }
    }

    private IEnumerator BringMenuPanelCo()
    {
        Time.timeScale = 0; 
        menuPanel.SetActive(true);
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

    private IEnumerator UpMenuPanelCo()
    {
        
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
