using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObstacleTrigger : MonoBehaviour
{
    bool isActivated = false;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            LevelGenerator.CreateNewObstacle();
        }
    }
}
