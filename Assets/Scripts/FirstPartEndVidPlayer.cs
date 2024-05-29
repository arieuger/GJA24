using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FirstPartEndVidPlayer : MonoBehaviour
{
    private string _videoFileName = "endgame.webm";
    private VideoPlayer _videoPlayer;
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(StopVideo());
        if (_videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, _videoFileName);
            Debug.Log(videoPath);
            _videoPlayer.url = videoPath;
            _videoPlayer.Play();
        }
    }

    private IEnumerator StopVideo()
    {
        float remainingTime = 35f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        _videoPlayer.Stop();
    }

}
