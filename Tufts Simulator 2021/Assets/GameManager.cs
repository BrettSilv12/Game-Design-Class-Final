using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Vector3 originalPos;
    bool gameHasEnded = false;
    
    void Start()
    {
        originalPos = GameObject.Find("FPSController").transform.position;
    }
    
    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            SceneManager.LoadScene("DEATHSCENE");
        }
    }
    // Exists for bugs, i.e. falling off the map
    public void ResetPlayer()
    {
        Debug.Log("OUT OF BOUNDS: RESET PLAYER");
        // RESET PLAYER POSITION TO OG POSITION
        GameObject.Find("FPSController").transform.position = originalPos;
    }
}
