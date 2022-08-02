using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


public class ExplosionAbility : MonoBehaviour, IAbility
{
    public void Select()
    {
        Debug.Log("ExplosionAbility Selected");
    }

    public void Deselect()
    {
        Debug.Log("ExplosionAbility Deselected");
    }

    public void AcquireAnimation()
    {
        Debug.Log("ExplosionAbility AcquireAnimation");
    }

    public void AbilityAction(InputAction.CallbackContext context)
    {
        Debug.Log("ExplosionAbility AbilityAction");
        
    }
}