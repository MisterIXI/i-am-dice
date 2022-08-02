using UnityEngine.InputSystem;

public interface IAbility
{
    void Select();
    void Deselect();
    void AcquireAnimation();
    void AbilityAction(InputAction.CallbackContext context);
}

public abstract class Ability : IAbility
{
    public abstract void Select();
    public abstract void Deselect();
    public abstract void AcquireAnimation();
    public abstract void AbilityAction(InputAction.CallbackContext context);
}