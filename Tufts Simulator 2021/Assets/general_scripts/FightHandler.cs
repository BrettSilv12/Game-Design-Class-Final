using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FightHandler : MonoBehaviour
{
  public int bossnumber;
  public Collider escape;
  public SphereCollider cutscene;
  private bool temptrigger = true;
    // Start is called before the first frame update
    void Start()
    {
      escape.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<FirstPersonController>().GetBossesDefeated() == bossnumber && temptrigger)
        {
          StartCoroutine(leaveScene());
          temptrigger = false;
        }
    }

    IEnumerator leaveScene()
    {
      cutscene.enabled = true;
      yield return new WaitForSeconds(2);
      escape.enabled = true;
    }
}
