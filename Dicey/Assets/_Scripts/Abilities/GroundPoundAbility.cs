using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


public class GroundPoundAbility : Ability
{
    DiceController _diceController;
    Rigidbody _rb;
    Collider _collider;
    public bool AddExplosionEffect = true;
    public float ExplosionRadius = 5f;
    public float ExplosionForce = 1000f;
    private GameObject _groundPoundParticle;
    private bool _isGroundPounding = false;
    private float _oldHeight;
    private bool _controlsWereLocked = false;
    private void Start()
    {
        _diceController = GetComponent<DiceController>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _groundPoundParticle = Resources.Load<GameObject>("GroundPoundImpactParticle");
    }
    public override void Select()
    {
        Debug.Log("GroundPoundAbility Selected");
    }

    public override void Deselect()
    {
        Debug.Log("GroundPoundAbility Deselected");
    }

    public override void AcquireAnimation()
    {
        Debug.Log("GroundPoundAbility AcquireAnimation");
    }

    public override void AbilityAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isOnCoolDown)
                StartCoroutine(GroundPound());
        }
    }

    private IEnumerator GroundPound()
    {
        if (_diceController.IsMovementLocked)
            _controlsWereLocked = true;
        else
            _diceController.IsMovementLocked = true;
        _isOnCoolDown = true;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(Vector3.up * 50, ForceMode.Impulse);
        _rb.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        _rb.velocity = Vector3.zero;
        _oldHeight = transform.position.y;
        _rb.AddForce(Vector3.down * 100, ForceMode.Impulse);
        _rb.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
        _isGroundPounding = true;
    }

    private void WaitForGroundPound()
    {
        _isGroundPounding = true;
    }

    private void CollisionAnimation(float scaleFactor)
    {
        GameObject particle = Instantiate(_groundPoundParticle, transform.position, Quaternion.identity);
        // scale particle according to heigh difference
        particle.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        Destroy(particle, 1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isGroundPounding)
        {
            if (_diceController.IsOnFloor())
            {
                float heightDiffFactor = _oldHeight - transform.position.y;
                heightDiffFactor = heightDiffFactor * 0.01f + 0.75f;
                _isGroundPounding = false;
                if (_controlsWereLocked)
                    _controlsWereLocked = false;
                else
                    _diceController.IsMovementLocked = false;
                _isOnCoolDown = false;
                if (AddExplosionEffect)
                    ExplosionEffect(heightDiffFactor);
                CollisionAnimation(heightDiffFactor);
            }
        }
    }

    private void ExplosionEffect(float scaleFactor)
    {
        // Debug.Log("Impact: " + impactVelocity);
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius * scaleFactor);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].tag != "Player" && hits[i].tag != "DiceFace")
            {
                if (hits[i].TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(ExplosionForce * scaleFactor, transform.position, ExplosionRadius * scaleFactor);
                }
                else if (hits[i].transform.parent != null && hits[i].transform.parent.TryGetComponent(out Rigidbody rb2))
                {
                    rb2.AddExplosionForce(ExplosionForce * scaleFactor, transform.position, ExplosionRadius * scaleFactor);
                }
            }
        }
    }

}