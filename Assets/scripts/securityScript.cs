using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class securityScript : MonoBehaviour
{
    static bool detected;
    // Start is called before the first frame update
    public bool fleeing;
    public NavMeshAgent myAgent;

    public Vector3 DestinationPoint { get; private set; }

    public static List<PeopleScript> people;
    int pathindex;
    Transform pathObjects;
    public string pathID;
    private void Start()
    {
        startMethod();
    }
    public void startMethod()
    {
        detected = false;
        pathindex = 0;
        pathObjects = GameObject.FindGameObjectWithTag(pathID).transform;
        myAgent = GetComponent<NavMeshAgent>();
        DestinationPoint = pathObjects.GetChild(pathindex).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (detected)
        {
            myAgent.speed = 20;
        }
        else 
        {
            myAgent.speed = 5;
        }
        if (DestinationPoint != null)
        {
            myAgent.SetDestination(DestinationPoint);
            if ((transform.position - DestinationPoint).magnitude < 1f)
            {
                pathindex++;
                pathindex %= pathObjects.childCount;
                DestinationPoint = pathObjects.GetChild(pathindex).transform.position;
                myAgent.SetDestination(DestinationPoint);    
            }

        }
        if (PeopleScript.Player != null)
        {
            if (MovementBehaviors.instance.isThirdPerson)
            {
                RaycastHit sight;
                Physics.Raycast(transform.position, (PeopleScript.Player.transform.position - transform.position).normalized,out sight, 15f);
                if (Vector3.Dot(transform.forward, (PeopleScript.Player.transform.position - transform.position).normalized) > .3f||detected&&sight.collider?.gameObject.GetComponent<samplecontrollerscript>()!=null)
                {
                    
                    if (!samplecontrollerscript.multiplier.Contains(this)) 
                    {
                        print(gameObject.name + " saw the player");
                        samplecontrollerscript.multiplier.Add(this);
                    }
                    detected = true;
                    if ((PeopleScript.Player.transform.position - transform.position).magnitude < .7f)
                    {
                        //kill the player
                    }

                    DestinationPoint = PeopleScript.Player.transform.position;
                    myAgent.SetDestination(DestinationPoint);
                    //add points
                }
                else
                {
                    if (samplecontrollerscript.multiplier.Contains(this))
                    {
                        print(gameObject.name + " lost the player");
                        samplecontrollerscript.multiplier.Remove(this);
                    }
                    detected = false;
                }

            }
            else 
            {
                if (samplecontrollerscript.multiplier.Contains(this))
                {
                    samplecontrollerscript.multiplier.Remove(this);
                }
                detected = false;
            }
           
        }

    }
}
