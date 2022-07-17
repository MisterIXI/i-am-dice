using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCam : MonoBehaviour
{
    public Transform Target;
    public float Speed = 5f;
    public float CameraDistance = 10f;
    private float _cameraZoom = 0;
    private Vector3 _direction;
    private Vector2 _directionChange;
    private DiceController _diceController;
    // Start is called before the first frame update
    void Start()
    {
        _diceController = Target.gameObject.GetComponent<DiceController>();
        _direction = new Vector3(1, 0, 1).normalized * CameraDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_directionChange.x != 0 || _directionChange.y != 0)
        {
            _diceController.updateMovement(_diceController.CurrentInput);
        }
        // Debug.Log("CameraDistance: " + cameraDistance+ " CameraZoom: " + cameraZoom);
        CameraDistance = Mathf.Clamp(CameraDistance + _cameraZoom, 5f, 20f);

        float oldX = transform.rotation.y;
        transform.position = Target.position + _direction;
        transform.LookAt(Target);
        transform.RotateAround(Target.position, Vector3.up, _directionChange.x * Speed);
        float angle = Vector3.Angle(new Vector3(_direction.x, 0, _direction.z), _direction);

        if (_direction.y > 0)
        {
            if (angle - _directionChange.y * Speed < 90f)
                transform.RotateAround(Target.position, transform.right, -_directionChange.y * Speed);
        }
        else
        {
            if (angle + _directionChange.y * Speed < 90f)
                transform.RotateAround(Target.position, transform.right, -_directionChange.y * Speed);
        }

        _direction = (transform.position - Target.position).normalized * CameraDistance;

    }

    public void RotateCamera(InputAction.CallbackContext context)
    {
        if (context.action.name == "RotateCamera")
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                // Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                if (context.action.activeControl.device.name == "Mouse")
                {
                    _directionChange = context.action.ReadValue<Vector2>() * 0.2f;
                }
                else
                {
                    _directionChange = context.action.ReadValue<Vector2>();
                }
                // Debug.Log("Direction change: " + directionChange);
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _directionChange = new Vector2(0, 0);
            }
        }
    }

    public void ZoomCamera(InputAction.CallbackContext context)
    {
        if (context.action.name == "ZoomCamera")
        {
            if (context.action.phase == InputActionPhase.Performed)
            {
                // Debug.Log("Zoom: " + context.action.ReadValue<float>());
                if (context.action.activeControl.device.name == "Mouse")
                {
                    CameraDistance += context.action.ReadValue<float>() < 0 ? 3 : -3;
                }
                else
                {
                    _cameraZoom = context.action.ReadValue<float>();
                }
                // Debug.Log("Zoom: " + cameraZoom);
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _cameraZoom = 0;
            }
        }
    }

}
