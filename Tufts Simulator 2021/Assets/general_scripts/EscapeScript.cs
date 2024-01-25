using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScript : MonoBehaviour
{
  public Collider collider;

  void Update()
  {
    if(collider.enabled == true)
    {
      SceneManager.LoadScene("Map 3.0");
    }
  }
}
