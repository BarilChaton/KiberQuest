using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeetCollide : MonoBehaviour
{
    public GameObject enemyDamage;

    private void Awake()
    {
        enemyDamage = GameObject.FindGameObjectWithTag("EnemyDamageCollision");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyAIDeath>().Crush();
            collision.gameObject.SetActive(false);
            enemyDamage.SetActive(false);
        }
    }
}
