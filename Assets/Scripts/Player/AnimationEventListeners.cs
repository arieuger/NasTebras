using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventListeners : MonoBehaviour
{

    public void AirAnimObserver(string message)
    {
        if (message.Equals("TransitionFallEnded"))
        {
            GetComponent<Animator>().Play("Fall");
        }
    }

    public void DashAnimObserver(string message)
    {
        if (message.Equals("StartDashEnded"))
        {
            GetComponent<Animator>().Play("DashLoop");
        }
    }
}
