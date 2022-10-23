using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gm;
    void Start()
    {
        gm = GetComponentInParent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BlueGoal"))
        {
            gm.Goal("Red");
        }
        if (other.gameObject.CompareTag("RedGoal"))
        {
            gm.Goal("Blue");
        }
    }
}
