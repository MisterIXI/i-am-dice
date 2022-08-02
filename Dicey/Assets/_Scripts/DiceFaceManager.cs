using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFaceManager : MonoBehaviour
{
    private Dictionary<int, int> _faceNumberForInstanceID = new Dictionary<
        int,    // InstanceID
        int     // faceNumber
        >();

    public void RegisterFace(int instanceID, int faceNumber)
    {
        _faceNumberForInstanceID.Add(instanceID, faceNumber);
    }

    public int GetFaceNumber(Collider faceCollider)
    {
        int instanceID = faceCollider.gameObject.GetInstanceID();
        return _faceNumberForInstanceID.GetValueOrDefault(instanceID);
    }
}
