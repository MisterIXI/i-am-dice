using UnityEngine;

public class DiceTowerRangeCheck : MonoBehaviour
{
    public DiceTowerPassCheck DiceTowerPassCheck;

    private bool _playerIsInRange = false;

    public bool IsPlayerInRange() => _playerIsInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerIsInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerIsInRange = false;
            DiceTowerPassCheck?.ResetPlayerHasPassed();
        }
    }
}
