using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{

    public GameObject FollowTarget;
    public GameObject Dice;
    public GameObject PlayerCamera;

    public void InitManager(){
        FollowTarget = GetComponentInChildren<FollowTarget>().gameObject;
        Dice = GetComponentInChildren<DiceController>().gameObject;
        PlayerCamera = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
        DontDestroyOnLoad(gameObject);
    }
}
