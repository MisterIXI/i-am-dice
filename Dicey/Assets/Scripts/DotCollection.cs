using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCollection : MonoBehaviour
{
    private int _dotCount = 0;
    private Material[] _materials;
    public void Start()
    {
        _materials = GetComponent<MeshRenderer>().materials;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.name);
        if (other.gameObject.CompareTag("Dot"))
        {
            DotAnimation dotAnimation = other.gameObject.GetComponent<DotAnimation>();
            dotAnimation.PlayCollectedAnimation();
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            CollectDot();
        }
    }

    private void CollectDot()
    {
        _dotCount++;
        if (_dotCount < _materials.Length - 1)
        {
            _materials[_dotCount].color = new Color(255, 215, 0);
            _materials[_dotCount].SetFloat("_Metallic", 1);
        }
        if (_dotCount == _materials.Length)
        {
            Debug.Log("You win!");
            _materials[0].color = new Color(181, 155, 13);
            _materials[0].SetFloat("_Metallic", 1);
            GetComponent<DiceController>().InfiniteJumpsEnabled = true;
        }
    }
}
