using UnityEngine;

public class DiceTowerPassCheck : MonoBehaviour
{
    private bool _playerHasPassed = false;

    public bool HasPlayerPassed() => _playerHasPassed;

    public void ResetPlayerHasPassed()
    {
        _playerHasPassed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerHasPassed = true;
        }
    }
}
