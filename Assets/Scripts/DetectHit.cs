using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHit : MonoBehaviour {

    void Start()
    {
        if (gameObject.GetComponent<Collider>())
        { // If this game object has a collider, continue
            gameObject.GetComponent<Collider>().enabled = true; // Enable the collider
            Destroy(GetComponent<DetectHit>()); // Remove this script from the game object
        }
    }
}
