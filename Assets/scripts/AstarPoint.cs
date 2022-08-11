using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts
{
    public class AstarPoint : MonoBehaviour
    {
        public static Dictionary<string, Vector3> ordinances;
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position,.1f);
        }
       
        public List<Tuple<AstarPoint,float>> neighbors;
        public void findMyNeighbors(float radius) 
        {
            neighbors = new List<Tuple<AstarPoint, float>>();
        
            foreach (var point in PeopleScript.AllPoints)
            {
                if ((transform.position - point.Item1.transform.position).magnitude <= radius&&transform.position!=point.Item1.transform.position)
                {
                    neighbors.Add(point);
                }
                
            }
        }
        public static float heuristic(Vector3 s, Vector3 f)
        {
            return (new Vector3(s.x, 0, s.z) - new Vector3(f.x, 0, f.z)).magnitude;
        }
        // Start is called before the first frame update
        void Start()
        {
            if (ordinances == null) 
            {
                ordinances = new Dictionary<string, Vector3>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
