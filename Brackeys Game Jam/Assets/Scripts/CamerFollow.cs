using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    [HideInInspector]
    public Transform player;

    private new Transform transform;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.3f;
    private float dampingSpeed = 1.0f;

    private void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 basepos = new Vector3(player.position.x, player.position.y, transform.position.z);

        if (shakeDuration > 0)
        {
            transform.localPosition = basepos + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.position = basepos;
        }
    }

    public void TriggerShake(float mag, float dur)
    {
        shakeMagnitude = mag;
        shakeDuration = dur;
    }
}
