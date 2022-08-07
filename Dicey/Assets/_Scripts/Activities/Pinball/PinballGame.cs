using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PinballGame : MonoBehaviour
{
    public CinemachineVirtualCamera PinballCamera;
    public Transform StartPoint;
    public GameObject LeftArm;
    public GameObject RightArm;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>().EnableCamera(PinballCamera);
            DiceController.PLAYER_MESH.transform.position = StartPoint.position;
        }
    }
}
