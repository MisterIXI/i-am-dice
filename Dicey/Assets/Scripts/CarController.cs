using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour, IControllable
{
    private bool _isActive = false;
    private bool _isBraking = false;
    private bool _isReversing = false;
    private enum CarState
    {
        Active,
        Braking,
        Reversing
    }
    Collider _triggerZone;
    Vector2 _movement;

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float maxBrakeTorque;
    public Material HeadLightsMaterial, TailLightsMaterial;
    private bool _hasTriggered = false;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // expected first child of car to be the trigger zone
        GameObject tempObj = transform.GetChild(0).gameObject;
        if (tempObj == null || tempObj.name != "TriggerZone")
        {
            gameObject.SetActive(false);
            throw new System.Exception("TriggerZone not found");
        }
        _triggerZone = tempObj.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        float steering = maxSteeringAngle * _movement.x;

        float motor = maxMotorTorque * (_movement.y + (_movement.y < 0 ? -Mathf.Abs(_movement.x) : Mathf.Abs(_movement.x)));
        // Debug.Log("_movement: " + _movement);
        Debug.Log("motor: " + motor);
        foreach (AxleInfo axle in axleInfos)
        {

            // steering
            if (axle.steering)
            {
                axle.leftWheel.steerAngle = steering;
                axle.rightWheel.steerAngle = steering;
                // rotate wheels to match steering angle
                axle.leftWheel.transform.localRotation = Quaternion.Euler(0, steering, 0);
                axle.rightWheel.transform.localRotation = Quaternion.Euler(0, steering, 0);
            }
            // acceleration
            if (axle.motor)
            {
                axle.leftWheel.motorTorque = motor;
                axle.rightWheel.motorTorque = motor;
            }
            // braking
            if (_rb.velocity.z > 0 && motor < 0)
            {
                axle.leftWheel.brakeTorque = maxBrakeTorque;
                axle.rightWheel.brakeTorque = maxBrakeTorque;
            }
            else if (_rb.velocity.z < 0 && motor > 0)
            {
                axle.leftWheel.brakeTorque = maxBrakeTorque;
                axle.rightWheel.brakeTorque = maxBrakeTorque;
            }
            else
            {
                axle.leftWheel.brakeTorque = 0;
                axle.rightWheel.brakeTorque = 0;
            }
        }
    }


    public void ReceiveInput(InputAction.CallbackContext context)
    {
        if (context.action.name == "MoveDice")
        {
            if (context.action.phase == InputActionPhase.Performed)
            {
                _movement = context.ReadValue<Vector2>();
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _movement = Vector2.zero;
            }
        }

    }

    private void HandleLights(CarState state, bool isOn)
    {
        switch (state)
        {
            case CarState.Active:
                if (isOn)
                {
                }
                else
                {
                    if(_isReversing){
                        _isReversing = false;
                        HandleLights(CarState.Reversing, false);
                    }
                    if(_isBraking){
                        _isBraking = false;
                        HandleLights(CarState.Braking, false);
                    }
                }
                break;
            case CarState.Braking:
                HeadLightsMaterial.DisableKeyword("_EMISSION");
                TailLightsMaterial.EnableKeyword("_EMISSION");
                break;
            case CarState.Reversing:
                HeadLightsMaterial.DisableKeyword("_EMISSION");
                TailLightsMaterial.EnableKeyword("_EMISSION");
                break;
        }
    }

    public void PlayerCollisionEnter(Collider other)
    {
        _movement = other.gameObject.GetComponent<DiceController>().RegisterControllable(this);
    }
    public void PlayerCollisionExit(Collider other)
    {
        other.gameObject.GetComponent<DiceController>().UnregisterControllable(this);
        _movement = Vector2.zero;
    }



    // https://docs.unity.cn/540/Documentation/Manual/WheelColliderTutorial.html
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
        public override string ToString()
        {
            string result = "Axle:(LeftWheel: " + leftWheel + ", RightWheel: " + rightWheel + ", Motor: " + motor + ", Steering: " + steering + ")";
            return result;
        }
    }
}
