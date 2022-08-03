using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class DashAbility : Ability
{
    private Rigidbody _rb;
    private Transform _followTargetTransform;
    private GameObject _dashParticles;
    public float DashStrength = 1500;

    private void Start()
    {
        _dashParticles = Resources.Load<GameObject>("DashDust");
        _rb = GetComponent<Rigidbody>();
        _followTargetTransform = transform.parent.GetComponentInChildren<FollowTarget>().gameObject.transform;
    }
    public override void Select()
    {
        Debug.Log("DashAbility Selected");
    }

    public override void Deselect()
    {
        Debug.Log("DashAbility Deselected");
    }

    public override void AcquireAnimation()
    {
        Debug.Log("DashAbility AcquireAnimation");
    }

    public override void AbilityAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _rb.velocity = Vector3.zero;
            _rb.AddForce(_followTargetTransform.forward * DashStrength);
            _rb.AddTorque(new Vector3(Random.Range(1f, 2f) * DashStrength, Random.Range(1f, 2f) * DashStrength, Random.Range(1f, 2f) * DashStrength));
            GameObject go = Instantiate(_dashParticles, transform.position, Quaternion.identity);
            go.transform.rotation = _followTargetTransform.rotation;
            Destroy(go, 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _followTargetTransform.forward * 10);
        }
    }
}