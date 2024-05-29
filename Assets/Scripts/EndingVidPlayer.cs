using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVidPlayer : MonoBehaviour
{
    [SerializeField] private string videoFileName;
    
    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    private IEnumerator PlayVideo()
    {
        
        float remainingTime = 35f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;            
        }
        
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(EndGame());
        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
    
    
    private IEnumerator EndGame()
    {
        float remainingTime = 45;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(0);
    }
    
}
