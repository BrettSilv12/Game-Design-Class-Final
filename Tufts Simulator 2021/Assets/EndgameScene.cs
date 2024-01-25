using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndgameScene : MonoBehaviour
{
    public void RestartButton() {
        SceneManager.LoadScene("Map 3.0");
    }
    public void ExitButton() {
        SceneManager.LoadScene("Menu_With_Buttons");
    }
 }
