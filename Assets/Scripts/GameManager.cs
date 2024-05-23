using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image pauseBackgroundImage;
    
    [HideInInspector] public static bool Playing = true;
    
    private float _timer = 244f;
    
    void Start()
    {
        
    }

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
            Time.timeScale = 0; // TODO: A corrutina
            StartCoroutine(HideGameCo());
        }
        else
        {
            StartCoroutine(ShowGameCo());
        }
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
        
        Time.timeScale = 1f;
    }
}
