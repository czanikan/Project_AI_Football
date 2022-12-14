using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gm;
    void Start()
    {
        gm = GetComponentInParent<GameManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BlueGoal"))
        {
            gm.Goal("Red");
            Debug.Log("Red team scored a goal!");
        }
        if (other.gameObject.CompareTag("RedGoal"))
        {
            gm.Goal("Blue");
            Debug.Log("Blue team scored a goal!");
        }
    }
}
