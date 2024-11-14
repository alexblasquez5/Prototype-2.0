using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
   gameController gameController;
   public Transform respawnPoint;

   private void Awake() // Changed 'awake' to 'Awake'
   {
     gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<gameController>();
   }

   private void OnTriggerEnter2D(Collider2D collision) // Changed 'onTriggerEnter2D' to 'OnTriggerEnter2D'
   {
        if(collision.CompareTag("Player"))
        {
            gameController.UpdateCheckPoint(respawnPoint.position);
        }
   }
}
