using UnityEngine;
using UnityEngine.Events;

public class DiceRollDetector : MonoBehaviour
{
    UnityEvent _diceRolledEvent = new UnityEvent();

    private bool _rollAnnounced = false;

    private int _currentlyTouchingFaceNumber;

    public void AddDiceRollListener(UnityAction listener)
    {
        _diceRolledEvent.AddListener(listener);
    }

    public void RemoveDiceRollListeners()
    {
        _diceRolledEvent.RemoveAllListeners();
    }

    // If the dice is currently on the goal, this returns the number shown on top of the dice.
    // Otherwise, 0 is returned.
    public int GetCurrentlyRolledNumber()
    {
        return _currentlyTouchingFaceNumber == 0 ? 0 : 7 - _currentlyTouchingFaceNumber;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DiceFace"))
        {
            var diceFaceManager = other.GetComponentInParent<DiceFaceManager>();
            _currentlyTouchingFaceNumber = diceFaceManager.GetFaceNumber(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_rollAnnounced && 
            other.gameObject.CompareTag("DiceFace") && 
            other.attachedRigidbody.velocity.magnitude < 0.1f)
        {
            _diceRolledEvent.Invoke();
            _rollAnnounced = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _currentlyTouchingFaceNumber = 0;
        _rollAnnounced = false;
    }
}
