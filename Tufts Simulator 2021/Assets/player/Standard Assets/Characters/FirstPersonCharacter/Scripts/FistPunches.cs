using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistPunches : MonoBehaviour
{

    public Transform Player;
    public SphereCollider collider;
    public float movementSpeed = 5.0f;
    public bool canattack;

    // Start is called before the first frame update
    void Start()
    {
      canattack = true;
      //disables ability for fist to do damage UNTIL a punch occurs
      collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canattack)
        {
          transform.forward = Player.forward;
          StartCoroutine(punch());
          StartCoroutine(waittoattack());

        }
    }

    //coroutine that gets called when player punches
    //This function basically creates a fist weapon that shoots out forwards
    //  and damages any enemy it touches. Once the enemy has taken tamage,
    //  it pulls the fist weapon back so that it can't do damage until the
    //  player punches again
    IEnumerator punch()
    {
      //enables collider so enemy can take damage from it
      collider.enabled = true;
      //moves the collider forwards to where the player is looking
      transform.position += transform.forward * 4;
      //expands collider to make sure it's big enough to hit enemy infront of you
      collider.radius += 5;
      //waits until game can register there's a collision
      yield return new WaitForFixedUpdate();
      //yield return new WaitForSeconds(1);
      //everything below here just removes changes from first half of function,
      //  now that damage has been taken, the fist can be "removed" until used again
      collider.radius -= 5;
      transform.position -= transform.forward * 4;
      collider.enabled = false;
    }


    //This function basically just makes the script wait 1 second before proceeding
    IEnumerator waittoattack()
    {
      canattack = false;
      yield return new WaitForSeconds(1);
      canattack = true;
    }

}
