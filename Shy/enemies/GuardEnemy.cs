using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GuardEnemy : Enemy
{
    [SerializeField] private List<Vector3> guardPos = new List<Vector3>();
    [SerializeField] private float waitTime;
    private float timer;
    private int currentPos;

    void Start()
    {
        currentPos = 0;
    }

    void FixedUpdate()
    {
        timer--;
        if(timer < 0)
        {
            timer = waitTime;
            base.MoveTo(guardPos[currentPos]);
            currentPos++;
            if(currentPos == guardPos.Count)
            {
                currentPos = 0;
            }
        }
    }
}
