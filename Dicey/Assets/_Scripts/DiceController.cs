using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
public class DiceController : MonoBehaviour
{
    private float JUMP_COOLDOWN = 1f;
    private float RAYCAST_DISTANCE_INNER = 2.3f;
    private float RAYCAST_DISTANCE_OUTER = 1.5f;

    public GameObject JumpParticleGO;

    private Vector2 _movement;
    [HideInInspector]
    public Vector2 CurrentInput;
    Rigidbody _rb;
    public float JumpStrength = 800;
    public float Offset = 0.5f;
    public Color BaseColor;
    private bool _isJumping = false;
    private bool _isOnJumpCooldown = false;
    public bool InfiniteJumpsEnabled = false;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Material _material;
    private Material _material2;
    public static GameObject PLAYER { get; private set; }
    public static GameObject PLAYER_MESH { get; private set; }
    [HideInInspector]
    public bool IsMovementLocked = false;
    private List<IControllable> _controllables;

    [SerializeField]
    private CameraControl _cameraControl;
    private PlayerInput _playerInput;
    private void Awake()
    {
        PLAYER = transform.parent.gameObject;
        PLAYER_MESH = transform.gameObject;
    }
    void Start()
    {
        BaseColor = GetComponent<MeshRenderer>().material.color;
        _movement = new Vector2(0, 0);
        _rb = GetComponent<Rigidbody>();
        CurrentInput = new Vector2(0, 0);
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _material = GetComponent<MeshRenderer>().material;
        _material2 = GetComponent<MeshRenderer>().materials[1];
        _controllables = new List<IControllable>();
        transform.parent.gameObject.SetActive(false);
    }


    void InitializePlayerInputSystem()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.currentActionMap.FindAction("MoveDice", true).performed += MoveDice;
        _playerInput.currentActionMap.FindAction("RotateCamera", true).performed += _cameraControl.RotateCamera;
        _playerInput.currentActionMap.FindAction("Jump", true).performed += Jump;
        _playerInput.currentActionMap.FindAction("ZoomCamera", true).performed += _cameraControl.ZoomCamera;
        _playerInput.currentActionMap.FindAction("ResetPosition", true).performed += ResetPosition;

