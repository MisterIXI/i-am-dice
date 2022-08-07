using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelWon : MonoBehaviour
{
    public CinemachineVirtualCamera WinCamera;
    public CameraManager CameraManager;
    public void ShowWinCamera()
    {
        StartCoroutine(DisplayCamera());
    }

    IEnumerator DisplayCamera()
    {
        CameraManager.AddCameraToList(WinCamera);
        CameraManager.EnableCamera(WinCamera);
        yield return new WaitForSecondsRealtime(3);
        CameraManager.EnablePlayerCamera();
    }
}
