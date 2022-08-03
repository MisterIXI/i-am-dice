using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    public CameraManager CameraManager;
    public float rotationSpeed;
    public GameObject player;
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public CinemachineVirtualCamera MainMenuCamera;
    public CinemachineVirtualCamera PlayerCamera;
    public IngameUI IngameUI;

    public bool DebugSkipMenu = false;

    void Start()
    {
        if (DebugSkipMenu)
        {
            StartCoroutine(DelayedAutoStart());
        }
    }
    
    void Update()
    {
        MainMenuCamera.transform.RotateAround(MainMenuCamera.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }


    public void StartGame()
    {

        IngameUI.TriggerStopwatch();
        mainMenuPanel.SetActive(false);
        CameraManager.EnableCamera(PlayerCamera);
        player.SetActive(true);
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
