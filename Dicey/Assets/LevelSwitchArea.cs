using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitchArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("Enable level switch");
            transform.parent.GetComponent<WorldHubDiceTower>().EnableLevelSwitch();
        }
    }
}
