using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public DiceFaceManager diceFaceManager;

    // If the dice is currently on the goal, this returns the number shown on top of the dice.
    // Otherwise, 0 is returned.
    public int GetCurrentlyRolledNumber()
    {
        return _currentlyTouchingFaceNumber == 0 ? 0 : 7 - _currentlyTouchingFaceNumber;
    }

    private int _currentlyTouchingFaceNumber;

    // Start is called before the first frame update
    void Start()
    {
        if(diceFaceManager == null)
        {
            Debug.Log("Please assign the diceFaceManager to the Goal object in the editor.");
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DiceFace"))
        {
            _currentlyTouchingFaceNumber = diceFaceManager.GetFaceNumber(other);
            Debug.Log($"Currently touching face: {_currentlyTouchingFaceNumber}.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //_currentFaceNumber = 0;
    }
}
