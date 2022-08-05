using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilitySelector : MonoBehaviour
{
    private IAbility _currentAbility;
    private IAbility[] _abilities;
    private int _abilityIndex = 0;
    private PlayerInput _playerInput;
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        InitializeAbilities();
        // Select first ability
        SelectAbility(_abilityIndex);
    }

    private void InitializeAbilities()
    {
        _playerInput.currentActionMap.FindAction("CycleAbilities").performed += CycleAbility;
        _abilities = new IAbility[]{
            gameObject.AddComponent<ExplosionAbility>(),
            gameObject.AddComponent<DashAbility>(),
            gameObject.AddComponent<GroundPoundAbility>(),
            gameObject.AddComponent<FreezeObjectsAbility>()
        };
        for (int i = 0; i < _abilities.Length; i++)
        {
            _abilities[i].MonoBehaviour().enabled = false;
        }
    }
    public void SelectAbility(int index)
    {
        if (_currentAbility != null)
        {
            _currentAbility.Deselect();
            _currentAbility.MonoBehaviour().enabled = false;

        }
        _currentAbility = _abilities[index];
        _currentAbility.Select();
        _currentAbility.MonoBehaviour().enabled = true;
    }

    public void TriggerCurrentAbility(InputAction.CallbackContext context)
    {
        if(_currentAbility != null)
        {
            _currentAbility.AbilityAction(context);
        }
    }

    public void CycleAbility(InputAction.CallbackContext context)
    {
        if (context.action.name == "CycleAbilities")
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
