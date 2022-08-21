using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ReferenceManager : MonoBehaviour
{
    public static GameObject REFERENCE_MANAGER;
    public static GameObject PLAYER { get; private set; }
    public static GameObject CAMERA_MANAGER { get; private set; }
    public static GameObject MENU_CANVAS { get; private set; }
    public static GameObject INGAME_UI { get; private set; }
    public static GameObject PAUSE_UI { get; private set; }

    [field: SerializeField]
    public GameObject PLAYER_SPAWN_POINT { get; private set; }
    [field: SerializeField]
    public GameObject MENU_CAMERA_START { get; private set; }

    [Space(15)]
    public bool DebugMode = false;
    [Header("Prefabs")]
    public GameObject PlayerPrefab;
    public GameObject MenuCanvasPrefab;
    public GameObject IngameUIPrefab;
    public GameObject CameraManagerPrefab;
    public GameObject PauseUIPrefab;
    [Header("Toggle spawning of prefabs")]
    public bool SpawnPlayer = true;
    public bool SpawnCameraManager = true;
    public bool SpawnMenuCanvas = true;
    public bool SpawnIngameUI = true;
    public bool SpawnPauseUI = true;
    [Header("Special actions")]
    public bool SkipMenu = false;



    private void Awake()
    {
        //disable placeholder camera
        GetComponentInChildren<Camera>().gameObject.SetActive(false);
        // Reference manager
        REFERENCE_MANAGER = gameObject;
        // Player
        if (SpawnPlayer)
        {
            PLAYER = Instantiate(PlayerPrefab);
            PLAYER.transform.position = PLAYER_SPAWN_POINT.transform.position;
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
        PLAYER_SPAWN_POINT.GetComponent<MeshRenderer>().enabled = false;
        if (SpawnCameraManager)
            CAMERA_MANAGER = Instantiate(CameraManagerPrefab);
        else
            CAMERA_MANAGER = GameObject.FindObjectOfType<CameraManager>().gameObject;

        if (SpawnIngameUI)
            INGAME_UI = Instantiate(IngameUIPrefab);
        else
            INGAME_UI = GameObject.FindObjectOfType<IngameUI>().transform.parent.parent.gameObject;

        if (SpawnMenuCanvas)
        {
            MENU_CANVAS = Instantiate(MenuCanvasPrefab);
            MENU_CANVAS.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.transform.position = MENU_CAMERA_START.transform.position;
        }
        else
            MENU_CANVAS = GameObject.FindObjectOfType<MainMenu>().transform.parent.gameObject;

        // MENU_CANVAS.GetComponentInChildren<MainMenu>().DebugSkipMenu = SkipMenu;
        MENU_CANVAS.transform.GetComponentInChildren<MainMenu>().DebugSkipMenu = SkipMenu;

        if (SpawnPauseUI)
        {
            PAUSE_UI = Instantiate(PauseUIPrefab);
            // PAUSE_UI.SetActive(false);
        }
        else
        {
            PAUSE_UI = GameObject.FindObjectOfType<PauseMenu>().transform.parent.gameObject;
        }
    }
    private void Start()
    {

    }


}