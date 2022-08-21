using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Timescale;
    public static GameObject GAME_MANAGER;

    void Awake(){
        GAME_MANAGER = this.gameObject;
    }

    void Start()
    {
        Time.timeScale = Timescale;
    }
}
