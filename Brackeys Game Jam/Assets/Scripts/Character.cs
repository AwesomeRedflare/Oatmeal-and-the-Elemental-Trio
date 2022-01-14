using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject projectile;
    public GameObject deathEffect;
    public Transform firePoint;

    public void Shoot()
    {
        Instantiate(projectile, firePoint.position, transform.rotation);
    }

    public void Death()
    {
        Instantiate(deathEffect, transform.position, transform.rotation);
    }
}
