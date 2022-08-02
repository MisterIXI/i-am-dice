using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


public class ExplosionAbility : Ability
{

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
        Debug.Log("ExplosionAbility AbilityAction");
    }
}