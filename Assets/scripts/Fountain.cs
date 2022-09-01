using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    bool belaying;
    public GameObject Apex;
    public GameObject SampleStart;
    public Vector3 End;
    private void Start()
    {
        belaying = false;
        Apex.GetComponent<MeshRenderer>().enabled = false;
        SampleStart.GetComponent<MeshRenderer>().enabled = false;
    }
    Vector3 biezerInTime(Vector3 start, float timepassed, float totaltime) {
        
        End = start + ((Apex.transform.position - start) * 2.2f);
        End = new Vector3(End.x, start.y, End.z);
        Vector3 D = Apex.transform.position;
        float distFromHalf = Mathf.Abs((timepassed-(totaltime / 2)))/totaltime;
        Vector3 spot = Vector3.Lerp(start, End, (timepassed / totaltime))-((distFromHalf*distFromHalf)/(totaltime/2))*totaltime*2f*new Vector3(0,D.y - start.y,0)+ new Vector3(0, D.y - start.y, 0);
        return spot;
    }
    private void OnDrawGizmos()
    {
        if (SampleStart == null) { 

            SampleStart = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            SampleStart.transform.SetParent(transform);
        }
        if (Apex == null)
        {
            Apex = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Apex.transform.SetParent(transform);
        }

        Vector3 Start = SampleStart.transform.position;

        for (float i = 1; i <= 5; i+=1f)
        {
            Gizmos.DrawSphere(biezerInTime(Start, i, 5f),1f);
        }
        

    }
    public IEnumerator belay(Vector3 start,float timetofinish, GameObject g) {
        float timePassed = 0;
        belaying = true;
        while (timePassed<timetofinish) {
            MovementBehaviors.instance.pausemovefor = Time.fixedDeltaTime * 2f;
            g.transform.position = biezerInTime(start, timePassed, timetofinish);
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }
}