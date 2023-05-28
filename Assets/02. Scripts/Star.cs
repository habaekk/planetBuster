using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star : MonoBehaviour
{

    public List<GameObject> objects;
    public GameObject star;

    public float gravitationalPull;

    void FixedUpdate()
    {
        
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            float distance = Vector3.Distance(star.transform.position, o.transform.position);
            if (o.GetComponent<Rigidbody>() && o != star)
            {
                if (distance < 30)
                {
                    o.GetComponent<Rigidbody>().AddForce((star.transform.position - o.transform.position).normalized * gravitationalPull / (distance * distance));
                }
            }
        }
    }
}