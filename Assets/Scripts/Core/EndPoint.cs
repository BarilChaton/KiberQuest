using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public Animator anim;
    private bool hasCollided;
    public string Scene;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (hasCollided == false)
            {
                anim.SetTrigger("Collided");
                hasCollided = true;
                LevelManager.instance.LoadScene(Scene);
            }
            else
            {
                anim.SetTrigger("Idle");
            }
        }
    }
}
