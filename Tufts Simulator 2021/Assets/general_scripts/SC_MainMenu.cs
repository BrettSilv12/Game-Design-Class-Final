using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;


public class SC_MainMenu : MonoBehaviour
{

    public GameObject MainMenu;
    [SerializeField] private Image[] tutorial;


    // Start is called before the first frame update
    void Start()
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }

    // public void Tisch()
    // {
    //     Cursor.visible = false;
    //     // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
    //     SceneManager.LoadScene("Tisch");
    // }

    public void MapButton()
    {
        // Cursor.visible = false;
        // Debug.Log("in");
        // StartCoroutine(playTutorial());
            Cursor.visible = false;
            // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
            SceneManager.LoadScene("Tutorial");
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

    // IEnumerator playTutorial() {
    //    int len = tutorial.Length;
    //    Debug.Log("in func");
    //
    //    for (int i = 0; i < len; i++) {
    //         Debug.Log("in loop");
    //         yield return new WaitForSeconds(0.1f);
    //         tutorial[i].enabled = true;
    //         Debug.Log(i);
    //         while(!Input.GetKeyDown(KeyCode.Space)) {
    //             yield return null;
    //         }
    //         tutorial[i].enabled = false;
    //    }
    //     SceneManager.LoadScene("Map");
    // }


 }
