using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject CanvasObject;
    public GameObject ControlsCanvas;
    public bool isMainMenu = false;

    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Escape))
        {
          PauseGame();
        }
    }

    public void PauseGame()
    {
      //pauses game / freezes time
      Time.timeScale = 0;
      //lets player use mouse to click buttons on menu
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
      //adds pause menu to screen
      CanvasObject.SetActive(true);
    }

    public void PauseResumeButton()
    {
      //removes pause menu from screen
      CanvasObject.SetActive(false);
      if(isMainMenu == false)
      {
        //resumes game (unfreezes time)
        FindObjectOfType<triggerdialogueobject>().EndDialogue();
      }
    }

    public void PauseQuitButton()
    {
        // Quit Game
        Application.Quit();
    }

    public void OpenControls()
    {
        ControlsCanvas.SetActive(true);
    }

    public void CloseControls()
    {
        ControlsCanvas.SetActive(false);
    }
}
