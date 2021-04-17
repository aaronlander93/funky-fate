using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private bool hasKey = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            hasKey = true;
            collision.gameObject.GetComponent<ItemPickup>().AnimateItemPickup(gameObject);

        }
    }
}
