using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    float z;
    float x;
    bool ret;
    // Start is called before the first frame update
    void Start()
    {
     float x = 0;
     float z = 0;
     bool ret = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(x);
        //Debug.Log(z);
        if (x < 13 && !ret) {
            x += 0.05f;
        } else if (x >= 13 && !ret && z < 14) {
            z += 0.05f;
        } else {
            ret = true;
        }

        if (ret && z > 0 && x > 0) {
            z = z - 0.05f;
        } else if (ret && z <= 0 && x > 0) {
            x = x - 0.05f;
        } else {
            ret = false;
        }


        transform.position = new Vector3(x, 0, z);

    }
}
