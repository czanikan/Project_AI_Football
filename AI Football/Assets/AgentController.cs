using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class AgentController : Agent
{
    [HideInInspector]
    public Rigidbody rb;
    private BehaviorParameters behaviorParameters;

    public float moveSpeed;
    public float agentMoveSpeed;

    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
        BACKWARD = 3
    }


    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        behaviorParameters = GetComponent<BehaviorParameters>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions[0];

        Vector3 direction = Vector3.zero;

        Debug.Log(actionTaken);

        switch (actionTaken)
        {
            case (int)ACTIONS.FORWARD:
                direction = transform.forward * moveSpeed;
                break;
            case (int)ACTIONS.BACKWARD:
                direction = transform.forward * -moveSpeed;
                break;
            case (int)ACTIONS.RIGHT:
                direction = transform.right * moveSpeed;
                break;
            case (int)ACTIONS.LEFT:
                direction = transform.right * -moveSpeed;
                break;
        }

        rb.AddForce(direction * agentMoveSpeed, ForceMode.VelocityChange);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h == -1)
        {
            actions[0] = (int)ACTIONS.LEFT;
            Debug.Log((int)ACTIONS.LEFT);
        }
        else if (h == +1)
        {
            actions[0] = (int)ACTIONS.RIGHT;
            Debug.Log(actions[0]);
        }
        else if (v == +1)
        {
            actions[0] = (int)ACTIONS.FORWARD;
            Debug.Log(actions[0]);
        }
        else if (v == -1)
        {
            actions[0] = (int)ACTIONS.BACKWARD;
            Debug.Log(actions[0]);
        }
        else
        {
            actions[0] = 3;
            Debug.Log("No action");
        }
    }

}
