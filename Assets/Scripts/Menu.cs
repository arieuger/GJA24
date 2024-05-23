using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject levelChanger;

    public void OnPlayButton()
    {
        levelChanger.SetActive(true);
        levelChanger.GetComponent<Animator>().Play("FadeOut");

        StartCoroutine(waitTilNextScene());
    }

    private IEnumerator waitTilNextScene()
    {
        float remainingTime = 1.5f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;            
        }
        SceneManager.LoadScene(1);
    }
}
