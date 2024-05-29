using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    [SerializeField] private string videoFileName;
    [SerializeField] private bool isEnding;

    private VideoPlayer _videoPlayer;
    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(2);
    }

    private IEnumerator PlayVideo()
    {
        if (isEnding)
        {
            float remainingTime = 21f;
            while (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;
                yield return null;            
            }
        }
        
        _videoPlayer = GetComponent<VideoPlayer>();
        if (_videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            _videoPlayer.url = videoPath;
            _videoPlayer.Play();
        }
        
        if (isEnding) StartCoroutine(WaitToNextScene());
    }

    private IEnumerator WaitToNextVideo()
    {
        float remainingTime = 20f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;            
        }
        _videoPlayer.Stop();
    }

    private IEnumerator WaitToNextScene()
    {
        float remainingTime = 7f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;            
        }
        SceneManager.LoadScene(2);
    }
}
