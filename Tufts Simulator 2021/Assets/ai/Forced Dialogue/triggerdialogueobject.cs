using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerdialogueobject : MonoBehaviour
{
    public Dialogue dialogue;
    public SphereCollider sphere;

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.tag == "Player")
      {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        TriggerDialogue();
      }
    }

    public void TriggerDialogue()
    {
      FindObjectOfType<DialogueManager>().SetTalkingTo(1);
      FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
      sphere.enabled = false;
    }

    public void EndDialogue()
    {
      StartCoroutine(FreePlayerScene());
    }

    IEnumerator FreePlayerScene()
    {
      Time.timeScale = 0.1f;
      yield return new WaitForFixedUpdate();
      Time.timeScale = 1;
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

}
