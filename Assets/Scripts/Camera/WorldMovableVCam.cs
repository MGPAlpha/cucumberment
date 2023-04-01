using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WorldMovableVCam : MonoBehaviour
{

    private CinemachineVirtualCamera vCam;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        WorldMover.MoveWorld += Move;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        WorldMover.MoveWorld -= Move;
    }

    void Move(Vector3 offset) {
        vCam.OnTargetObjectWarped(vCam.Follow, offset);
    }
}
