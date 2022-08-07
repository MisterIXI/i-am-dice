using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{

    public static GameObject PLAYER_OBJECT;
    public static CinemachineVirtualCamera PLAYER_CAMERA;
    public static Transform PLAYER_TARGET;
    private void Start()
    {
        PLAYER_OBJECT = DiceController.PLAYER;
        PLAYER_CAMERA = PLAYER_OBJECT.GetComponentInChildren<CinemachineVirtualCamera>();
        PLAYER_TARGET = PLAYER_OBJECT.GetComponentInChildren<FollowTarget>().transform;
        DontDestroyOnLoad(gameObject);
    }
}
