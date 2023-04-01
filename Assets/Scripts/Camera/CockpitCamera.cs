using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CockpitCamera : MonoBehaviour
{
    public static CockpitCamera Main {get; private set;}

    private CinemachineVirtualCamera cmVCam;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        cmVCam = GetComponent<CinemachineVirtualCamera>();
        cmVCam.enabled = false;
    }

    public void EnterCockpit() {
        cmVCam.enabled = true;
    }

    public void ExitCockpit() {
        cmVCam.enabled = false;
    }
}
