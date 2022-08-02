using UnityEngine.InputSystem;

public interface IAbility
{
    void Select();
    void Deselect();
    void AcquireAnimation();
    void AbilityAction(InputAction.CallbackContext context);
}