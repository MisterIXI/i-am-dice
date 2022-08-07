using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    public Material[] Materials;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.transform.childCount != 0)
        {
            Material chosenMat = Materials[Random.Range(0, Materials.Length)];
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = chosenMat;
            }
        }
        else
            gameObject.GetComponent<MeshRenderer>().material = Materials[Random.Range(0,Materials.Length)];
    }
}
