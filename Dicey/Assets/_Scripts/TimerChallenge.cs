using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TimerChallenge : MonoBehaviour
{
    public TMP_Text TimerText;
    public float TotalTime;
    public GameObject RewardDot;
    public Transform RewardSpawnPosition;

    bool hasChallengeStarted;
    GameObject spawnedReward;
    float _counter;
    
    float _millis;
    float _seconds;
    float startedTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!hasChallengeStarted)
            {
                TimerText.gameObject.SetActive(true);


                _counter = TotalTime;
                DisplayTime(TotalTime);

                spawnedReward = DotAnimation.SpawnDot(RewardSpawnPosition.position);
                StartCoroutine(StartCountdown());
            }
        }
    }

    IEnumerator StartCountdown()
    {
        //start countdown after spin animation is done.
        yield return new WaitForSecondsRealtime(3);
        hasChallengeStarted = true;
        startedTime = Time.realtimeSinceStartup;

    }

    private void Update()
    {

        if (hasChallengeStarted)
        {
            //if timer is still counting
            if (_counter > 0)
            {
                //destroy this challenge
                if(spawnedReward == null)
                {
                    TimerText.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
                //display proper timer on timer
                float elapsedTime = Time.realtimeSinceStartup - startedTime;
                _counter = TotalTime - elapsedTime;
                DisplayTime(_counter);
            }
            else
            {   
                //challenge will be resetted...
                hasChallengeStarted = false;
                TimerText.gameObject.SetActive(false);
                if (spawnedReward != null)
                {
                    Destroy(spawnedReward);
                }
            }
        }
    }



    void DisplayTime(float _counter)
    {
        _millis = (_counter * 100) % 100;
        _seconds = (int)_counter % 60;
        TimerText.text = _seconds.ToString("00") + ":" + _millis.ToString("00");
    }
}


