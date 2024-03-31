using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupBox"))
        {
            //GameObject.Find("PlayerGroup").GetComponent<PlayerMovement>().AddBox();
            Destroy(gameObject);
        }
    }

    public void DestoyThis()
    {
        Destroy(gameObject);
    }
}
