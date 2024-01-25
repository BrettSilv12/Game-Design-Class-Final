using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


 public class ai_mover : MonoBehaviour
 {
     public Transform Player;
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
     public int MoveSpeed = 4;
     public int respawnTimer = 60;

     private bool attacking = false;
     private bool canattack = true;
     private int player_xp;
     private float player_damage;
     private Vector3 initialPosition;
     private float initialHealth;

     public HealthBar healthBar;

     void Start()
     {
      // set health bar max\
      healthBar.SetMaxHealth(ai_health);


      Player = GameObject.FindWithTag("Player").GetComponent<Transform>();

      //We store the initial position ad health so that when we respawn the enemy,
      //    it reverts to its true original state
      initialPosition = transform.position;
      initialHealth = ai_health;
     }

     void Update()
     {
         //makes ai look at player from start
         transform.LookAt(Player);

         //if ai is close enough to player to notice player
         if (Vector3.Distance(transform.position, Player.position) <= MaxDist && Vector3.Distance(transform.position, Player.position) >= MinDist)
         {
           //ai chases player
           transform.position += transform.forward * MoveSpeed * Time.deltaTime;
         }
         //if ai is too far away from player to notice them
         else if (Vector3.Distance(transform.position, Player.position) >= MaxDist)
         {
           //do what ai does when its too far away
           //add script for idle ai activity here (stand still is ok, or add idle walking)
         }
         //if ai is close enough to player to attack them
         else if (canattack && !attacking && Vector3.Distance(transform.position, Player.position) <= MinDist)
         {
           FindObjectOfType<FirstPersonController>().DamagePlayer(ai_damage);
           FindObjectOfType<FirstPersonController>().CameraShakeEffect();
           StartCoroutine(waittoattack());
         }
     }

     //this detects if the ai has collided with a weapon
     // (checks if the player punches this ai)
     private void OnTriggerEnter(Collider other)
     {
       if(other.gameObject.tag == "Weapon")
       {
         player_damage = CalculateDamage();
         ReduceHealth(other);
       }
     }

     //This function calculates how much damage the player should do per hit
     // based on their level / XP
     float CalculateDamage()
     {
       player_xp = FindObjectOfType<FirstPersonController>().GetXP();
       return (5 + (player_xp / 20));

     }

     //this function actually removes health from ai, then kills them if they have
     // no health by deleting them from the game
     void ReduceHealth(Collider other)
     {
       audioSource.clip = hit_sounds[Random.Range(0, hit_sounds.Length)];
       audioSource.Play();

       ai_health -= player_damage;
       // update health
       healthBar.SetHealth(ai_health);
       if (ai_health <= 0)
       {
             //if the ai dies, it gives xp to the player
            FindObjectOfType<FirstPersonController>().addXP(xp_to_give);
            StartCoroutine(EnemyRespawn());
       }
     }

     //These 2 functions basically just makes the script wait 1 second before proceeding
     IEnumerator waittoattack()
     {
       canattack = false;
       yield return new WaitForSeconds(ai_attackSpeed);
       canattack = true;
     }

     IEnumerator EnemyRespawn()
     {
         transform.position = new Vector3(-999999, -9999999, -99999);
         ai_health = initialHealth;
         yield return new WaitForSeconds(respawnTimer);
         transform.position = initialPosition;
         healthBar.SetMaxHealth(ai_health);
     }
}
