using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyQuit : MonoBehaviour
{
        // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
          {
            SceneManager.LoadScene("Menu_With_Buttons");
          }
    }
}
