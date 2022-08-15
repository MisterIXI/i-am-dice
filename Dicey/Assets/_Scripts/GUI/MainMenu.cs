using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    private CameraManager _cameraManager;
    public float rotationSpeed;
    private GameObject _player;
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    private CinemachineVirtualCamera _mainMenuCamera;
    private CinemachineVirtualCamera _playerCamera;
    private IngameUI _ingameUI;

    public bool DebugSkipMenu = false;

    void Start()
    {
        _cameraManager = ReferenceManager.CAMERA_MANAGER.GetComponent<CameraManager>();
        _player = ReferenceManager.PLAYER;
        _mainMenuCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _playerCamera = _player.GetComponentInChildren<CinemachineVirtualCamera>();
        _ingameUI = ReferenceManager.INGAME_UI.GetComponentInChildren<IngameUI>();
        if (DebugSkipMenu)
        {
            StartCoroutine(DelayedAutoStart());
        }
    }
    
    void Update()
    {
        if(_mainMenuCamera != null)
                _mainMenuCamera.transform.RotateAround(_mainMenuCamera.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }


    public void StartGame()
    {
        _ingameUI.TriggerStopwatch();
        mainMenuPanel.SetActive(false);
        _cameraManager.EnableCamera(_playerCamera);
        _player.SetActive(true);
        if(ReferenceManager.INGAME_UI != null)
            ReferenceManager.INGAME_UI.GetComponentInChildren<IngameUI>().StartStopwatch();
    }

    public void OpenOptionsMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    public void OpenCreditsMenu()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    IEnumerator DelayedAutoStart()
    {
        // delay the start for other code to finish
        yield return new WaitForSeconds(0.1f);
        StartGame();
    }
}
