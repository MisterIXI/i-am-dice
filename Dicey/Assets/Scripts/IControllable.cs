using UnityEngine.InputSystem;

public interface IControllable
{
    void ReceiveInput(InputAction.CallbackContext context);
}