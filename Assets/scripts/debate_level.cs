using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debate_level : MonoBehaviour
{
    float timePassed;
    public static List<chairScript> chairsFlipped;

    public static debate_level instance;
    // Start is called before the first frame update
    void Start()
    {
        chairsFlipped = new List<chairScript>();
        timePassed = 0;
        if (instance == null)
            instance = this;
       
    }
   
    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= 1) 
        {
            samplecontrollerscript.score += ((int)Mathf.Clamp(samplecontrollerscript.multiplier.Count,1,float.MaxValue) * samplecontrollerscript.scorePerSecond.Count)/100;
            timePassed = 0;
        }
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Score: " + samplecontrollerscript.score; 
        
    }
}
