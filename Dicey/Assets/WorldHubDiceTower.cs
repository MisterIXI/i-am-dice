using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class WorldHubDiceTower : MonoBehaviour
{
    Transform _playerTransform;
    CinemachineVirtualCamera _cinemachineCam;
    public CameraManager CameraManager;
    CinemachineBlendDefinition _style;
    private void Start()
    {
        try
        {
            _cinemachineCam = gameObject.transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
            if (_cinemachineCam == null)
                throw new NullReferenceException();
        }
        catch (NullReferenceException ex)
        {
            Debug.LogError("Dice Tower could not find tower camera! Make sure to place a virtual camera as the first child of Dice Tower! " + ex.Message);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            // CameraManager.PlayerCamera.Follow = null;
            // CameraManager.PlayerCamera.LookAt = null;
            _style = CameraManager.GetCinemachineBrain().m_DefaultBlend;
            CameraManager.GetCinemachineBrain().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
            CameraManager.EnableCamera(_cinemachineCam);
            _playerTransform = other.gameObject.transform;
            _cinemachineCam.LookAt = _playerTransform;
            StartCoroutine(ResetCamera());
        }
    }

    IEnumerator ResetCamera()
    {
        // Transform _followTarget = _playerTransform.parent.GetComponentInChildren<FollowTarget>().gameObject.transform;
        // _followTarget.localRotation = _cinemachineCam.transform.localRotation;
        yield return new WaitForSecondsRealtime(3);
        // CameraManager.PlayerCamera.transform.rotation = _cinemachineCam.transform.rotation;
        // CameraManager.PlayerCamera.transform.position = _cinemachineCam.transform.position;
        // CameraManager.PlayerCamera.Follow = _followTarget;
        // CameraManager.PlayerCamera.LookAt = _followTarget;
        CameraManager.GetComponent<CinemachineBrain>().m_DefaultBlend = _style;
        CameraManager.EnablePlayerCamera();
    }
}
