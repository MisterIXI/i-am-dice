using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCam : MonoBehaviour
{

    public LayerMask IgnoreLayer;
    public LayerMask IgnoreCollision;
    public Transform Target;
    public float Speed = 5f;
    public float ZoomSpeed = 1;
    public float CameraDistance = 10f;
    private float _cameraZoom = 0;
    private Vector3 _direction;
    private Vector2 _directionChange;
    private DiceController _diceController;
    private HashSet<GameObject> _affectedMat;
    bool _isCameraColliding;
    float _lastZoomedDistance;
    bool _hasZoomedIn;
    // Start is called before the first frame update
    void Start()
    {
        _diceController = Target.gameObject.GetComponent<DiceController>();
        _direction = new Vector3(1, 0, 1).normalized * CameraDistance;
        _affectedMat = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_directionChange.x != 0 || _directionChange.y != 0)
        {
            _diceController.UpdateMovement(_diceController.CurrentInput);
        }
        CameraDistance = Mathf.Clamp(CameraDistance + _cameraZoom, 5f, 20f);

        float oldX = transform.rotation.y;


        if (_isCameraColliding || IsCameraColliding())
        {
            ZoomToPlayer();
        }
        else
        {
            transform.position = Target.position + _direction;
        }

        ZoomOut();





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

    bool _isZooming;
    bool _canZoomBack;
    //automatic zoom
    void ZoomToPlayer()
    {
        if (CameraDistance > 5f)
        {
            transform.position = Vector3.Lerp(transform.position, Target.position, ZoomSpeed * Time.deltaTime);
            _hasZoomedIn = true;
        }
        CameraDistance = Vector3.Distance(transform.position, Target.position);
        
    }

    void ZoomOut()
    {
        //if (_lastZoomedDistance < 10)
        //{
        //    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - _lastZoomedDistance), ZoomSpeed * Time.deltaTime);
        //    CameraDistance = Vector3.Distance(transform.localPosition, Target.position);
        //}
        //else
        //{
        //    _canZoomBack = false;
        //}
        //if (!_isCameraColliding && _hasZoomedIn)
        //{

        //    if(CameraDistance < 20)
        //    {
        //        transform.position = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - _lastZoomedDistance), ZoomSpeed * Time.deltaTime);
        //        //CameraDistance = Vector3.Distance(transform.localPosition, Target.position);
        //        Debug.Log("Zoom out");
        //    }
        //    else
        //    {
        //        _hasZoomedIn = false;
        //    }
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
            _isCameraColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
            _isCameraColliding = false;
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
                    _lastZoomedDistance = CameraDistance;
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



    private bool IsCameraColliding()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Target.position - transform.position, Color.red);
        //check for collision between camera and player
        return (Physics.Raycast(transform.position, (Target.position - transform.position), out hit, IgnoreCollision) && hit.collider.gameObject.layer == 8);
    }

    public void CheckCameraCollision()
    {
        RaycastHit hit;
        RaycastHit[] hitAll = Physics.RaycastAll(transform.position, (Target.position - transform.position), Vector3.Distance(transform.position,Target.position),IgnoreLayer);
        Debug.DrawRay(transform.position, Target.position-transform.position,Color.cyan);
        //check for collision between camera and player
        if (Physics.Raycast(transform.position, (Target.position - transform.position), out hit, IgnoreLayer))
        {
            //collision detected (which is not the player itself)
            if (hit.collider.gameObject.layer != 7 && hit.collider.gameObject.GetComponent<Renderer>())
            {

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
            //get all raycasts
            //RaycastHit[] hitAll = Physics.RaycastAll(transform.position, (Target.position - transform.position), Vector3.Distance(transform.position, Target.position), IgnoreLayer);
            foreach (RaycastHit hitObj in hitAll)
            {
                if (hitObj.collider.gameObject.layer != 7)
                {
                    isInHit = true;
                }
            }
            if (hit.collider.gameObject)
            {
                if (!isInHit && hit.collider.gameObject.GetComponent<Renderer>())
                {
                    //readjusting dither back to normal
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
