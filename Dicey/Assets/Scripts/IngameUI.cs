using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class IngameUI : MonoBehaviour
{
    public TMP_Text StopwatchText;
    int _minutes;
    int _seconds;
    float _millis;
    float totalTime;
    bool _isPlaying;
    Stopwatch _stopwatch;

    bool _isStopwatchStarted;
    bool once;
    private void Start()
    {
        _stopwatch = new Stopwatch();
    }

    private void Update()
    {

        if (_isPlaying)
        {
            //pause stopwatch if time is freezed
            if(Time.timeScale == 0)
            {
                _isStopwatchStarted = true;
                once = false;
                _stopwatch.Stop();
            }
            else
            {
                if (!once)
                {
                    if (_isStopwatchStarted)
                    {
                        _isStopwatchStarted = false;
                    }
                    once = true;
                }
            }

            if (!_isStopwatchStarted)
            {
                _stopwatch.Start();
                _isStopwatchStarted = true;
            }

            _millis = (_stopwatch.ElapsedMilliseconds / 10) % 100;
            _seconds = (int)_stopwatch.ElapsedMilliseconds / 1000 % 60;
            _minutes = (int)_stopwatch.ElapsedMilliseconds / 1000 / 60;
            StopwatchText.text =  _minutes.ToString("00") + ":" + _seconds.ToString("00") + ":" + _millis.ToString("00");
        }
    }

    public void TriggerStopwatch()
    {
        _isPlaying = true;
    }


    public void StartStopwatch()
    {
        _stopwatch.Start();
        _isPlaying = true;
    }


    public void StopStopwatch()
    {
        _stopwatch.Stop();
        _isPlaying = false;
    }
}
