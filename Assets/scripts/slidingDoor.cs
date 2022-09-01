using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor : MonoBehaviour
{
    public GameObject DoorA;
    public GameObject DoorB;
    public GameObject startDoorA;
    public GameObject endDoorA;
    public GameObject startDoorB;
    public GameObject endDoorB;
    public TransformState FinalStateA;
    public TransformState StartStateA;
    public TransformState FinalStateB;
    public TransformState StartStateB;
    private void Start()
    {


        FinalStateA = new TransformState(endDoorA.transform);
        FinalStateB = new TransformState(endDoorB.transform);
        StartStateA = new TransformState(startDoorA.transform);
        StartStateB = new TransformState(startDoorB.transform);
        startDoorA.SetActive(false);
        endDoorA.SetActive(false);
        startDoorB.SetActive(false);
        endDoorB.SetActive(false);
        StartCoroutine(DoorLoop());
    }
    IEnumerator DoorLoop() {
        while (true) {
            StartCoroutine(DoorCycle(3f));
            yield return new WaitForSeconds(3f);
        }
    }
    public IEnumerator DoorCycle(float time) {
        StartCoroutine(Open(time / 5f));
        yield return new WaitForSeconds(((time*3)/5f));
        StartCoroutine(Shut(time / 5f));
    }
    public IEnumerator Shut(float time) 
    { 
        float timepassed=0;
        while (timepassed < time)
        {
            DoorB.transform.position = Vector3.Lerp(FinalStateB.location, StartStateB.location, timepassed / time);
            DoorA.transform.position = Vector3.Lerp(FinalStateA.location,StartStateA.location, timepassed / time);
            timepassed+=Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator Open(float time)
    {
        float timepassed = 0;
        while (timepassed < time)
        {
            DoorB.transform.position = Vector3.Lerp(StartStateB.location, FinalStateB.location, timepassed / time);
            DoorA.transform.position = Vector3.Lerp(StartStateA.location, FinalStateA.location, timepassed / time);
            timepassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    // Start is called before the first frame update
    public void OnDrawGizmos()
    {
        if (startDoorA == null) {
            startDoorA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            startDoorA.transform.parent = transform;
            startDoorA.transform.name = transform.name + "StartDoorA";
        }
        else
        {
            if(StartStateA == null)
                StartStateA = new TransformState();
            StartStateA.location = startDoorA.transform.position;
        }
        if (endDoorA == null)
        {
            endDoorA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            endDoorA.transform.parent = transform;
            endDoorA.transform.name = transform.name + "endDoorA";
        }
        else
        {
            if (FinalStateA == null)
                FinalStateA = new TransformState();
            FinalStateA.location = startDoorA.transform.position;
        }
        if (startDoorB == null)
        {
            startDoorB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            startDoorB.transform.parent = transform;
            startDoorB.transform.name = transform.name + "StartDoorB";
        }
        else
        {
            if (StartStateB == null)
                StartStateB = new TransformState();
            StartStateB.location = startDoorA.transform.position;
        }
        if (endDoorB == null)
        {
            endDoorB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            endDoorB.transform.parent = transform;
            endDoorB.transform.name = transform.name + "endDoorB";
        }
        else
        {
            if (FinalStateB == null)
                FinalStateB = new TransformState();
            FinalStateB.location = startDoorA.transform.position;
        }



    }
}
