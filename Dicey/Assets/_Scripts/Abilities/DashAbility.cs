using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour, IAbility
{
    public void Select()
    {
        Debug.Log("DashAbility Selected");
    }

    public void Deselect()
    {
        Debug.Log("DashAbility Deselected");
    }

    public void AcquireAnimation()
    {
        Debug.Log("DashAbility AcquireAnimation");
    }

    public void AbilityAction(InputAction.CallbackContext context)
    {
        Debug.Log("DashAbility AbilityAction");
    }
}