        _playerInput.currentActionMap.FindAction("MoveDice", true).canceled += MoveDice;
        _playerInput.currentActionMap.FindAction("RotateCamera", true).canceled += _cameraControl.RotateCamera;
        _playerInput.currentActionMap.FindAction("Jump", true).canceled += Jump;
        _playerInput.currentActionMap.FindAction("ZoomCamera", true).canceled += _cameraControl.ZoomCamera;
        _playerInput.currentActionMap.FindAction("ResetPosition", true).canceled += ResetPosition;
    }

    private void OnLevelWasLoaded(int level)
    {
        _cameraControl = FindObjectOfType<CameraControl>();
        InitializePlayerInputSystem();
        Debug.Log("Level loaded");
    }

    void FixedUpdate()
    {
        if (IsMoving() && !IsMovementLocked)
        {
            _rb.AddForce(GetMovementVector());
        }
        CheckForFloor();
    }

    private bool IsMoving()
    {
        return _movement.x != 0 || _movement.y != 0;
    }


    public Vector2 RegisterControllable(IControllable controllable)
    {
        if (!_controllables.Contains(controllable))
        {
            _controllables.Add(controllable);
            if (_controllables.Count == 1)
            {
                IsMovementLocked = true;
            }
            return _movement;
        }
        return Vector2.zero;
    }

    public void UnregisterControllable(IControllable controllable)
    {
        if (_controllables.Contains(controllable))
        {
            _controllables.Remove(controllable);
            if (_controllables.Count == 0)
            {
                IsMovementLocked = false;
            }
        }
    }

    private Vector3 GetMovementVector()
    {
        return new Vector3(_movement.x, 0, _movement.y) * 10;
    }
    public void MoveDice(InputAction.CallbackContext context)
    {
        if (context.action.name == "MoveDice")
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                // Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                UpdateMovement(context.action.ReadValue<Vector2>());
                CurrentInput = context.action.ReadValue<Vector2>();
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _movement = new Vector2(0, 0);
                CurrentInput = new Vector2(0, 0);
            }
            foreach (var controllable in _controllables)
            {
                controllable.ReceiveInput(context);
            }
        }
    }

    public void UpdateMovement(Vector2 currentMovement)
    {
        // adjust Vector2 to match the camera angle
        Vector2 adjustedMovement = currentMovement;
        Vector2 viewDir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        // -1.5708f is the constant angle of the calculation
        float angle = Mathf.Atan2(viewDir.y, viewDir.x) - 1.5708f;
        adjustedMovement = new Vector2(
            adjustedMovement.x * Mathf.Cos(angle) - adjustedMovement.y * Mathf.Sin(angle),
            adjustedMovement.x * Mathf.Sin(angle) + adjustedMovement.y * Mathf.Cos(angle));
        _movement = adjustedMovement;
        // Debug.Log("Movement: " + _movement + " currentMovement: " + currentMovement);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.name == "Jump")
        {
            if (!_isJumping && !_isOnJumpCooldown || InfiniteJumpsEnabled)
            {

                if (context.action.phase == InputActionPhase.Performed)
                {
                    //Instantiate JumpParticleGO at ground position 
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
                    {
                        GameObject jumpEffect = Instantiate(JumpParticleGO);
                        jumpEffect.transform.position = hit.point;
                        Destroy(jumpEffect, 2);
                    }
                    _rb.AddForce(new Vector3(0, JumpStrength, 0));
                    _rb.AddTorque(new Vector3(Random.Range(1f, 2f) * JumpStrength, Random.Range(1f, 2f) * JumpStrength, Random.Range(1f, 2f) * JumpStrength));
                    StartCoroutine(JumpCooldown());
                }
                else if (context.action.phase == InputActionPhase.Canceled)
                {
                    // Debug.Log("Canceled");
                }
            }
            foreach (var controllable in _controllables)
            {
                controllable.ReceiveInput(context);
            }
        }
    }

    private string _debugString = "";
    public bool IsOnFloor()
    {
        bool isOnFloor = false;
        isOnFloor = CheckFloorRaycast(transform.position, RAYCAST_DISTANCE_INNER);
        isOnFloor = !isOnFloor ? CheckFloorRaycast(transform.position + new Vector3(Offset, 0, Offset), RAYCAST_DISTANCE_OUTER) : isOnFloor;
        isOnFloor = !isOnFloor ? CheckFloorRaycast(transform.position + new Vector3(-Offset, 0, Offset), RAYCAST_DISTANCE_OUTER) : isOnFloor;
        isOnFloor = !isOnFloor ? CheckFloorRaycast(transform.position + new Vector3(Offset, 0, -Offset), RAYCAST_DISTANCE_OUTER) : isOnFloor;
        isOnFloor = !isOnFloor ? CheckFloorRaycast(transform.position + new Vector3(-Offset, 0, -Offset), RAYCAST_DISTANCE_OUTER) : isOnFloor;
        return isOnFloor;
    }
    private void CheckForFloor()
    {
        bool isOnFloor = IsOnFloor();
        if (isOnFloor)
        {
            _isJumping = false;
            // _material2.color = Color.green;
        }
        else
        {
            _isJumping = true;
            // _material2.color = Color.red;
        }
    }
    public void SpawnDot(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            DotAnimation.SpawnDot(transform.position + transform.forward * 5f);
        }
    }
    private bool CheckFloorRaycast(Vector3 position, float distance)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, distance);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.tag != "DiceFace")
            {
                return true;
            }
        }
        return false;
    }
    private IEnumerator JumpCooldown()
    {
        _isOnJumpCooldown = true;
        yield return new WaitForSeconds(JUMP_COOLDOWN);
        _isOnJumpCooldown = false;
    }

    public void ResetPosition(InputAction.CallbackContext context)
    {
        if (context.action.name == "ResetPosition")
        {
            if (context.action.phase == InputActionPhase.Performed)
            {
                transform.position = _initialPosition;
                transform.rotation = _initialRotation;
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
        }
    }
    // gizmos
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    // draw force arrow
    //    // Vector2 viewDir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z).normalized;
    //    // float angle = Mathf.Atan2(viewDir.y, viewDir.x) *Mathf.Rad2Deg;//Vector2.Angle(new Vector2(0, 1), viewDir);
    //    // if(angle < 0){
    //    //     GUI.color = Color.red;
    //    //     angle = 360 + angle;
    //    // }
    //    // else{
    //    //     GUI.color = Color.green;
    //    // }
    //    // Handles.Label(transform.position, "Angle: " + angle);

    //    Gizmos.DrawRay(transform.position, Vector3.down * RAYCAST_DISTANCE_INNER);
    //    Gizmos.DrawRay(transform.position + new Vector3(Offset, 0, Offset), Vector3.down * RAYCAST_DISTANCE_OUTER);
    //    Gizmos.DrawRay(transform.position + new Vector3(-Offset, 0, Offset), Vector3.down * RAYCAST_DISTANCE_OUTER);
    //    Gizmos.DrawRay(transform.position + new Vector3(Offset, 0, -Offset), Vector3.down * RAYCAST_DISTANCE_OUTER);
    //    Gizmos.DrawRay(transform.position + new Vector3(-Offset, 0, -Offset), Vector3.down * RAYCAST_DISTANCE_OUTER);
    //    Handles.Label(transform.position, _debugString);
    //    if (IsMoving())
    //    {
    //        Gizmos.DrawRay(transform.position, GetMovementVector());
    //    }
    //}
}
