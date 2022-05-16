using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShooting : MonoBehaviour
{
    Animator anim;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] bullets;

    [SerializeField] private float agroRange;
    [SerializeField] Transform player;
    [SerializeField] Transform castPoint;

    //bool playerVisible = false;
    private float cooldownTimer;

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSound;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Attack()
    {
        anim.Play("Shooting");
    }

    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void Update()
    {
        if (CanSeePlayer(agroRange))
        {
            Attack();
        }
        else
        {
            anim.Play("Idle");
        }
    }

    private void Shoot()
    {
        SoundManager.instance.PlaySound(arrowSound);
        bullets[FindBullet()].transform.position = firePoint.position;
        bullets[FindBullet()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    bool CanSeePlayer(float distance)
    {

        bool var = false;
        float castDist = distance;

        Vector2 endPos = castPoint.position + Vector3.left * distance;
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Player"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                var = true;
            }
            else
            {
                var = false;
            }
            
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.blue);
        }

        return var;
    }
}
