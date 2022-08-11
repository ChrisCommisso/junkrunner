using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PeopleScript : MonoBehaviour
{
    public bool fleeing;
    public static GameObject Player;
    public NavMeshAgent myAgent;
    public static List<PeopleScript> people;
    public static void init() 
    {
        foreach (var item in people)
        {
            item.iniAsync();
        }
    }

    public Vector3 DestinationPoint;
    public static List<Tuple<AstarPoint,float>> AllPoints = new List<Tuple<AstarPoint, float>>();

   
    



    public Tuple<AstarPoint, float> NextTargetPoint() 
    {
        int address = UnityEngine.Random.Range(0, AllPoints.Count);
        return AllPoints[address];
    }
   
    // Start is called before the first frame update
    void iniAsync()
    {
        fleeing = false;
        DestinationPoint = NextTargetPoint().Item1.transform.position;
        myAgent = GetComponent<NavMeshAgent>();
        calculatePath();
    }
    void calculatePath() //taskify
    {
        myAgent.SetDestination(DestinationPoint);
    }
        
        
        
    
 void OnDrawGizmos()
    {
        
    }
    public virtual void startMethod()
    {
        if (people == null) 
        {
            people = new List<PeopleScript>();
        }
        if (Player == null) 
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        people.Add(this);//populate people after start
    }
    // Update is called once per frame
    void Update()
    {
        
        if (DestinationPoint != null) 
        {
            if ((transform.position - DestinationPoint).magnitude < 1f) 
            {
                DestinationPoint = NextTargetPoint().Item1.transform.position;
                calculatePath();
            }
        }
        if (Player != null) 
        {
            if ((Vector3.Dot(transform.forward, (PeopleScript.Player.transform.position - transform.position).normalized) > .3f || (PeopleScript.Player.transform.position - transform.position).magnitude<30) && MovementBehaviors.instance.isThirdPerson)
            {
                if (!samplecontrollerscript.scorePerSecond.Contains(this))
                {
                    samplecontrollerscript.scorePerSecond.Add(this);
                }
                fleeing = true;
                myAgent.speed *= 2;
                DestinationPoint = transform.position - (Player.transform.position - transform.position).normalized;
                calculatePath();
            }
            else if (fleeing) 
            {
                if (samplecontrollerscript.scorePerSecond.Contains(this))
                {
                    samplecontrollerscript.scorePerSecond.Remove(this);
                }
                myAgent.speed *= .5f;
                fleeing = false;
                DestinationPoint = NextTargetPoint().Item1.transform.position;
                calculatePath();
            }
        }
       
    }
}