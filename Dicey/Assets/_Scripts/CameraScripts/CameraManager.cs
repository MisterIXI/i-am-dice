using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;


public class CameraManager : MonoBehaviour
{
    CinemachineBrain _cinemachineBrain;
    public CinemachineVirtualCamera MainMenuCamera;
    public CinemachineVirtualCamera PlayerCamera;
    public CinemachineVirtualCamera[] Cameras;

    public bool IsSceneLevel;
    public static CameraManager cameraManager;
    HashSet<CinemachineVirtualCamera> _cameraSet;
    // Start is called before the first frame update
    void Start()
    {
        _cameraSet = new HashSet<CinemachineVirtualCamera>();
        cameraManager = GetComponent<CameraManager>();
        foreach (CinemachineVirtualCamera camera in Cameras)
        {
            camera.enabled = false;
        }
        if (MainMenuCamera != null)
            MainMenuCamera.enabled = true;
        _cinemachineBrain = GetComponent<CinemachineBrain>();
        UniversalAdditionalCameraData cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(ReferenceManager.INGAME_UI.GetComponent<Camera>());

        InitializePlayerCam();
    }
    public void InitializePlayerCam()
    {
        if (PlayerCamera == null)
            PlayerCamera = DiceController.PLAYER.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void EnableCamera(CinemachineVirtualCamera camera)
    {
        foreach (CinemachineVirtualCamera _camera in Cameras)
        {
            _camera.enabled = false;
        }

        foreach(CinemachineVirtualCamera _camera in _cameraSet)
        {
            _camera.enabled = false;
        }

        camera.enabled = true;
    }

    public void EnablePlayerCamera()
    {
        InitializePlayerCam();
        foreach (CinemachineVirtualCamera camera in Cameras)
        {
            camera.enabled = false;
        }
        foreach (CinemachineVirtualCamera _camera in _cameraSet)
        {
            _camera.enabled = false;
        }
        PlayerCamera.enabled = true;
    }

    public CinemachineBrain GetCinemachineBrain()
    {
        return _cinemachineBrain;
    }

    public void AddCameraToList(CinemachineVirtualCamera camera)
    {
        _cameraSet.Add(camera);
    }
}
