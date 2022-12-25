using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    private Vector3 originalPosition;

    //private BehaviorParameters behaviorParameters;

    //private CarController carController;

    private Rigidbody carControllerRigidBody;

    private CarSpots carSpots;


    public override void Initialize()
    {
        originalPosition = transform.localPosition;
       // behaviorParameters = GetComponent<BehaviorParameters>();
        //carController = GetComponent<CarController>();
        //carControllerRigidBody = carController.GetComponent<Rigidbody>();
        carSpots = transform.parent.GetComponentInChildren<CarSpots>();

        ResetParkingLotArea();
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation);

        sensor.AddObservation(carSpots.CarGoal.transform.position);
        sensor.AddObservation(carSpots.CarGoal.transform.rotation);

        sensor.AddObservation(carControllerRigidBody.velocity);
    }

    public override void OnEpisodeBegin() {
        ResetParkingLotArea();
    }


    public void ResetParkingLotArea() {
        //carController.IsAutonomous = behaviorParameters.BehaviorType == BehaviorType.Default;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        carControllerRigidBody.velocity = Vector3.zero;
        carControllerRigidBody.angularVelocity = Vector3.zero;

        carSpots.Setup();
    }

    //Giving reward to the agent
    public void GivePoints(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);

        if (isFinal)
        {
            //StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));

            EndEpisode();
        }
    }

    //Taking away reward from agent
    public void TakeAwayPoints() {
        //StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));

        AddReward(-0.01f);

        EndEpisode();
    }

    void Update()
    {
        if (transform.localPosition.y <= 0)
        {
            TakeAwayPoints();
        }
    }
    /*
     * TODO => Implement Car Controller
    public void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && carController.canApplyTorque())
        {
            actionsOut[0] = 3;
        }

        if (Input.GetKey(KeyCode.RightArrow) && carController.canApplyTorque())
        {
            actionsOut[0] = 4;
        }
    }*/

}
