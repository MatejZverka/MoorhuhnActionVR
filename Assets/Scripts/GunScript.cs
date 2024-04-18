using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    public GameObject projectile;   // odkaz na prefab projectilu
    public float power = 10.0f;     // sila/rýchlosť výstrelu
    public GameObject shootPoint;   // pozícia na ktorej vznikne projektil
    public GameObject grabPoint;    // pozícia na ktorej vznikne projektil

    // Update is called once per frame
    public void Shoot() {
        GameObject newProjectile = Instantiate(projectile, 
                                               shootPoint.transform.position, 
                                               shootPoint.transform.rotation) as GameObject;
        //nasmerujeme projektil podľa natočenia grabPointu s danou silou
        newProjectile.GetComponent<Rigidbody>().AddForce(grabPoint.transform.forward * power, ForceMode.VelocityChange);
    }

}