using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//kulka atsitrenkus lieka masinoj             kitos atsimusineja

public class Bullet : MonoBehaviour
{
    public LayerMask playerLayer;
    public float MaxDamage = 50f;
    public float force = 5f;
    public float lifeTime = 50f;
    public float radius = 1f;

    public string plIgnore = "Player1";
    private Light lightColor;
    public void Start()
    {
        lightColor = GetComponentInChildren<Light>(); ;
        //  Destroy(gameObject, lifeTime);
        //Shooting plIgnore = ignore.GetComponent<Shooting>();
        // Debug.Log(plIgnore.playerToIgnnore.name);
        //ignore = playerSkip.GetComponent<>();      
    }
    /// <summary>
    /// ///gauna tik tada kai  as bunu arti privazeves wtfwtfwtf
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
            
      //kad ignorintu save 
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, playerLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponentInParent<Rigidbody>();
            if (!targetRigidbody)
                continue;
            targetRigidbody.AddExplosionForce(force,transform.position,radius);            
            PlayerHealth target = targetRigidbody.GetComponent<PlayerHealth>();
            if (!target)
                continue;
            
            //gal padaryt kad tik zaideja,,    ooveliau jei sienos grina ir sienom dmg

            //tikrint targeta ir jo narda tada givibiu skripte ziuret kad sau dmg nedaryt
            if (targetRigidbody.name != plIgnore)
            {
                float damage = CalculateDamage(targetRigidbody.position);
                target.TakeDamage(damage);
                Debug.Log("biske");
                Destroy(gameObject.GetComponent<Bullet>());
                lightColor.color = Color.yellow;
            }
        }

        

    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        // Create a vector from the shell to the target.
        Vector3 explosionToTarget = targetPosition - transform.position;

        // Calculate the distance from the shell to the target.
        float explosionDistance = explosionToTarget.magnitude;

        // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
        float relativeDistance = (radius - explosionDistance) / radius;

        // Calculate damage as this proportion of the maximum possible damage.
        float damage = relativeDistance * MaxDamage;

        // Make sure that the minimum damage is always 0.
        damage = Mathf.Max(0f, damage);

        return damage;
    }
}
