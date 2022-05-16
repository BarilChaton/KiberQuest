using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Fields")]
    //Detection point
    public Transform detectionPoint;
    //Detection radious
    private const float detectionRadious = 0.2f;
    //Detection layer
    public LayerMask detectionLayer;
    //Cached Trigger Object
    public GameObject detectedObject;
    [Header("Examine Fields")]
    //Examine window object
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamining;
    [Header("Others")]
    //List of picked items
    public List<GameObject> pickedItems = new List<GameObject>();

    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    bool InteractInput()
    {
        return Input.GetButtonDown("Use");
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadious, detectionLayer);
        
        if (obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    public void PickUpItem(GameObject item)
    {
        pickedItems.Add(item);
    }

    public void ExamineItem(Item item)
    {
        if (isExamining)
        {
            //Hide the examine window
            examineWindow.SetActive(false);
            //Disable the boolean
            isExamining = false;
        }
        else
        {
            //Show the items image in the middle
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            //Write description text underneath the image
            examineText.text = item.descriptionText;
            //Display the examine window
            examineWindow.SetActive(true);
            //Enable the boolean
            isExamining = true;
        }
    }
}
