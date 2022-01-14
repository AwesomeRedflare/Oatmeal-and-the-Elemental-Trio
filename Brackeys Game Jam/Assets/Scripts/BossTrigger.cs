using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject cam;
    public GameObject block;
    public PlayerController player;
    public BossManager bossManager;
    public Transform target;
    public float speed;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine("MoveCamera");
        }
    }

    IEnumerator MoveCamera()
    {
        bossManager.StartCoroutine("Intro");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        cam.GetComponent<CamerFollow>().enabled = false;
        player.enabled = false;
        block.SetActive(true);

        while (player.enabled == false)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(target.position.x, target.position.y, cam.transform.position.z), speed * Time.deltaTime);

            if (Vector2.Distance(cam.transform.position, target.position) < .1f)
            {
                yield return new WaitForSeconds(2);

                player.enabled = true;

            }

            yield return null;
        }
    }
}
