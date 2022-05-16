using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagingPC : MonoBehaviour
{
    [SerializeField] protected float damage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponent<Health>().TakeDamage(damage);

        var player = collision.GetComponent<Kiber>();
        player.knockbackCount = player.knockbackLength;

        if (collision.transform.position.x < transform.position.x)
            player.knockFromRight = true;
        else
            player.knockFromRight = false;
    }
}
