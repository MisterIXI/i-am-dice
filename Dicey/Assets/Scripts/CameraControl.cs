using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class CameraControl : MonoBehaviour
{
    private float _cameraDistance;
    private float _cameraZoom = 0;
    public float Speed = 10;
    private Vector2 _directionChange;
    public CinemachineVirtualCamera VirtualCamera;
    private Cinemachine3rdPersonFollow _followCam;
    public LayerMask IgnoreLayer;
    public Transform Target;
    private HashSet<GameObject> _affectedMat;

    Quaternion _camRotation;
    public float LookUpMax = 150;
    public float LookDownMax = 20;
    void Start()
    {
        _followCam = VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        _cameraDistance = _followCam.CameraDistance;
        _affectedMat = new HashSet<GameObject>();
        _camRotation = transform.localRotation;
    }

    void Update()
    {
        // Rotate left/right
        Target.transform.RotateAround(Target.transform.position, Vector3.up, -_directionChange.x * Time.deltaTime * Speed);

        // Rotate up/down
        float angle = _directionChange.y * Time.deltaTime * Speed;
        float currentAngle = transform.localRotation.eulerAngles.x;
        // Rotate frame of reference
        currentAngle += 90f;
        currentAngle = currentAngle % 360;
        // clamp angle for full rotations
        angle = angle % 180f;
        // check for out of bounds
        if (currentAngle + angle > LookUpMax)
        {
            angle = LookUpMax - currentAngle;
        }
        else if (currentAngle + angle < LookDownMax)
        {
            angle = LookDownMax - currentAngle;
        }
        // do the actual rotation
        Target.transform.RotateAround(Target.transform.position, Target.transform.right, angle);


        if (_cameraZoom != 0)
        {
            _cameraDistance = Mathf.Clamp(_cameraDistance + _cameraZoom, 2f, 20f);
            _followCam.CameraDistance = _cameraDistance;
        }

        CheckCameraCollision();
    }

    public void RotateCamera(InputAction.CallbackContext context)
    {
        if (context.action.name == "RotateCamera")
        {
            if (context.action.phase == InputActionPhase.Performed)
            {
                if (context.action.activeControl.device.name == "Mouse")
                {
                    _directionChange = -context.action.ReadValue<Vector2>() * 0.2f;
                    //_directionChange = new Vector2(-_directionChange.x, _directionChange.y);
                }
                else
                {
                    _directionChange = -context.action.ReadValue<Vector2>();
                }
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
                    _cameraDistance += context.action.ReadValue<float>() < 0 ? 3 : -3;
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

    public void CheckCameraCollision()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Target.position - transform.position, Color.cyan);
        //check for collision between camera and player
        if (Physics.Raycast(transform.position, (Target.position - transform.position), out hit, IgnoreLayer))
        {
            //collision detected (which is not the player itself)
            if (hit.collider.gameObject.layer != 7 && hit.collider.gameObject.GetComponent<Renderer>())
            {

                RaycastHit[] hitAll = Physics.RaycastAll(transform.position, (Target.position - transform.position), Vector3.Distance(transform.position, Target.position), IgnoreLayer);
                foreach (RaycastHit hitObj in hitAll)
                {
                    //filter out player mesh
                    if (hitObj.collider.gameObject.layer != 7)
                    {
                        //filter for submeshes
                        if (hitObj.collider.gameObject.tag.Equals("Submesh"))
                        {
                            //get list of all submeshes in parent (all children)
                            int amountOfChildren = hitObj.collider.transform.parent.childCount;
                            for (int i = 0; i < amountOfChildren; i++)
                            {
                                AdjustDitherMaterial(hitObj.collider.transform.parent.GetChild(i).gameObject, 0.5f, 0.8f);
                            }
                        }
                        else
                        {
                            AdjustDitherMaterial(hitObj.collider.gameObject, 0.5f, 0.8f);
                        }
                    }
                }
            }
        }
        foreach (GameObject obj in _affectedMat)
        {
            bool isInHit = false;
            RaycastHit[] hitAll = Physics.RaycastAll(transform.position, (Target.position - transform.position), Vector3.Distance(transform.position, Target.position), IgnoreLayer);
            foreach (RaycastHit hitObj in hitAll)
            {
                if (hitObj.collider.gameObject.layer != 7)
                {
                    isInHit = true;
                }
            }


            if (hit.collider != null)
            {
                if (!isInHit && hit.collider.gameObject.GetComponent<Renderer>())
                {

                    AdjustDitherMaterial(obj, 1f, 1f);
                }
            }
        }
    }


    private void AdjustDitherMaterial(GameObject gameObject, float opacity, float ditherSize)
    {
        if (gameObject.GetComponent<Renderer>())
        {
            MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
            //change the opacity of object which is colliding
            Renderer objRenderer = gameObject.GetComponent<Renderer>();
            _affectedMat.Add(gameObject);
            objRenderer.GetPropertyBlock(materialProperty);
            materialProperty.SetFloat("_Opacity", opacity);
            materialProperty.SetFloat("_Dither_Size", ditherSize);
            objRenderer.SetPropertyBlock(materialProperty);
        }
    }
}