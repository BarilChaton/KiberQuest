using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead2 : Enemy_damage
{
    [Header("Spikehead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private float checkTimer;
    private Vector3 destination;
    private Vector3 startPosition;
    private Animator anim;

    private bool attacking;

    private Vector3[] directions = new Vector3[2];
    

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;

    private void Awake()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        anim = GetComponent<Animator>();
    }

    //private void OnEnable()
    //{
    //    Stop();
    //}

    private void Update()
    {
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);

        else
        {
            checkTimer += Time.deltaTime;

            if (checkTimer > checkDelay)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
                CheckForPlayer();
            }
        }
    }
    private void CheckForPlayer()
    {
        CalculateDirections();

        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        anim.SetBool("Bottom", false);
        directions[0] = transform.up * range;
        directions[1] = -transform.up * range;
    }

    private void Stop()
    {
        destination = transform.position;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        anim.SetBool("Bottom", true);
        base.OnTriggerEnter2D(collision);
        Stop();
    }

}
