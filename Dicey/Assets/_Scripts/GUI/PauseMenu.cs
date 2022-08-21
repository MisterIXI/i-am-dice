using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class PauseMenu : MonoBehaviour
{
    public TMP_Text PauseText;
    public float MinSize = 50f;
    public float AnimationSpeed = 2f;
    public float PulseInkrement = 10f;
    [Header("InternalReferences")]
    public GameObject ContinueButton;
    public GameObject OptionsButton;
    public GameObject QuitButton;


    private void OnEnable()
    {
        GameObject.FindObjectOfType<EventSystem>().SetSelectedGameObject(ContinueButton);
    }

    // Update is called once per frame
    void Update()
    {
        // pulse pausetext size with sin wave
        PauseText.fontSize = Mathf.Sin(Time.unscaledTime * AnimationSpeed) * PulseInkrement + MinSize;
    }

    public void ContinueGame()
    {
        ReferenceManager.PLAYER.GetComponentInChildren<DiceController>().TogglePauseInternal();
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
