using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CrashCamera : MonoBehaviour
{
    public static CrashCamera Main {get; private set;}

    private CinemachineVirtualCamera cmVCam;
    private CinemachineTransposer transposer;

    private Vector3 cameraOffset;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cmVCam = GetComponent<CinemachineVirtualCamera>();
        transposer = ((CinemachineTransposer)cmVCam.GetComponentPipeline()[0]);
        cameraOffset = transposer.m_FollowOffset;
        gameObject.SetActive(false);
    }

    public void ActivateCrash(Transform ship) {
        transposer.m_FollowOffset = Camera.main.transform.position - ship.position;
        gameObject.SetActive(true);
        cmVCam.ForceCameraPosition(Camera.main.transform.position, Camera.main.transform.rotation);
    }

    public void DeactivateCrash() {
        gameObject.SetActive(false);
    }
}
