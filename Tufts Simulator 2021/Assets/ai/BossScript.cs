using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class BossScript : MonoBehaviour
{
    public Transform Player;
    public Dialogue dialogue;
    private bool playerhere;
    public Image m_speakto;
    private bool cantalk = true;
    public SphereCollider sphere;

    public Collider playerCollider;
    public Collider physicsCollider;
    public int xp_to_give = 10;
    public float ai_health = 20;
    public int ai_damage = 5;
    public int ai_attackSpeed = 1;
    public int MaxDist = 10;
    public float MinDist = 2;
    public Text defeated_text;
    public AudioClip[] hit_sounds;
    public AudioSource audioSource;

    private bool donetalking = false;
    private int MoveSpeed = 4;
    private bool attacking = false;
    private bool canattack = true;
    private int player_xp;
    private float player_damage;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
      playerhere = false;
      healthBar.SetMaxHealth(ai_health);
      Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
        if (cantalk && playerhere && Input.GetKeyDown(KeyCode.E))
        {
          m_speakto.enabled = false;
          Time.timeScale = 0;
          Cursor.visible = true;
          Cursor.lockState = CursorLockMode.None;
          TriggerDialogue();
          cantalk = false;
          sphere.enabled = false;
          donetalking = true;
        }
        //only behaves like an enemy if the player can't talk to the nemy anymore
        if (donetalking)
        {
            //if ai is close enough to player to notice player
            if (Vector3.Distance(transform.position, Player.position) >= MinDist)
            {
              //ai chases player
              transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            }
            //if ai is close enough to player to attack them
            else if (canattack && !attacking && Vector3.Distance(transform.position, Player.position) <= MinDist)
            {
              FindObjectOfType<FirstPersonController>().DamagePlayer(ai_damage);
              FindObjectOfType<FirstPersonController>().CameraShakeEffect();
              StartCoroutine(waittoattack());
            }
        }
    }

    public void TriggerDialogue()
    {
      FindObjectOfType<DialogueManager>().SetTalkingTo(1);
      FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (cantalk && other.gameObject.tag == "Player")
      {
        playerhere = true;
        m_speakto.enabled = true;
      }
      //this detects if the ai has collided with a weapon
      // (checks if the player punches this ai)
      if(donetalking && other.gameObject.tag == "Weapon")
      {
        player_damage = CalculateDamage();
        ReduceHealth(other);
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (cantalk && other.gameObject.tag == "Player")
      {
        playerhere = false;
        m_speakto.enabled = false;
      }
    }

    public void FreePlayer()
    {
      StartCoroutine(PlayerBFree());
    }

    IEnumerator PlayerBFree()
    {
      Time.timeScale = 0.1f;
      yield return new WaitForFixedUpdate();
      Time.timeScale = 1;
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

    public void doneTalking()
    {
        donetalking = true;
    }

    //This function calculates how much damage the player should do per hit
    // based on their level / XP
    float CalculateDamage()
    {
      player_xp = FindObjectOfType<FirstPersonController>().GetXP();
      return (5 + (player_xp / 100));

    }

    //this function actually removes health from ai, then kills them if they have
    // no health by deleting them from the game
    void ReduceHealth(Collider other)
    {
      if (hit_sounds.Length > 0)
      {
        audioSource.clip = hit_sounds[Random.Range(0, hit_sounds.Length)];
        audioSource.Play();
      }
      ai_health -= player_damage;
      // update health
      healthBar.SetHealth(ai_health);
      if (ai_health <= 0)
      {
           //if the ai dies, it gives xp to the player
           FindObjectOfType<FirstPersonController>().addXP(xp_to_give);
           //if the ai was a boss enemy, we display info
           defeated_text.text = $"You Defeated This Boss and Were Rewarded {xp_to_give} XP!";
           FindObjectOfType<FirstPersonController>().SetBossesDefeated();
           StartCoroutine(DisplayDefeatedText());
           //insert some function we'll call when a boss dies
           // (win screen? add to quest completion? ...)
       }
    }

    //These 2 functions basically just makes the script wait 1 second before proceeding
    IEnumerator waittoattack()
    {
      canattack = false;
      yield return new WaitForSeconds(ai_attackSpeed);
      canattack = true;
    }

    IEnumerator DisplayDefeatedText()
    {
      //displays winning text on player's screen
      defeated_text.enabled = true;
      //temporarily moves the enemy to reeeeeeeallly far away
      // (can't just delete the enemy yet because there's more script to run)
      transform.position = new Vector3(-999999, -9999999, -99999);
      //keeps winning text on screen for 3 seconds
      yield return new WaitForSeconds(3);
      //gets rid of winning text
      defeated_text.enabled = false;
      //destroys this object once there's no more code to run
      Object.Destroy(this.gameObject);
    }
}
