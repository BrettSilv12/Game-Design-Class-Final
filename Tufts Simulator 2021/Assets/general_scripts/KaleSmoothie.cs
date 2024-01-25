using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class KaleSmoothie : MonoBehaviour
{
    public GameObject player;
    private FirstPersonController fps_script;

  void OnTriggerEnter(Collider other) 
  {
    if (other.gameObject.CompareTag("Player")) 
    {
        if (FindObjectOfType<FirstPersonController>().getHealth() != 100)
        {
            //This function call adds 5 health to player
            FindObjectOfType<FirstPersonController>().IncreaseHealth(20);
            //Destroys kale smoothie object so that the player "drinks" it
            Object.Destroy(this.gameObject);            
        }
     }
   }
}
