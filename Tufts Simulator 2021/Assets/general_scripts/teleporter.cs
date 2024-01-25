using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleporter : MonoBehaviour
{
  public string destination;
    private void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.tag == "Player")
      {
        SceneManager.LoadScene(destination);
        Object.Destroy(this.gameObject);
      }
    }
}
