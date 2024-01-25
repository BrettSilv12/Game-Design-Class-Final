using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    void Start()
    {
      //pauses game / freezes time
      Time.timeScale = 0;
      //lets player use mouse to click buttons on menu
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToGame()
    {
      //pauses game / freezes time
      Time.timeScale = 1;
      //lets player use mouse to click buttons on menu
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      SceneManager.LoadScene("Map 3.0");
    }

    public void QuitGame()
    {
      Application.Quit();
    }
}
