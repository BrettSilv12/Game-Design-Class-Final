using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class deletecutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      Debug.Log(FindObjectOfType<FirstPersonController>().GetFirstLoad());
        if(FindObjectOfType<FirstPersonController>().GetFirstLoad() == true)
        {
          FindObjectOfType<FirstPersonController>().SetFirstLoad();
        }
        else
        {
          Object.Destroy(this.gameObject);
        }
      Debug.Log(FindObjectOfType<FirstPersonController>().GetFirstLoad());
    }
}
