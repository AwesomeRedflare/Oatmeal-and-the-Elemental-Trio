using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Enemy enemy;

    private GameObject key;
    public GameObject target;

    private void Start()
    {
        key = transform.GetChild(1).gameObject;
        enemy = GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enemy.health <= 0)
        {
            FindObjectOfType<GameManager>().target = target.transform;
            key.gameObject.transform.rotation = Quaternion.identity;
            key.gameObject.transform.parent = null;
            key.GetComponent<BoxCollider2D>().enabled = true;
            key.SetActive(true);
        }
    }
}
