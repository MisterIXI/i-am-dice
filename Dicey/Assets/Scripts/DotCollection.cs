using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCollection : MonoBehaviour
{
    private int _dotCount = 0;
    private Material[] _materials;
    private List<Material> _materialsList;
    public void Start()
    {
        _materials = GetComponent<MeshRenderer>().materials;
        _materialsList = new List<Material>(_materials);
        _materialsList.Sort((x, y) => x.name.CompareTo(y.name));
        for (int i = 1; i < _materialsList.Count; i++)
        {
            // Debug.Log("Material " + i + ": " + _materials[i].name);
            _materials[i].color = Color.black;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collided with " + other.name);
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
        Debug.Log("Name: " + _materialsList[0].name);
        if (_dotCount < _materialsList.Count - 1)
        {
            _dotCount++;
            Debug.Log("Dot collected: " + _dotCount);
            if (_dotCount < _materialsList.Count)
            {
                _materialsList[_dotCount].color = new Color(1f, 0.8431372549019608f, 0f);
                // _materials[_dotCount].SetFloat("_Metallic", 1);
            }
            if (_dotCount == _materialsList.Count - 1)
            {
                Debug.Log("You win!");

                _materialsList[0].color = new Color(0.709803921f, 0.6078431372f, 0.05098039f);
                _materialsList[0].SetFloat("_Metallic", 1f);
                GetComponent<DiceController>().InfiniteJumpsEnabled = true;
            }
        }
    }
}
