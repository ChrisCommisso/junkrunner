using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PeopleNode : MonoBehaviour
{
    public float width;
    public float length;
    public float height;
    public int peopleAmount;
    public GameObject person;
    // Start is called before the first frame update
    void Start()
    {
        if(person.GetComponent<securityScript>()==null)
        for (float x = 0; x < width; x+=3f)
        {
            for (float z = 0; z < length; z += 3f)
            {
                RaycastHit hit1 = new RaycastHit();
                if (Physics.Raycast(transform.position + new Vector3(x, height, z), -Vector3.up, out hit1)) 
                {
                    GameObject g = new GameObject(gameObject.name+"node"+x+" "+z);
                    g.transform.position = hit1.point;
                    g.AddComponent<AstarPoint>();
                    float wieght = 0;
                    if (hit1.transform.tag == "OffLimits" || hit1.transform.gameObject.layer == LayerMask.NameToLayer("physicsLayer")) 
                    {
                        wieght += 10;
                    }

                    PeopleScript.AllPoints.Add(new Tuple<AstarPoint,float>(g.GetComponent<AstarPoint>(), wieght));
                }
                if (Physics.Raycast(transform.position + new Vector3(-x, height, z), -Vector3.up, out hit1))
                {
                    GameObject g = new GameObject(gameObject.name + "node" + -x + " " + z);
                    g.transform.position = hit1.point;
                    g.AddComponent<AstarPoint>();
                    float wieght = 0;
                    if (hit1.transform.tag == "OffLimits" || hit1.transform.gameObject.layer == LayerMask.NameToLayer("physicsLayer"))
                    {
                        wieght += 10;
                    }

                    PeopleScript.AllPoints.Add(new Tuple<AstarPoint, float>(g.GetComponent<AstarPoint>(), wieght));
                }
                if (Physics.Raycast(transform.position + new Vector3(-x, height, -z), -Vector3.up, out hit1))
                {
                    GameObject g = new GameObject(gameObject.name + "node" + -x + " " + -z);
                    g.transform.position = hit1.point;
                    g.AddComponent<AstarPoint>();
                    float wieght = 0;
                    if (hit1.transform.tag == "OffLimits" || hit1.transform.gameObject.layer == LayerMask.NameToLayer("physicsLayer"))
                    {
                        wieght += 10;
                    }

                    PeopleScript.AllPoints.Add(new Tuple<AstarPoint, float>(g.GetComponent<AstarPoint>(), wieght));
                }
                if (Physics.Raycast(transform.position + new Vector3(x, height, -z), -Vector3.up, out hit1))
                {
                    GameObject g = new GameObject(gameObject.name + "node" + x + " " + -z);
                    g.transform.position = hit1.point;
                    g.AddComponent<AstarPoint>();
                    float wieght = 0;
                    if (hit1.transform.tag == "OffLimits" || hit1.transform.gameObject.layer == LayerMask.NameToLayer("physicsLayer"))
                    {
                        wieght += 10;
                    }

                    PeopleScript.AllPoints.Add(new Tuple<AstarPoint, float>(g.GetComponent<AstarPoint>(), wieght));
                }
            }
        }
        for (int i = 0; i < peopleAmount; i++)
        {
            float x = (UnityEngine.Random.value * width) - width * .5f;
            float z = (UnityEngine.Random.value * length) - length * .5f;
            
            float y = transform.position.y;//must change
            RaycastHit hit1 = new RaycastHit();
            if (Physics.Raycast(transform.position + new Vector3(0, height, 0), -Vector3.up, out hit1))
            {
                y = hit1.point.y;
            }
            GameObject g = Instantiate(person, new Vector3(transform.position.x + x, y, transform.position.z),Quaternion.identity);
            g.GetComponent<PeopleScript>().startMethod();
        }
        PeopleScript.init();
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position - new Vector3(width, 0, length), transform.position + new Vector3(width, 0, length));
        Gizmos.DrawLine(transform.position - new Vector3(0, height, 0), transform.position + new Vector3(0, height, 0));
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
