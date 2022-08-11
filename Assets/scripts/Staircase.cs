using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Staircase : MonoBehaviour
{
    public float pausemovefor;
    public float tolerance;
    public List<GameObject> pathpoints;
    public List<Vector3> path;
    private void Awake()
    {
        if (pathpoints !=null)
        {
            foreach (var pathpoint in pathpoints)
            {
                pathpoint.SetActive(false);
            }
        }
    }
    public IEnumerator belay(float speed, GameObject g) 
    {
        
        bool StartToEnd = !((g.transform.position - path.First()).sqrMagnitude > (g.transform.position - path.Last()).sqrMagnitude);
        if (StartToEnd) {
            int index = 0;
            while (index<=path.Count-1) { 
                
            float distToTarget = (path[index]-g.transform.position).magnitude;
                if (distToTarget <= tolerance)
                {
                    index++;
                    //
                    if (index <= path.Count - 1)
                    {
                        g.GetComponent<CharacterController>().Move(Vector3.ClampMagnitude((path[index] - g.transform.position), speed * .1f));
                        g.transform.forward = pathpoints[index].transform.forward;
                    }
                    MovementBehaviors.instance.pausemovefor = Time.fixedDeltaTime*2;
                    yield return new WaitForFixedUpdate();
                }
                else {
                    g.GetComponent<CharacterController>().Move(Vector3.ClampMagnitude((path[index] - g.transform.position), speed * .1f));
                    g.transform.forward = pathpoints[index].transform.forward;
                    MovementBehaviors.instance.pausemovefor = Time.fixedDeltaTime*2;
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        else {
            int index = path.Count - 1;
            while (index >= 0)
            {
                float distToTarget = (path[index] - g.transform.position).magnitude;
                if (distToTarget < tolerance)
                {
                    index--;
                    g.GetComponent<CharacterController>().Move(Vector3.ClampMagnitude((path[index] - g.transform.position), speed * .1f));
                    g.transform.forward = pathpoints[index].transform.forward * -1f;
                    MovementBehaviors.instance.pausemovefor = Time.fixedDeltaTime * 2;
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    g.GetComponent<CharacterController>().Move(Vector3.ClampMagnitude((path[index] - g.transform.position), speed * .1f));
                   
                    g.transform.forward = pathpoints[index].transform.forward * -1f;
                    MovementBehaviors.instance.pausemovefor = Time.fixedDeltaTime * 2;
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        

    }
    public void OnDrawGizmos() {
       
            if (pathpoints == null) { pathpoints = new List<GameObject>(); }
            int index = 0;
            if (path != null)
                foreach (var pathpoint in path.ToArray())
                {
                    if ((index + 1) > pathpoints.Count)
                    {
                        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        temp.transform.SetParent(this.transform, false);
                        pathpoints.Add(temp);

                    }
                    else
                    {
                        try
                        {
                            path[index] = pathpoints[index].transform.position;
                        }
                        catch
                        {

                            path.RemoveAt(index);
                        if (pathpoints[index] != null) {
                            GameObject.Destroy(pathpoints[index]);
                        }
                            pathpoints.RemoveAt(index);
                        }
                        
                    }
                    index++;
                }
       
        
    }
}
