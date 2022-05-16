using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_upAndDown2 : MonoBehaviour
{
    public float movementDistance;
    public float speed;
    public float damage;
    private bool movingDown;
    private float downEdge;
    private float upEdge;

    private void Awake()
    {
        downEdge = transform.position.y + movementDistance;
        upEdge = transform.position.y - movementDistance;
    }

    private void Update()
    {
        if (movingDown)
        {
            if (transform.position.y < downEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else
                movingDown = false;
        }
        else
        {
            if (transform.position.y > upEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
            else
                movingDown = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
