using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class paperbagAddXP : MonoBehaviour
{
  public Collider box;
  public int XP_to_give = 25;

    private void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.tag == "Player")
      {
        FindObjectOfType<FirstPersonController>().addXP(XP_to_give);
        Object.Destroy(this.gameObject);
      }
    }

}
