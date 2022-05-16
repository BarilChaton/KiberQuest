using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxxer : MonoBehaviour
{
    [SerializeField] AudioClip landSound;
    AudioSource landSource;
    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        landSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        Falling();
    }

    void Falling()
    {
        if (rb.velocity.x > 0.01f || rb.velocity.x < -0.01f)
            anim.SetBool("Falling", true);
        else
            anim.SetBool("Falling", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            landSource.PlayOneShot(landSound, 1f);
        }
    }
}
