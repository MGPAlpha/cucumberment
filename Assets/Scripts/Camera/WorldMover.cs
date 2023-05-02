using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class WorldMover : MonoBehaviour
{

    public static Action<Vector3> MoveWorld;

    [SerializeField] private float threshold = 1000;
    [SerializeField] private float increment = 500;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 position = transform.position;
        Vector3 translation = Vector3.zero;
        if (position.x > threshold) translation.x -= 1;
        if (position.x < -threshold) translation.x += 1;
        if (position.y > threshold) translation.y -= 1;
        if (position.y < -threshold) translation.y += 1;
        if (position.z > threshold) translation.z -= 1;
        if (position.z < -threshold) translation.z += 1;

        // print("translation " + translation);

        if (translation != Vector3.zero) {
            translation *= increment;
            transform.position += translation;
            MoveWorld.Invoke(translation);
        }
    }

    public void Recenter() {
        MoveWorld(-transform.position);
        transform.position = Vector3.zero;
    }
}
