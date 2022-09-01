using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class samplecontrollerscript : MonoBehaviour
{
    List<Rigidbody> shuddering;
    Vector3 vel;
    bool prevButton8;
    int physicsLayer;
    int doorLayer;
    float slowDownTime;
    public float slowamount;
    public static samplecontrollerscript instance;
    public static bool rotating;
    public static List<securityScript> multiplier;
    public static List<PeopleScript> scorePerSecond;
    public static int score;
    IEnumerator rotationshift(RaycastHit hitInfo,float timeToTake) 
    {
        CameraShake.instance.shakeDuration = timeToTake;
        rotating = true;
        float timePassed = 0;
        Vector3 start = transform.forward;
        Vector3 finish = Vector3.Reflect(transform.forward, hitInfo.normal);
        while (timeToTake >= timePassed) 
        {
            Vector3 newfwd = Vector3.Lerp(start, finish, (timePassed) / (timeToTake));
            transform.forward = new Vector3(newfwd.x, 0, newfwd.z);
            yield return new WaitForEndOfFrame();
            timePassed += Time.deltaTime;
        }
        rotating = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        shuddering = new List<Rigidbody>();
        multiplier = new List<securityScript>();
        scorePerSecond = new List<PeopleScript>();
        rotating = false;
        slowDownTime = 0f;
        physicsLayer = LayerMask.NameToLayer("physicsLayer");
        doorLayer = LayerMask.NameToLayer("door");
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
    IEnumerator DoorShutter(Vector3 forward,Vector3 v,Rigidbody rig) {
        bool contained= shuddering.Contains(rig);
        if (!contained)
        {
            shuddering.Add(rig);
        }
        Vector3 fwd = forward.normalized;
        float magnitudeToUse = v.sqrMagnitude;
        while ((magnitudeToUse>10||magnitudeToUse<-10)&&!contained)
        { 
            rig.AddForceAtPosition(Vector3.ClampMagnitude(transform.forward * magnitudeToUse,100f), transform.position, ForceMode.Impulse);
            magnitudeToUse /= -2;
            yield return new WaitForSeconds(.3f);
        }
        if(!contained)
        shuddering.Remove(rig);
    }
    private void Update()
    {
        Vector3 pos = transform.position;
        controllerState state = getControllerstate();
        
        
        MovementBehaviors.rotateAndPositionCam(state, MovementBehaviors.instance._camera);
        //GetComponent<CharacterController>().Move();
        if (MovementBehaviors.instance.pausemovefor >= 0) { 
        MovementBehaviors.instance.pausemovefor-=Time.deltaTime;
        }
        else if (MovementBehaviors.instance.isThirdPerson)
        {
            RaycastHit hitInfo = new RaycastHit();
            UnityEngine.Vector3 movement = MovementBehaviors.cruiseVelocity(state) * Time.deltaTime;
            movement = transform.forward * movement.z + transform.up * movement.y + transform.right * movement.x + new Vector3(0, -MovementBehaviors.instance.gravity * Time.deltaTime, 0); 
            Ray ray = new Ray(MovementBehaviors.instance._camera.transform.parent.position+(Vector3.up*.5f), MovementBehaviors.instance._camera.transform.parent.forward);
            if (slowDownTime >= 0f)
            {
                movement *= slowamount;
            }
            if (Physics.Raycast(ray, out hitInfo, 1.25f) &&!rotating)
            {
                if (hitInfo.transform.gameObject.layer != physicsLayer && hitInfo.transform.gameObject.layer != doorLayer)
                {
                    StartCoroutine(rotationshift(hitInfo, .4f));
                }
                else if (hitInfo.transform.gameObject.layer == doorLayer)
                {
                    StartCoroutine(DoorShutter(transform.forward.normalized, vel, hitInfo.transform.gameObject.GetComponent<Rigidbody>()));
                }
            }    
           //    Vector3 collisionpoint = hitInfo.point;
           //    Vector3 part1 = (hitInfo.point - transform.position).normalized* ((hitInfo.point - transform.position).magnitude);
           //    movement = part1+Vector3.Project(movement,Vector3.Cross(hitInfo.normal, Vector3.up));
           //}
            if(!rotating)
            GetComponent<CharacterController>().Move(movement+ new Vector3(0, -MovementBehaviors.instance.gravity * Time.deltaTime, 0));
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
            GetComponent<CharacterController>().Move(movement + new Vector3(0, -MovementBehaviors.instance.gravity * Time.deltaTime, 0));

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
                if (hit.transform.gameObject.GetComponent<SilkShatter>() != null) {
                    hit.transform.gameObject.GetComponent<SilkShatter>().ShatterFrom(hit.point-transform.forward*2f,vel.magnitude);
                }
                if (hit.transform.gameObject.GetComponent<Bush>() != null)
                {
                    slowDownTime = hit.transform.gameObject.GetComponent<Bush>().slowTime;
                    slowamount = hit.transform.gameObject.GetComponent<Bush>().slowAmount;
                }
                if (hit.rigidbody != null&& hit.transform.gameObject.layer != doorLayer)
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
                        //hit.rigidbody.AddForceAtPosition((hit.transform.position - transform.position).normalized * Time.fixedDeltaTime, hit.point);
                    }
                    
                }
                
                float cruisevel = .5f * (MovementBehaviors.instance.maxCruiseVel - MovementBehaviors.instance.minCruiseVel)+ MovementBehaviors.instance.minCruiseVel;
                if (hit.transform.gameObject.GetComponent<Staircase>() && MovementBehaviors.instance.pausemovefor <= 0)
                {
                    MovementBehaviors.instance.pausemovefor = hit.transform.gameObject.GetComponent<Staircase>().pausemovefor;
                    StartCoroutine(hit.transform.gameObject.GetComponent<Staircase>().belay(cruisevel/2f, this.gameObject));
                }
                if (hit.transform.gameObject.GetComponent<Fountain>() && MovementBehaviors.instance.pausemovefor <= 0)
                {
                    
                    StartCoroutine(hit.transform.gameObject.GetComponent<Fountain>().belay(hit.point,.73f, this.gameObject));
                }

            }
        }


    }
    
}
