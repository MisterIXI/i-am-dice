using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilitySelector : MonoBehaviour
{
    private IAbility _currentAbility;
    private readonly IAbility[] _abilities = new IAbility[]{
        new ExplosionAbility()
        // new JumpAbility(),
        // new TeleportAbility()
    };
    private int _abilityIndex = 0;
    private PlayerInput _playerInput;

    void Start()
    {
        // Select first ability
        SelectAbility(_abilityIndex);
        _playerInput = GetComponent<PlayerInput>();
    }


    public void SelectAbility(int index)
    {
        if (_currentAbility != null)
        {
            _currentAbility.Deselect();
        _playerInput.currentActionMap.FindAction("AbilityAction", true).performed -= _currentAbility.AbilityAction;

        }
        _currentAbility = _abilities[index];
        _currentAbility.Select();
        _playerInput.currentActionMap.FindAction("AbilityAction", true).performed += _currentAbility.AbilityAction;
    }

    public void CycleAbility(InputAction.CallbackContext context)
    {
        if (context.action.name == "CycleAbility")
        {
            if (context.performed)
            {
                _abilityIndex++;
                if (_abilityIndex >= _abilities.Length)
                {
                    _abilityIndex = 0;
                }
                SelectAbility(_abilityIndex);
            }
        }
    }

}
