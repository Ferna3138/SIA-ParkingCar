using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGoal : MonoBehaviour
{
    private CarAgent agent = null;

    [SerializeField] private GoalType goalType = GoalType.Milestone;

    [SerializeField] private float goalReward = 1f;

    [SerializeField] private bool enforceGoalMinRotation = false;

    [SerializeField] private float goalMinRotation = 10.0f;

    //Prevent AI from cheating
    public bool HasCarUsedIt { get; set; } = false;

    public enum GoalType
    {
        Milestone,
        FinalDestination
    } 

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag.ToLower() == "player" && !HasCarUsedIt) {
            agent = transform.parent.GetComponentInChildren<CarAgent>();

            if (goalType == GoalType.Milestone)
            {
                HasCarUsedIt = true;
                agent.GivePoints(goalReward);
            }
            else {
                //If its a final destination
                //This ensures the car tries to align when parking
                if (Mathf.Abs(agent.transform.rotation.y) <= goalMinRotation || !enforceGoalMinRotation)
                {
                    HasCarUsedIt = true;
                    agent.GivePoints(goalReward, true);
                }
                else {
                    agent.TakeAwayPoints();
                }
            }

        }
    }
}
