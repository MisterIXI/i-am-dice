using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject FollowTarget { get; private set; }
    [HideInInspector]
    public GameObject Dice { get; private set; }
    [HideInInspector]
    public GameObject PlayerCamera { get; private set; }

    public void InitManager()
    {
        FollowTarget = GetComponentInChildren<FollowTarget>().gameObject;
        Dice = GetComponentInChildren<DiceController>().gameObject;
        PlayerCamera = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
        DontDestroyOnLoad(gameObject);
    }
}
