using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyPatrolAI : MonoBehaviour
{
    //Refernce to waypoints.
    public List<Transform> points;
    //The int value for the next point index.
    public int nextID = 0;
    //The value of that applies to ID for changing.
    int idChangeValue = 1;
    //Speed of movement.
    public float speed = 1;

    bool isWaiting = false;
    //bool isDying = false;

    Animator anim;

    public void Awake()
    {
        //GetComponent<Rigidbody2D>().enabled = true;
        anim = GetComponent<Animator>();
    }

    private void Reset()
    {
        Init();
    }
    private void Init()
    {
        //Make box collider tirgger.
        GetComponent<BoxCollider2D>().isTrigger = true;

        //Create root object
        GameObject root = new GameObject(name + "_Root");

        //Reset position of Root to this Enemy Object.
        root.transform.position = transform.position;

        //Set enemy object as child of root.
        transform.SetParent(root.transform);

        //Create waypoint object
        GameObject waypoints = new GameObject("Waypoints");

        //Reset waypoints position to root

        //Make waypoints child of root
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;

        //Create two points (gameobject) and reset their position to waypoints object

        //Make the points children of waypoint object.
        GameObject p1 = new GameObject("Point1"); p1.transform.SetParent(waypoints.transform);p1.transform.position = Vector3.zero;
        GameObject p2 = new GameObject("Point2"); p2.transform.SetParent(waypoints.transform);p2.transform.position = Vector3.zero;

        //Init points list then add the points to it.
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);

    }

    private void Update()
    {
        if (isWaiting == false)
            MoveToNextPoint();
        else
            StartCoroutine(WaitABit());
        
    }

    void MoveToNextPoint()
    {
        //Get the next Point Transform.
        Transform goalPoint = points[nextID];
        //Flip the enemy scale to look towards the waypoint.
        if (goalPoint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
            
        else
            transform.localScale = new Vector3(1, 1, 1);
        //Move the enemy towards the goal point.
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
            anim.SetBool("Walking", true);
        //Check the distance between the enemy and goal point to trigger next point.
        if (Vector2.Distance(transform.position, goalPoint.position)<1f)
        {
            //Check if we are at the end of the line (make the change to -1)
            if (nextID == points.Count - 1)
            {
                idChangeValue = -1;
                isWaiting = true;
                anim.SetBool("Walking", false);
            }
                
            //Check if we are at the start of the line (make the change to +1)
            if (nextID == 0)
            {
                idChangeValue = 1;
                isWaiting = true;
                anim.SetBool("Walking", false);
            }
            //Apply the change on the nextID
            nextID += idChangeValue;
        }
    }
    public IEnumerator WaitABit()
    {
        yield return new WaitForSeconds (2);
        isWaiting = false;
        StopCoroutine(WaitABit());
    }
}
