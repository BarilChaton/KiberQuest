using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PigCharge : MonoBehaviour
{
    [SerializeField] Transform player;
    public AudioClip chargeSound;
    AudioSource chargeSource;
    Rigidbody2D rb;

    [SerializeField]float agroRange;
    [SerializeField]float moveSpeed;

    public bool activateScript = false;
    bool attackSoundPlaying = false;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        chargeSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (activateScript == true)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            //print("distToPlayer" + distToPlayer);

            if (distToPlayer < agroRange)
            {
                anim.SetBool("Charging", true);
                ChargePlayer();
                if (!attackSoundPlaying)
                {
                    chargeSource.PlayOneShot(chargeSound, 1.0f);
                    attackSoundPlaying = true;
                }
            }
            else
            {
                anim.SetBool("Charging", false);
                StopCharge();
                if (attackSoundPlaying == true)
                   attackSoundPlaying=false;
            }
        }
    } 

    private void ChargePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            GetComponent<EnemyPatrolAI>().enabled = false;
            rb.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (transform.position.x > player.position.x)
        {
            GetComponent<EnemyPatrolAI>().enabled = false;
            rb.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void StopCharge()
    {
        GetComponent<EnemyPatrolAI>().enabled = true;
        rb.velocity = new Vector2(0, 0);
    }
}
