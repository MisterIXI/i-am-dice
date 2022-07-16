using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFace : MonoBehaviour
{
    public int FaceNumber;

    // Start is called before the first frame update
    void Start()
    {
        var diceFaceManager = GetComponentInParent<DiceFaceManager>();
        if (diceFaceManager == null)
        {
            Debug.Log("There is no DiceFaceManager script on the dice object.");
            enabled = false;
            return;
        }

        diceFaceManager.RegisterFace(gameObject.GetInstanceID(), FaceNumber);
        enabled = false;
    }
}
