using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIDeath : MonoBehaviour
{
    [SerializeField] AudioClip deathSound;
    private bool shouldDie = false;
    private float deathTimer = 0;

    public float timeBeforeDestroy = 1.0f;
    public GameObject Kiber;
    
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckDead();
    }

    public void Crush()
    {
        anim.SetBool("Dying", true);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<EnemyPatrolAI>().enabled = false;
        shouldDie = true;
        SoundManager.instance.PlaySound(deathSound);
        Kiber.GetComponent<Kiber>().Bounce();

    }

    void CheckDead()
    {
        if (shouldDie == true)
        {
            

            if (deathTimer <= timeBeforeDestroy)
            {
                deathTimer += Time.deltaTime;
            }
            else
            {
                shouldDie = false;
                Destroy(this.gameObject);
            }
        }
    }

}
