using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotSpot : MonoBehaviour
{
    public float DeclineSpeed;
    public float DeclinePower;
    Light _dotLight;

    private void OnEnable()
    {
        _dotLight = GetComponent<Light>();
        _dotLight.enabled = true;
        StartCoroutine(DeclineLight());
    }

    IEnumerator DeclineLight()
    {
        while(_dotLight.intensity > 0)
        {
            yield return new WaitForSeconds(DeclineSpeed * Time.deltaTime);
            _dotLight.intensity -= DeclinePower;
            if (_dotLight.intensity <= 0)
                _dotLight.intensity = 0;
        }
    }
}
