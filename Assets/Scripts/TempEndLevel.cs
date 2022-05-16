using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempEndLevel : MonoBehaviour
{
    public Animator anim;
    private bool hasCollided;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (hasCollided == false)
            {
                anim.SetTrigger("Collided");
                hasCollided = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
            }
            else
            {
                anim.SetTrigger("Idle");
            }
        }
    }
}
