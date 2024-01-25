using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastForward : MonoBehaviour {
    void Start () {

    }

//prints that how far FPS controller is from something -- not really sure whats going on 
    void Update () {
        RaycastHit hit; 
        float dist; 
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        if (Physics.Raycast(transform.position,(forward), out hit)){
            dist = hit.distance; 
            Debug.Log(dist + " " + hit.collider.gameObject.name + hit.collider.gameObject.tag);
        }
    }
}