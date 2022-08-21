using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    [field: SerializeField]
    public GameObject FollowTarget { get; private set; }
    [field: SerializeField]
    public GameObject Dice { get; private set; }
    [field: SerializeField]
    public GameObject PlayerCamera { get; private set; }

}
