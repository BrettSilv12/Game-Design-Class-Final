using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void Update() {
        if (Input.GetKey("1"))
        {
            SceneManager.LoadScene("BrettScene");
        } 
        else if (Input.GetKey("2")) 
        {
            SceneManager.LoadScene("Map");
        }
        else if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
