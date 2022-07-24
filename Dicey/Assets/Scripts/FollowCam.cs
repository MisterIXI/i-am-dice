using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCam : MonoBehaviour
{
    public LayerMask IgnoreLayer;
    public Transform Target;
    public float Speed = 5f;
    public float CameraDistance = 10f;
    private float _cameraZoom = 0;
    private Vector3 _direction;
    private Vector2 _directionChange;
    private DiceController _diceController;
    private HashSet<GameObject> _affectedMat;
    // Start is called before the first frame update
    void Start()
    {
        _diceController = Target.gameObject.GetComponent<DiceController>();
        _direction = new Vector3(1, 0, 1).normalized * CameraDistance;
        _affectedMat = new HashSet<GameObject>();
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
        CheckCameraCollision();
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

    public void CheckCameraCollision()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Target.position-transform.position,Color.cyan);
        //check for collision between camera and player
        if (Physics.Raycast(transform.position, (Target.position - transform.position), out hit, IgnoreLayer))
        {
            //collision detected (which is not the player itself)
            if (hit.collider.gameObject.layer != 7 && hit.collider.gameObject.GetComponent<Renderer>())
            {

                RaycastHit[] hitAll = Physics.RaycastAll(transform.position, (Target.position - transform.position), Vector3.Distance(transform.position,Target.position),IgnoreLayer);
                foreach(RaycastHit hitObj in hitAll)
                {
                    //filter out player mesh
                    if(hitObj.collider.gameObject.layer != 7)
                    {
                        //filter for submeshes
                        if (hitObj.collider.gameObject.tag.Equals("Submesh"))
                        {
                            //get list of all submeshes in parent (all children)
                            int amountOfChildren = hitObj.collider.transform.parent.childCount;
                            for(int i = 0; i < amountOfChildren; i++)
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
        foreach(GameObject obj in _affectedMat)
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

            if (!isInHit && hit.collider.gameObject.GetComponent<Renderer>())
            {

                AdjustDitherMaterial(obj, 1f, 1f);
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
