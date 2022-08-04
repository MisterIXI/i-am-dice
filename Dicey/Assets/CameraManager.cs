using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineBrain _cinemachineBrain;
    public CinemachineVirtualCamera MainMenuCamera;
    public CinemachineVirtualCamera PlayerCamera;
    public CinemachineVirtualCamera[] Cameras;



    // Start is called before the first frame update
    void Start()
    {
        foreach(CinemachineVirtualCamera camera in Cameras)
        {
            camera.enabled = false;
        }
        MainMenuCamera.enabled = true;
        _cinemachineBrain = GetComponent<CinemachineBrain>();
    }


    public void EnableCamera(CinemachineVirtualCamera camera)
    {
        foreach (CinemachineVirtualCamera _camera in Cameras)
        {
            _camera.enabled = false;
        }
        camera.enabled = true;
    }

    public void EnablePlayerCamera()
    {
        foreach (CinemachineVirtualCamera camera in Cameras)
        {
            camera.enabled = false;
        }
        PlayerCamera.enabled = true;
    }

    public CinemachineBrain GetCinemachineBrain()
    {
        return _cinemachineBrain;
    }
}
