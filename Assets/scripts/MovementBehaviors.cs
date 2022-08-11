using Assets.scripts;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine;

public class MovementBehaviors : MonoBehaviour
{
    public float pausemovefor;
    public static MovementBehaviors instance;
    public GameObject _camera;
    public float gravity;
    public float charWidth;
    public float thirdPersonMaxDist;
    public float sideMoveMult;
    public float minCruiseVel;
    public float maxCruiseVel;
    public float Sensitivity;
    public bool isThirdPerson;
    string[] hitboxIgnore = { "Player", "Scenery" };
    public static void resolveCollisions(BoxCollider hitbox) 
    {
        hitbox.transform.Translate( performCollisionsUp(hitbox));
        hitbox.transform.Translate( performCollisionsDown(hitbox));
        hitbox.transform.Translate( performCollisionsLeft(hitbox));
        hitbox.transform.Translate( performCollisionsRight(hitbox));
        hitbox.transform.Translate( performCollisionsForward(hitbox));
        hitbox.transform.Translate( performCollisionsBackwards(hitbox));
        

    }
    public static Vector3 performCollisionsUp(BoxCollider hitbox) 
    {
        Vector3 displacement = Vector3.zero;
        //push down
        Vector3 anchor = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Vector3 end = new Vector3(hitbox.bounds.center.x, hitbox.bounds.max.y, hitbox.bounds.center.z);
        Ray ray = new Ray(anchor, (end-anchor).normalized);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, hitbox.bounds.max.y - hitbox.bounds.center.y))    
        {
            
                displacement = new Vector3(0, -((hitbox.bounds.max.y - hitbox.bounds.center.y) - hitInfo.distance), 0);
        }
        return displacement;
    }
    public static Vector3 performCollisionsDown(BoxCollider hitbox)
    {
        Vector3 displacement = Vector3.zero;
        //push down
        Vector3 anchor = new Vector3(hitbox.bounds.center.x, hitbox.bounds.max.y, hitbox.bounds.center.z);
        Vector3 end = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Ray ray = new Ray(anchor, (end - anchor).normalized);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, hitbox.bounds.max.y - hitbox.bounds.center.y))
        {
            
                displacement = new Vector3(0, ((hitbox.bounds.max.y - hitbox.bounds.center.y) - hitInfo.distance), 0);
        }
        return displacement;
    }
    public static Vector3 performCollisionsRight(BoxCollider hitbox)
    {
        
        Vector3 displacement = Vector3.zero;
        //push down
        Vector3 anchor = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Vector3 end = new Vector3(hitbox.bounds.max.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Debug.DrawLine(anchor, end, Color.red);
        Ray ray = new Ray(anchor, (end - anchor).normalized);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, hitbox.bounds.max.x - hitbox.bounds.center.x))
        {
            
                displacement = new Vector3(-((hitbox.bounds.max.x - hitbox.bounds.center.x) - hitInfo.distance), 0, 0);
        }
        return displacement;
    }
    public static Vector3 performCollisionsLeft(BoxCollider hitbox)
    {
        Vector3 displacement = Vector3.zero;
        //push down
        Vector3 anchor = new Vector3(hitbox.bounds.max.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Vector3 end = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Ray ray = new Ray(anchor, (end - anchor).normalized);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, hitbox.bounds.max.x - hitbox.bounds.center.x))
        {
            
                displacement = new Vector3(((hitbox.bounds.max.x - hitbox.bounds.center.x) - hitInfo.distance), 0, 0);
        }
        return displacement;
    }
    public static Vector3 performCollisionsForward(BoxCollider hitbox)
    {
        Vector3 displacement = Vector3.zero;
        
        Vector3 anchor = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Vector3 end = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.max.z);
        Debug.DrawLine(anchor, end, Color.blue);
        Ray ray = new Ray(anchor, (end - anchor).normalized);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, hitbox.bounds.max.z - hitbox.bounds.center.z))
        {
            
                displacement = new Vector3(0, 0, -((hitbox.bounds.max.z - hitbox.bounds.center.z) - hitInfo.distance));
        }
        return displacement;
    }
    public static Vector3 performCollisionsBackwards(BoxCollider hitbox)
    {

        Vector3 displacement = Vector3.zero;
        //push down
        Vector3 anchor = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.max.z);
        Vector3 end = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, hitbox.bounds.center.z);
        Debug.DrawLine(anchor, end, Color.green);
        Ray ray = new Ray(anchor, (end - anchor).normalized);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, hitbox.bounds.max.z - hitbox.bounds.center.z))
        {
            
                displacement = new Vector3(0, 0, ((hitbox.bounds.max.z - hitbox.bounds.center.z) - hitInfo.distance));
        }
        return displacement;
    }
    public static Vector3 cruiseVelocity(controllerState s) 
    {
        float sideMovement = 0;
        
        return new Vector3(sideMovement*instance.sideMoveMult,0,(((s.leftStick.y+1f)/2f) * (instance.maxCruiseVel - instance.minCruiseVel)+instance.minCruiseVel));

    }
    public static Vector3 walkVelocity(controllerState s) 
    {
        float sideMovement = s.leftStick.x;
        print(s.leftStick.x + " "+ s.leftStick.y);
        return new Vector3(sideMovement * instance.minCruiseVel, 0, ((s.leftStick.y * instance.minCruiseVel)));
    }
    public static void rotateAndPositionCam(controllerState s, GameObject g) 
    {
        g.transform.parent.parent.Rotate(new Vector3(0, 1, 0), s.rightStick.x * instance.Sensitivity * Time.deltaTime);
        if (instance.isThirdPerson)
        {
            Ray ray = new Ray(g.transform.parent.position, new Vector3(0, Mathf.Clamp(1+s.rightStick.y,0,1), Mathf.Clamp(1 + (-1*s.rightStick.y), 0, 1)).normalized);
            RaycastHit hitInfo = new RaycastHit();
            
                g.transform.localPosition = new Vector3(0, Mathf.Sqrt(instance.thirdPersonMaxDist)/2f, -Mathf.Sqrt(instance.thirdPersonMaxDist));
            
        }
        else {
            g.transform.position = g.transform.parent.position+Vector3.up* 1.5f;
        }
        g.transform.LookAt(g.transform.parent.position+g.transform.parent.forward*.2f + Vector3.up * 1.5f);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
