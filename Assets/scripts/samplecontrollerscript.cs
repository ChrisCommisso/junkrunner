using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class samplecontrollerscript : MonoBehaviour
{

    Vector3 vel;
    bool prevButton8;
    int physicsLayer;
    float slowDownTime;
    public float slowamount;
    public static samplecontrollerscript instance;
    public static bool rotating;
    public static List<securityScript> multiplier;
    public static List<PeopleScript> scorePerSecond;
    public static int score;
    IEnumerator rotationshift(RaycastHit hitInfo,float timeToTake) 
    {
        rotating = true;
        float timePassed = 0;
        Vector3 start = transform.forward;
        Vector3 finish = Vector3.Cross(Vector3.Cross(transform.forward, hitInfo.normal), transform.forward);
        while (timeToTake >= timePassed) 
        {
            Vector3 newfwd = Vector3.Lerp(start, finish, timePassed / timeToTake);
            transform.forward = new Vector3(newfwd.x, 0, newfwd.z);
            yield return new WaitForEndOfFrame();
            timePassed += Time.deltaTime;
        }
        rotating = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        multiplier = new List<securityScript>();
        scorePerSecond = new List<PeopleScript>();
        rotating = false;
        slowDownTime = 0f;
        physicsLayer = LayerMask.NameToLayer("physicsLayer");
        if (instance == null)
        {
            instance = this;
        }
    }
    controllerState getControllerstate() 
    {
        controllerState c = new controllerState();
        if (Input.GetKey(KeyCode.Space)) 
        {
            c.BUTTON8 = true;
        }
        Vector2 left = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            left = new Vector2(left.x - 1, left.y);
        }
        if (Input.GetKey(KeyCode.S))
        {
            left = new Vector2(left.x , left.y - 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            left = new Vector2(left.x + 1, left.y);
        }
        if (Input.GetKey(KeyCode.W))
        {
            left = new Vector2(left.x, left.y + 1);

        }
        Vector3 right = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            right = new Vector2(right.x - 1, right.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            right = new Vector2(right.x + 1, right.y);
        }
        c.rightStick = right;
        c.leftStick = left;
        return c;
    }
    private void Update()
    {
        Vector3 pos = transform.position;
        controllerState state = getControllerstate();
        
        
        MovementBehaviors.rotateAndPositionCam(state, MovementBehaviors.instance._camera);
        GetComponent<CharacterController>().Move(new Vector3(0,-MovementBehaviors.instance.gravity*Time.deltaTime,0));
        if (MovementBehaviors.instance.pausemovefor >= 0) { 
        MovementBehaviors.instance.pausemovefor-=Time.deltaTime;
        }
        else if (MovementBehaviors.instance.isThirdPerson)
        {
            RaycastHit hitInfo = new RaycastHit();
            UnityEngine.Vector3 movement = MovementBehaviors.cruiseVelocity(state) * Time.deltaTime;
            movement = transform.forward * movement.z + transform.up * movement.y + transform.right * movement.x;
            Ray ray = new Ray(MovementBehaviors.instance._camera.transform.parent.position, MovementBehaviors.instance._camera.transform.parent.forward);
            if (slowDownTime >= 0f)
            {
                movement *= slowamount;
            }
            if (Physics.Raycast(ray, out hitInfo, 1f) &&!rotating)
            {
                if(hitInfo.transform.gameObject.layer != physicsLayer)
                StartCoroutine(rotationshift(hitInfo, .1f));
            }    
           //    Vector3 collisionpoint = hitInfo.point;
           //    Vector3 part1 = (hitInfo.point - transform.position).normalized* ((hitInfo.point - transform.position).magnitude);
           //    movement = part1+Vector3.Project(movement,Vector3.Cross(hitInfo.normal, Vector3.up));
           //}
            
            GetComponent<CharacterController>().Move(movement);
        }
        else
        {
            RaycastHit hitInfo = new RaycastHit();
            UnityEngine.Vector3 movement = MovementBehaviors.walkVelocity(state) * Time.deltaTime;
            movement = transform.forward * movement.z + transform.up * movement.y + transform.right * movement.x;

            if (rotating)
            {
                movement = movement * .05f;
            }
            GetComponent<CharacterController>().Move(movement);

        }
        vel = (pos - transform.position)/Time.deltaTime;
        
        slowDownTime -= Time.deltaTime;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        controllerState state = getControllerstate();
        if (state.BUTTON8 != prevButton8 && state.BUTTON8 == false)
        {
            MovementBehaviors.instance.isThirdPerson = !MovementBehaviors.instance.isThirdPerson;
        }
        prevButton8 = state.BUTTON8;
        RaycastHit hit;
        for (int i = 0; i < 360; i += 36)
        {
            Debug.DrawLine(GetComponent<CharacterController>().transform.position, GetComponent<CharacterController>().transform.position+new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), Color.red,5f);
            //Check if anything with the platform layer touches this object
            Ray ray = new Ray(GetComponent<CharacterController>().transform.position+Vector3.up*.8f, new Vector3(Mathf.Cos(i), -.75f, Mathf.Sin(i)));
            if (Physics.Raycast(ray,out hit,1,(1<<physicsLayer)))//(Physics.CapsuleCast(GetComponent<CharacterController>().transform.position - new Vector3(0, GetComponent<CharacterController>().height, 0), GetComponent<CharacterController>().transform.position + new Vector3(0,GetComponent<CharacterController>().height,0) , 10, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit,3, 1 << physicsLayer))
            {
                //If the object is touched by a platform, move the object away from it

                if (hit.rigidbody != null)
                {
                    slowDownTime = .2f;
                    slowamount = 1 / hit.rigidbody.mass;
                    if (MovementBehaviors.instance.isThirdPerson)
                    {
                        Vector3 forceToAdd = transform.forward + transform.right * Random.Range(4, 10);
                        if ((int)Time.realtimeSinceStartup % 2 == 0)
                        {
                            forceToAdd *= -1f;
                        }
                        forceToAdd += Vector3.up * vel.magnitude;
                        hit.rigidbody.velocity = forceToAdd;
                    }

                    else
                    {
                        hit.rigidbody.AddForceAtPosition((hit.transform.position - transform.position).normalized * Time.fixedDeltaTime, hit.point);
                    }
                    if (MovementBehaviors.instance.isThirdPerson)
                        hit.rigidbody.AddTorque(Random.Range(-1f, 2f) * Time.fixedDeltaTime * hit.rigidbody.mass * .5f * 3 * vel, ForceMode.Impulse);
                }
                float cruisevel = .5f * (MovementBehaviors.instance.maxCruiseVel - MovementBehaviors.instance.minCruiseVel)+ MovementBehaviors.instance.minCruiseVel;
                if (hit.transform.gameObject.GetComponent<Staircase>() && MovementBehaviors.instance.pausemovefor <= 0)
                {
                    MovementBehaviors.instance.pausemovefor = hit.transform.gameObject.GetComponent<Staircase>().pausemovefor;
                    StartCoroutine(hit.transform.gameObject.GetComponent<Staircase>().belay(cruisevel, this.gameObject));
                }


            }
        }


    }
    
}
