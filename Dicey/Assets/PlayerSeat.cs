using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeat : MonoBehaviour
{
    public CarBehavior car;
    public WheelCollider[] wheels;

    private void Start()
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.enabled = false;
        }
        car.enabled = false;
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            car.enabled = true;
            foreach(WheelCollider wheel in wheels)
            {
                wheel.enabled = true;
            }
            Debug.Log("Player has entered the car!");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            car.enabled = false;
            foreach (WheelCollider wheel in wheels)
            {
                wheel.enabled = false;
            }
            Debug.Log("Player has left the car!");
        }
    }
}
