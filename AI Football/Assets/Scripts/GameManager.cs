using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<AgentController> Agents = new List<AgentController>();

    private SimpleMultiAgentGroup blueAgentGroup;
    private SimpleMultiAgentGroup redAgentGroup;

    [SerializeField] GameObject ball;
    private Vector3 ballStartPos;

    public int MaxEnvironmentSteps = 25000;
    private int resetTimer = 0;

    void Start()
    {
        ballStartPos = ball.transform.position;

        blueAgentGroup = new SimpleMultiAgentGroup();
        redAgentGroup = new SimpleMultiAgentGroup();

        foreach (AgentController agent in Agents)
        {
            if (agent.team == Team.Blue)
            {
                blueAgentGroup.RegisterAgent(agent);
            }
            else
            {
                redAgentGroup.RegisterAgent(agent);
            }
        }
        ResetGame();
    }

    private void ResetGame()
    {
        foreach (Agent agent in Agents)
        {
            agent.transform.position = agent.GetComponent<AgentController>().startPos;
            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            agent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        ball.transform.position = ballStartPos;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        resetTimer = 0;
    }

    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            blueAgentGroup.GroupEpisodeInterrupted();
            redAgentGroup.GroupEpisodeInterrupted();
            ResetGame();
        }
    }

    public void Goal(string teamName)
    {
        if (teamName == "Blue")
        {
            blueAgentGroup.AddGroupReward(1 - (float)resetTimer / MaxEnvironmentSteps);
            redAgentGroup.AddGroupReward(-1);
        }
        if (teamName == "Red")
        {
            redAgentGroup.AddGroupReward(1 - (float)resetTimer / MaxEnvironmentSteps);
            blueAgentGroup.AddGroupReward(-1);
        }
        redAgentGroup.EndGroupEpisode();
        blueAgentGroup.EndGroupEpisode();
        ResetGame();
    }
}
