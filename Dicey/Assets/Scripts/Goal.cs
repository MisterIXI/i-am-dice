using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public DiceFaceManager diceFaceManager;

    private int _currentFaceNumber;

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
            _currentFaceNumber = diceFaceManager.GetFaceNumber(other);
            Debug.Log($"Currently touching face: {_currentFaceNumber}.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //_currentFaceNumber = 0;
    }
}
