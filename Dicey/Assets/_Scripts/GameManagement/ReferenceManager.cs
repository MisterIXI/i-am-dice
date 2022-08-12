using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static GameObject REFERENCE_MANAGER;
    public static GameObject PLAYER { get; private set; }
    public static GameObject CAMERA_MANAGER { get; private set; }
    public static GameObject MENU_CANVAS { get; private set; }
    public static GameObject INGAME_UI { get; private set; }

    public GameObject PlayerPrefab;
    public GameObject MenuCanvasPrefab;
    public GameObject IngameUIPrefab;
    public GameObject CameraManagerPrefab;
    public bool SpawnPlayer = true;
    public bool SpawnCameraManager = true;
    public bool SpawnMenuCanvas = true;
    public bool SpawnIngameUI = true;
    public bool SkipMenu = false;

    private void Awake()
    {
        // Reference manager
        REFERENCE_MANAGER = gameObject;
        // Player
        if (SpawnPlayer)
        {
            PLAYER = Instantiate(PlayerPrefab);
            PLAYER.transform.position = transform.position;
        }
        else
        {
            //try to find player
            PLAYER = GameObject.FindGameObjectWithTag("Player");
            if (PLAYER == null)
            {
                PLAYER = DiceController.PLAYER;
            }
        }
        if (SpawnCameraManager)
            CAMERA_MANAGER = Instantiate(CameraManagerPrefab);
        else
            CAMERA_MANAGER = GameObject.FindObjectOfType<CameraManager>().gameObject;

        if (SpawnIngameUI)
            INGAME_UI = Instantiate(IngameUIPrefab);
        else
            INGAME_UI = GameObject.FindObjectOfType<IngameUI>().transform.parent.parent.gameObject;

        if (SpawnMenuCanvas)
            MENU_CANVAS = Instantiate(MenuCanvasPrefab);
        else
            MENU_CANVAS = GameObject.FindObjectOfType<MainMenu>().transform.parent.gameObject;

        // MENU_CANVAS.GetComponentInChildren<MainMenu>().DebugSkipMenu = SkipMenu;
        MENU_CANVAS.transform.GetChild(0).GetComponent<MainMenu>().DebugSkipMenu = SkipMenu;
        PLAYER.GetComponent<PlayerManager>().InitManager();
    }
    private void Start()
    {

    }


}