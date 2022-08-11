using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chairScript : MonoBehaviour
{
    bool onGround = false;
    void Start()
    {
        onGround = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.realtimeSinceStartup > 4f)
        {
            if (Physics.Raycast(transform.position, -transform.up,.6f,1<<0))
            {
                onGround = true;
            }
            else
            {
                if (onGround)
                {
                    if (!debate_level.chairsFlipped.Contains(this)) 
                    {
                        debate_level.chairsFlipped.Add(this);
                    }
                    onGround = false;
                }

            }
        }
    }
}
