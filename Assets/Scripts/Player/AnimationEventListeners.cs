using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventListeners : MonoBehaviour
{

    public void AirAnimObserver(string message)
    {
        if (message.Equals("TransitionFallEnded"))
        {
            Debug.Log("Holi");
            GetComponent<Animator>().Play("Fall");
        }
    }
}
