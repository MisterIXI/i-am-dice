using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameUI : MonoBehaviour
{
    public TMP_Text StopwatchText;
    int _minutes;
    int _seconds;
    float _millis;
    float totalTime;
    bool _isPlaying;



    private void Update()
    {


        if (_isPlaying)
        {
            totalTime += Time.deltaTime;
            _millis = (int) (totalTime * 100) % 100;
            Debug.Log(_seconds);
            if(_seconds >= 60)
            {
                _seconds = 0;
                _minutes++;
            }
            StopwatchText.text =  _minutes.ToString("00") + ":" + _seconds.ToString("00") + ":" + _millis.ToString("00");
            _seconds = (int)totalTime % 60;

        }
    }

    public void TriggerStopwatch()
    {
        _isPlaying = true;

    }
}
