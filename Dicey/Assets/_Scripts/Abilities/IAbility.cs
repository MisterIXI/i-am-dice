using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;
using System.Collections;

public interface IAbility
{
    MonoBehaviour MonoBehaviour();
    void Select();
    void Deselect();
    void AcquireAnimation();
    void AbilityAction(InputAction.CallbackContext context);
}

public abstract class Ability : MonoBehaviour, IAbility
{
    public MonoBehaviour MonoBehaviour()
    {
        return this;
    }
    protected bool _isOnCoolDown = false;
    private IEnumerator AbilityCoolDown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        _isOnCoolDown = false;
    }

    public bool IsUnlocked = true;
    public abstract void Select();
    public abstract void Deselect();
    public abstract void AcquireAnimation();
    public abstract void AbilityAction(InputAction.CallbackContext context);
}