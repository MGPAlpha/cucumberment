using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCore : MonoBehaviour
{

    private Rigidbody _rb;

    private float throttle = 0;
    private float currentSpeed = 0;
    private Vector2 turnInput = Vector2.zero;
    private float rollInput = 0;
    [SerializeField] private float baseMaxSpeed = 25;
    [SerializeField] private float baseAcceleration = 10;
    [SerializeField] private float baseBrake = 6;

    [SerializeField] private float baseTurnSpeed = 1f;
    [SerializeField] private float turnSpeedDampingFactor = .4f;
    [SerializeField] private float baseRollSpeed = 60;
    private float currentRollSpeed;
    [SerializeField] private float rollAcceleration = 90;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoll(float val) {
        rollInput = val;
    }

    public void SetThrottle(float val) {
        throttle = val;
    }

    public void SetTurning(Vector2 val) {
        turnInput = val;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        float speedTarget = baseMaxSpeed * throttle;
        if (speedTarget < currentSpeed) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, speedTarget, baseAcceleration * Time.fixedDeltaTime);
        } else if (speedTarget > currentSpeed) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, speedTarget, baseBrake * Time.fixedDeltaTime);
        }

        currentRollSpeed = Mathf.MoveTowards(currentRollSpeed, -baseRollSpeed * rollInput, Time.fixedDeltaTime * rollAcceleration);

        float turnDamping = Mathf.Lerp(1, turnSpeedDampingFactor, Mathf.InverseLerp(0, baseMaxSpeed, currentSpeed));

        Vector2 smoothedTurning = turnInput * Mathf.Pow(turnInput.magnitude, 2);
        transform.Rotate(new Vector3(-smoothedTurning.y, smoothedTurning.x, 0) * baseTurnSpeed * turnDamping * Time.fixedDeltaTime, Space.Self);

        transform.Rotate(new Vector3(0, 0, currentRollSpeed * Time.fixedDeltaTime), Space.Self);

        transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
    }
}
