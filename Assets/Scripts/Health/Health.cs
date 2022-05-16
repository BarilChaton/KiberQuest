using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    public bool dead = false;
    public float deathTimer;
    private Vector3 respawnPoint;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashed;
    private SpriteRenderer spriteRend;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;


    private void Start()
    {
        respawnPoint = transform.position;
    }
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invulnerability());
            SoundManager.instance.PlaySound(hurtSound);  
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Die");
                dead = true;
                FindObjectOfType<Kiber>().Die();
                SoundManager.instance.PlaySound(deathSound);
                StartCoroutine(Respawn());
            }
        }
    }

    public IEnumerator Respawn()
    {
        if (dead)
        {
            yield return new WaitForSeconds(2);
            anim.SetTrigger("Respawn");
            transform.position = respawnPoint;
            currentHealth = startingHealth;
            dead = false;
            FindObjectOfType<Kiber>().Respawn();
            StopCoroutine(Respawn());
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashed; i++)
        {
            spriteRend.color = new Color(0, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashed * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashed * 2));    
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
