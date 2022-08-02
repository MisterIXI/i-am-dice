using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


public class ExplosionAbility : Ability
{
    public float ExplosionRadius = 25;
    public float ExplosionForce = 3000;
    private GameObject _explosionParticles;
    private void Start()
    {
        _explosionParticles = Resources.Load<GameObject>("ExplosionParticles");
    }
    public override void Select()
    {
        Debug.Log("ExplosionAbility Selected");
    }

    public override void Deselect()
    {
        Debug.Log("ExplosionAbility Deselected");
    }

    public override void AcquireAnimation()
    {
        Debug.Log("ExplosionAbility AcquireAnimation");
    }

    public override void AbilityAction(InputAction.CallbackContext context)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].tag != "Player" && hits[i].tag != "DiceFace")
            {
                if (hits[i].TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                }
                else if (hits[i].transform.parent.TryGetComponent(out Rigidbody rb2))
                {
                    rb2.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                }
            }
        }
        GameObject go = Instantiate(_explosionParticles, transform.position, Quaternion.identity);
        go.transform.localScale = new Vector3(ExplosionRadius * 0.1f, ExplosionRadius * 0.1f, ExplosionRadius * 0.1f);
        Destroy(go, 2);
    }

    private void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}