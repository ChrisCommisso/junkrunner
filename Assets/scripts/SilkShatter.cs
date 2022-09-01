using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilkShatter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            gameObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.transform.GetChild(i).gameObject.GetComponent<SphereCollider>().enabled = false;
        }
        


    }
    public void Shatter() { 
        gameObject.GetComponent<MeshCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            
            gameObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(i).gameObject.GetComponent<SphereCollider>().enabled = true;
            deSpawnIn(20f, gameObject.transform.GetChild(i).gameObject);
        }
    }
    IEnumerator deSpawnIn(float time, GameObject g) {
        g.GetComponent<Rigidbody>().MoveRotation(Quaternion.EulerAngles(Random.value*360f, Random.value * 360f, Random.value * 360f));
        yield return new WaitForSeconds(time);
        GameObject.Destroy(g);
        
    }
    public void ShatterFrom(Vector3 center,float speed)
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            child.GetComponent<Rigidbody>().isKinematic = false;
            child.GetComponent<MeshRenderer>().enabled = true;
            child.GetComponent<SphereCollider>().enabled = true;
            child.GetComponent<Rigidbody>().velocity = (child.transform.position - center).normalized * speed;
            StartCoroutine(deSpawnIn(4f, child));
        }
    }
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player"&&MovementBehaviors.instance.isThirdPerson) {
            Shatter();
        }
    }
}
