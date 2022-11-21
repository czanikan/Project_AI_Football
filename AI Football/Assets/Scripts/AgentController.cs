using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public enum Team
{
    Blue = 0,
    Red = 1
}
public class AgentController : Agent
{
    public Team team;

    public float kickPower;
    public float agentMoveSpeed;

    private float moveSpeed = 1;
    
    private EnvironmentParameters resetParams;
    private BehaviorParameters behaviorParameters;
    [HideInInspector]
    public Rigidbody rb;

    private float ballTouch;
    [HideInInspector]
    public Vector3 startPos;

    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
        BACKWARD = 3,
        IDLE = 4
    }


    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        behaviorParameters = GetComponent<BehaviorParameters>();

        if (behaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            startPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
        }
        else
        {
            team = Team.Red;
            startPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
        }

        resetParams = Academy.Instance.EnvironmentParameters;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions[0];

        Vector3 direction = Vector3.zero;

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
            case (int)ACTIONS.IDLE:
                direction = Vector3.zero;
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
        }
        else if (h == +1)
        {
            actions[0] = (int)ACTIONS.RIGHT;
        }
        else if (v == +1)
        {
            actions[0] = (int)ACTIONS.FORWARD;
        }
        else if (v == -1)
        {
            actions[0] = (int)ACTIONS.BACKWARD;
        }
        else
        {
            actions[0] = (int)ACTIONS.IDLE;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AddReward(.2f * ballTouch);
            var dir = collision.contacts[0].point - transform.position;
            dir = dir.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * kickPower);

            Debug.Log("Ball Kicked");
        }
    }

    public override void OnEpisodeBegin()
    {
        ballTouch = resetParams.GetWithDefault("ball_touch", 1);
    }

}
