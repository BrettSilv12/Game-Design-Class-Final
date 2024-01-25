using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class disableboss1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<FirstPersonController>().GetBossesDefeated() >= 1)
        {
          Object.Destroy(this.gameObject);
        }
    }
}
