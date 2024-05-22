using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TMP_Text timerText;
    [HideInInspector] public bool playing = true;
    
    private float _timer = 244f;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (playing) {
            _timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timer / 60F);
            int seconds = Mathf.FloorToInt(_timer % 60F);
            string timeText = minutes.ToString ("00") + ":" + seconds.ToString ("00");
            timerText.text = timeText;
        }
    }
}
