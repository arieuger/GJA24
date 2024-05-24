using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject menuPanel; 
    [SerializeField] private GameObject levelChanger;
    [SerializeField] private AudioSource loopMenu;
    
    private const float MenuSpeed = 2135f;

    private void Start()
    {
        StartCoroutine(BringStartMenu());
    }

    private IEnumerator BringStartMenu()
    {
        
        float remainingTime = 0.75f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;            
        }
        
        float actualYPos = menuPanel.transform.localPosition.y; 
        while (menuPanel.transform.localPosition.y > 90f)
        {
            actualYPos -= MenuSpeed * Time.unscaledDeltaTime;
            menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
            yield return null;
        }

        actualYPos = 90f;
        menuPanel.transform.localPosition = new Vector3(menuPanel.transform.localPosition.x, actualYPos);
        loopMenu.Play();
    
    }

    public void OnPlayButton()
    {
        levelChanger.SetActive(true);
        levelChanger.GetComponent<Animator>().Play("FadeOut");

        StartCoroutine(_waitTilNextScene());
    }

    private IEnumerator _waitTilNextScene()
    {
        float remainingTime = 1.5f;
        while (remainingTime > 0f)
        {
            if (loopMenu.volume >= 0)
            {
                loopMenu.volume -= Time.deltaTime;
            }
            remainingTime -= Time.deltaTime;
            yield return null;            
        }
        SceneManager.LoadScene(1);
    }
}
