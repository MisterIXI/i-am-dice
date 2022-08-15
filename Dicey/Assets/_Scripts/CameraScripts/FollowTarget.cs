using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Target.localPosition;
    }
}
