using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableItem
    {
        Dice
    }

    public CollectableItem CollectableType;

    public float OffsetY;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + OffsetY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerCollectedCollectables player = other.transform.parent.gameObject.GetComponent<PlayerCollectedCollectables>();
            switch (CollectableType)
            {
                case CollectableItem.Dice:
                    player.AddCollectableDice();
                    break;
            }



            Destroy(gameObject);
        }
    }
}
