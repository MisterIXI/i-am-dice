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
        yield return new WaitForSecondsRealtime(3);
        CameraManager.GetComponent<CinemachineBrain>().m_DefaultBlend = _style;
        CameraManager.EnablePlayerCamera();
    }
}
