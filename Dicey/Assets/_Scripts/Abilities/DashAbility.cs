using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class DashAbility : Ability
{
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
        Debug.Log("DashAbility AbilityAction");
    }
}