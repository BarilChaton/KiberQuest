using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    //Interaction Type
    public enum InteractionType { NONE, PickUp, Examine }
    public InteractionType type;
    [Header("Examine")]
    public string descriptionText;
    public Sprite image;
    public UnityEvent customEvent;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 11;
    }

    public void Interact()
    {
        switch (type)
        {
            case InteractionType.PickUp:
                //Add the object to the "picked up items" list
                FindObjectOfType<InteractionSystem>().PickUpItem(gameObject);
                //Disable the object
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                //Call the examine item in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this);

                Debug.Log("EXAMINE");
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        //Invoke (call) the custom event
        customEvent.Invoke();
    }
}
