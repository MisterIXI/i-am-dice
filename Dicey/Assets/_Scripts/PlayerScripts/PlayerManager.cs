using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public void InitManager()
    {
        DontDestroyOnLoad(gameObject);
    }
}
