using UnityEngine.InputSystem;
using UnityEngine;

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
    public MonoBehaviour MonoBehaviour(){
        return this;
    }
    public abstract void Select();
    public abstract void Deselect();
    public abstract void AcquireAnimation();
    public abstract void AbilityAction(InputAction.CallbackContext context);
}