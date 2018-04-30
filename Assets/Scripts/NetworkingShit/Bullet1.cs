using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//kulka atsitrenkus lieka masinoj             kitos atsimusineja

public class Bullet1 : MonoBehaviour
{
    public int Damage = 25;
    private Light lightColor;

    private void Start()
    {
        lightColor = GetComponentInChildren<Light>(); //
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<PlayerHealth1>();//genialu, tikrina ar turi skripta jei jo daga////tikrint ar nera esams
        var name = hit.name + "B";
		///paimti pirma suikta colider ir ji ignorint,  tagas koks
		if(health != null && name != gameObject.name)
        {
            health.TakeDamage(Damage);            
            lightColor.enabled = false;   
            Destroy(gameObject.GetComponent<Bullet1>());
        }

    }
}
