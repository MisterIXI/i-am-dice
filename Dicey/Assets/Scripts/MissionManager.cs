using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MissionManager : MonoBehaviour
{
    public enum MissionGoals
    {
        RollToSide,
        JumpIntoDiceCup,
        ReachDestination
    }

    public MissionGoals mission;


    // Start is called before the first frame update
    void Start()
    {
        ChooseMission();
        Time.timeScale = 1.5f;
    }

    private void Update()
    {
        
    }


    public void ChooseMission()
    {
        mission = (MissionGoals)Random.Range(0, Enum.GetNames(typeof(MissionGoals)).Length);

        switch (mission)
        {
            case MissionGoals.RollToSide:
                Debug.Log("Roll on side " + GetRandomDiceFace() + "!");
                break;
            case MissionGoals.JumpIntoDiceCup:
                Debug.Log("Jump into dice cup!");
                break;
            case MissionGoals.ReachDestination:
                Debug.Log("Reach destination!");
                break;
        }
    }


    int GetRandomDiceFace()
    {
        return Random.Range(1, 7);
    }
}
