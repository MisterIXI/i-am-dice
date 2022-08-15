using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.SceneManagement;

public class WorldHubDiceTower : MonoBehaviour
{
    public int NextLevelBuildIndex;
    private CameraManager _cameraManager;
    public bool ApplyTransformToFollowTarget = true;
    Transform _playerTransform;
    CinemachineVirtualCamera _cinemachineCam;
    CinemachineBlendDefinition _style;
    bool _canLevelSwitch;
    private void Start()
    {
        _cameraManager = ReferenceManager.CAMERA_MANAGER.GetComponent<CameraManager>();
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
            _cameraManager.GetComponent<CameraControl>().enabled = false;
            _style = _cameraManager.GetCinemachineBrain().m_DefaultBlend;
            _cameraManager.GetCinemachineBrain().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
            _cameraManager.EnableCamera(_cinemachineCam);
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
        //yield return new WaitForSecondsRealtime(3);
        yield return new WaitUntil(() => _canLevelSwitch == true);
        Debug.Log("level switch initialized");
        asyncLoad.allowSceneActivation = true;
        asyncLoad.completed += SceneLoadFinished;
    }

    private void SceneLoadFinished(AsyncOperation obj)
    {
        SceneManager.MoveGameObjectToScene(DiceController.PLAYER, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        StartCoroutine(CameraTransitionToPlayer());
    }

    public void EnableLevelSwitch()
    {
        _canLevelSwitch = true;
    }
    IEnumerator CameraTransitionToPlayer()
    {
        yield return new WaitForEndOfFrame();
        if (ApplyTransformToFollowTarget)
        {
            Transform _followTarget = _playerTransform.parent.GetComponentInChildren<FollowTarget>().gameObject.transform;
            _followTarget.localRotation = _cinemachineCam.transform.rotation;
        }
        _cameraManager = GameObject.FindObjectOfType<CameraManager>();
        // Debug.Log(CameraManager);
        _cameraManager.GetComponent<CinemachineBrain>().m_DefaultBlend = _style;
        _cameraManager.EnablePlayerCamera();
        DiceController.PLAYER.GetComponentInChildren<DiceController>().IsMovementLocked = false;

        yield return new WaitForSecondsRealtime(_cameraManager.GetCinemachineBrain().m_DefaultBlend.m_Time);
        //CameraManager.GetComponent<CameraControl>().enabled = true;
    }
}
