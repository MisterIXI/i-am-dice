using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelperFunctions : MonoBehaviour
{
    public static void SubsribeToEvent(InputActionMap actionMap, string ActionName, System.Action<InputAction.CallbackContext> callback)
    {
        InputAction action = actionMap.FindAction(ActionName, true);
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }

    public static void UnsubscribeFromEvent(InputActionMap actionMap, string ActionName, System.Action<InputAction.CallbackContext> callback)
    {
        try
        {
            InputAction action = actionMap.FindAction(ActionName, true);
            action.started -= callback;
            action.performed -= callback;
            action.canceled -= callback;
        }
        catch (System.Exception)
        {
            Debug.Log("Could not unsubscribe from event " + ActionName);
        }
    }
}
