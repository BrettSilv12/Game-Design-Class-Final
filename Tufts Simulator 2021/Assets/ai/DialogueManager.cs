using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameTag;
    public Text dialogueText;
    public Image dialogueImage;
    public GameObject CanvasObject;

    public int talkingto = 0;
    private Queue<string> sentences;
    private bool indialogue;

    // Start is called before the first frame update
    void Start()
    {
      sentences = new Queue<string>();
      indialogue = false;
    }

    void Update()
    {
      if (indialogue && Input.GetMouseButtonDown(0))
      {
        DisplayNextSentence();
      }
    }

    public void StartDialogue (Dialogue dialogue)
    {
      nameTag.text = dialogue.name;
      dialogueImage.sprite = dialogue.image;
      //dialogueImage = dialogue.image;
      sentences.Clear();
      indialogue = true;
      EnableCanvas();

      foreach (string sentence in dialogue.sentences)
      {
        sentences.Enqueue(sentence);
      }

      DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
      if (sentences.Count == 0)
      {
        EndDialogue();
        return;
      }

      string sentence = sentences.Dequeue();
      dialogueText.text = sentence;
    }

    void EndDialogue()
    {
      indialogue = false;
      DisableCanvas();
      if(talkingto == 0)
      {
        FindObjectOfType<npc_interaction>().FreePlayer();
      }else if (talkingto == 1)
      {
        FindObjectOfType<triggerdialogueobject>().EndDialogue();
        talkingto = 0;
      }
    }

    public void SetTalkingTo(int givennumber)
    {
      talkingto = givennumber;
    }

    void DisableCanvas()
    {
      CanvasObject.SetActive(false);
    }

    void EnableCanvas()
    {
      CanvasObject.SetActive(true);
    }
}
