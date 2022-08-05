using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.SceneManagement;

public class WorldHubDiceTower : MonoBehaviour
{
    public int NextLevelBuildIndex;
    public CameraManager CameraManager;
    public bool ApplyTransformToFollowTarget = true;
    Transform _playerTransform;
    CinemachineVirtualCamera _cinemachineCam;
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
            CameraManager.GetComponent<CameraControl>().enabled = false;
            _style = CameraManager.GetCinemachineBrain().m_DefaultBlend;
            CameraManager.GetCinemachineBrain().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
            CameraManager.EnableCamera(_cinemachineCam);
            _playerTransform = other.gameObject.transform;
            _cinemachineCam.LookAt = _playerTransform;
            DiceController.PLAYER.GetComponentInChildren<DiceController>().IsMovementLocked = true;
            StartCoroutine(ResetCamera());
        }
    }

    IEnumerator ResetCamera()
    {
        AsyncOperation asyncLoad = WorldHubManager.LoadLevelAsync(NextLevelBuildIndex);
        DontDestroyOnLoad(DiceController.PLAYER);
        DontDestroyOnLoad(gameObject);
        //wait for player to fall down before moving camera
        yield return new WaitForSecondsRealtime(3);
        asyncLoad.allowSceneActivation = true;
        asyncLoad.completed += SceneLoadFinished;
    }

    private void SceneLoadFinished(AsyncOperation obj)
    {
        SceneManager.MoveGameObjectToScene(DiceController.PLAYER, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        StartCoroutine(CameraTransitionToPlayer());
    }

    IEnumerator CameraTransitionToPlayer()
    {
        yield return new WaitForEndOfFrame();
        if (ApplyTransformToFollowTarget)
        {
            Transform _followTarget = _playerTransform.parent.GetComponentInChildren<FollowTarget>().gameObject.transform;
            _followTarget.localRotation = _cinemachineCam.transform.rotation;
        }
        CameraManager = GameObject.FindObjectOfType<CameraManager>();
        // Debug.Log(CameraManager);
        CameraManager.GetComponent<CinemachineBrain>().m_DefaultBlend = _style;
        CameraManager.EnablePlayerCamera();
        DiceController.PLAYER.GetComponentInChildren<DiceController>().IsMovementLocked = false;

        yield return new WaitForSecondsRealtime(CameraManager.GetCinemachineBrain().m_DefaultBlend.m_Time);
        //CameraManager.GetComponent<CameraControl>().enabled = true;
    }
}
