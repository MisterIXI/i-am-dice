using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTriggerZone : MonoBehaviour
{
    CarController _carController;
    // Start is called before the first frame update
    void Start()
    {
        _carController = transform.parent.GetComponent<CarController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            _carController.PlayerCollisionEnter(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            _carController.PlayerCollisionExit(other);
        }
    }
}
