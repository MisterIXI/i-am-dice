using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject player;
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public GameObject mainMenuCamera;
    public IngameUI IngameUI;
    // Update is called once per frame
    void Update()
    {
        mainMenuCamera.transform.RotateAround(mainMenuCamera.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }


    public void StartGame()
    {
        player.SetActive(true);
        IngameUI.TriggerStopwatch();
        mainMenuPanel.SetActive(false);
        mainMenuCamera.SetActive(false);
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
}
