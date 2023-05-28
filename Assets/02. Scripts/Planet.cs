using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject ship;
    public GameObject planet;
    public float gravitationalPull;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //apply spherical gravity to selected objects (set the objects in editor)
        float distance = Vector3.Distance(planet.transform.position, ship.transform.position);

        if (ship.GetComponent<Rigidbody>())
        {
            ship.GetComponent<Rigidbody>().AddForce((planet.transform.position - ship.transform.position).normalized * gravitationalPull / (distance * distance));
        }
    }
 }
    
