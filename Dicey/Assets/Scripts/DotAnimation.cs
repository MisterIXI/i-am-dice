using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotAnimation : MonoBehaviour
{
    private const float BOB_SPEED = 2f;
    private const float BOB_DISTANCE = 0.3f;
    private const float ROTATION_SPEED = 1f;
    private float _rot = 0;
    private GameObject _dot;

    // Start is called before the first frame update
    void Start()
    {
        _dot = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _dot.transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * BOB_SPEED) * BOB_DISTANCE, 0);
        _rot = (_rot + ROTATION_SPEED) % 360;
        _dot.transform.localRotation = Quaternion.Euler(0, _rot, 90);
    }
}
