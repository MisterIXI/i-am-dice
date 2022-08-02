using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AbilitySelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CycleAbility(InputAction.CallbackContext context)
    {
        if (context.action.name == "CycleAbility")
        {
            Debug.Log("CycleAbility");
        }
    }

}
