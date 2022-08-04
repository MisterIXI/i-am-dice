using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Linq;

public class FreezeObjectsAbility : Ability
{
    public float FreezeTime = 10f;
    public float FreezeRadius = 10f;
    private bool _isFreezing = false;
    public override void Select()
    {
        Debug.Log("FreezeObjectsAbility Selected");
    }

    public override void Deselect()
    {
        Debug.Log("FreezeObjectsAbility Deselected");
    }

    public override void AcquireAnimation()
    {
        Debug.Log("FreezeObjectsAbility AcquireAnimation");
    }

    public override void AbilityAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isOnCoolDown)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, FreezeRadius);
                List<Rigidbody> rigidbodies = new List<Rigidbody>();
                foreach (Collider hit in hits)
                {
                    if (hit.tag != "Player" && hit.tag != "DiceFace")
                    {
                        Rigidbody rb = hit.GetComponent<Rigidbody>();
                        if (rb == null)
                            rb = hit.transform.parent.GetComponent<Rigidbody>();
                        if (rb != null)
                            rigidbodies.Add(rb);
                    }
                }
                StartCoroutine(FreezeObjects(rigidbodies.Distinct().ToArray()));
            }
        }
    }
    private void FrozenTint(bool isFrozen, Rigidbody[] frozenObjects)
    {
        //TODO: Implement tinting of colors
        // Color[][] frozenTint = new Color[frozenObjects.Length][];
        // for (int i = 0; i < frozenObjects.Length; i++)
        // {
        //     List<Color> tempColors = new List<Color>();
        //     frozenObjects[i].GetComponentsInChildren<Renderer>().ToList().ForEach(renderer =>
        //     {
        //         renderer.materials.ToList().ForEach(material =>
        //         {
        //             tempColors.Add(material.color);
        //         });
        //     });
        //     frozenTint[i] = new Color[2];

        //     frozenTint[i][0] = frozenObjects[i].GetComponent<Renderer>().material.color;
        //     frozenTint[i][1] = isFrozen ? Color.blue : frozenTint[i][0];
        // }
        // if (isFrozen)
        // {
        //     GetComponent<Renderer>().material.color = Color.blue;
        // }
        // else
        // {
        //     GetComponent<Renderer>().material.color = Color.white;
        // }
    }
    private IEnumerator FreezeObjects(Rigidbody[] frozenObjects)
    {
        _isOnCoolDown = true;
        RigidbodyConstraints[] oldConstraints = new RigidbodyConstraints[frozenObjects.Length];
        for (int i = 0; i < frozenObjects.Length; i++)
        {
            oldConstraints[i] = frozenObjects[i].constraints;
            frozenObjects[i].constraints = RigidbodyConstraints.FreezeAll;
        }
        FrozenTint(true, frozenObjects);
        yield return new WaitForSeconds(FreezeTime);
        for (int i = 0; i < frozenObjects.Length; i++)
        {
            if (oldConstraints[i] != RigidbodyConstraints.FreezeAll)
            {
                frozenObjects[i].constraints = oldConstraints[i];
            }
        }
        FrozenTint(false, frozenObjects);
        _isOnCoolDown = false;
    }
}
