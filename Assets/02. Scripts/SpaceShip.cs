using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class SpaceShip : Agent
{
    public GameObject ship;

    private Transform tr;
    private Rigidbody rb;

    public Transform pltr;
    public Rigidbody plrb;
    public GameObject planet;

    public Transform targetTr;
    public Renderer floorRd;

    private Material originMt;
    public Material badMt;
    public Material goodMt;

    Vector3 force = new Vector3(170f, 0, 0); 

    Vector3 force2 = new Vector3(500f, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 8f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        originMt = floorRd.material;

        MaxStep = 4500;
    }

    //에피소드(학습단위)가 시작할때마다 호출
    public override void OnEpisodeBegin()
    {
        //물리력을 초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        tr.localPosition = new Vector3(0, 0.05f, 4);


        plrb.velocity = Vector3.zero;
        plrb.angularVelocity = Vector3.zero;

        pltr.localPosition = new Vector3(0, 1.5f, 10);

        ship.GetComponent<Rigidbody>().AddForce(force);
        planet.GetComponent<Rigidbody>().AddForce(force2);

        StartCoroutine(RevertMaterial());
    }

    //환경 정보를 관측 및 수집해 정책 결정을 위해 브레인에 전달하는 메소드
    public override void CollectObservations(Unity.MLAgents.Sensors.VectorSensor sensor)
    {
        sensor.AddObservation(pltr.localPosition);
        sensor.AddObservation(targetTr.localPosition);
        sensor.AddObservation(tr.localPosition);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousActions = actions.ContinuousActions;

        Vector3 velocity = rb.velocity;
        Vector3 direction = velocity.normalized;
        


        float v = continuousActions[0];
        Debug.Log($"value of v: {v}");
        Vector3 dir = (direction * v);
  


        rb.AddForce(dir.normalized * 0.1f);

        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        Debug.Log($"[0]={continuousActionsOut[0]}");
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("OUTER_SPACE"))
        {
            floorRd.material = goodMt;

            SetReward(+1.0f);

            EndEpisode();
        }

        if (coll.collider.CompareTag("STAR"))
        {
            floorRd.material = badMt;

            SetReward(-1.0f);

            EndEpisode();
        }

        if (coll.collider.CompareTag("PLANET"))
        {
            floorRd.material = badMt;

            SetReward(-1.0f);

            EndEpisode();
        }
    }

    IEnumerator RevertMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        floorRd.material = originMt;
    }
}
