using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestingCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float yOffset = -2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, transform.position.z);
    }
}
