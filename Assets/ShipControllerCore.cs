using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControllerCore : MonoBehaviour
{

    private Rigidbody _rb;

    private float throttle = 0;
    private float currentSpeed = 0;
    private Vector2 turnInput = Vector2.zero;
    [SerializeField] private float throttleSpeed = 1;
    [SerializeField] private float baseMaxSpeed = 25;
    [SerializeField] private float baseAcceleration = 10;
    [SerializeField] private float baseBrake = 6;

    [SerializeField] private float turnSensitivity = 1f;
    [SerializeField] private float baseTurnSpeed = 1f;
    [SerializeField] private float turnSpeedDampingFactor = .4f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            throttle = Mathf.MoveTowards(throttle, 1, throttleSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.S)) {
            throttle = Mathf.MoveTowards(throttle, 0, throttleSpeed * Time.deltaTime);
        }

        Vector2 newTurnInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * turnSensitivity;
        float aspRatio = Screen.width / Screen.height;
        newTurnInput.y /= aspRatio;
        turnInput += newTurnInput;

        turnInput = Vector2.ClampMagnitude(turnInput, 1);


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

        float turnDamping = Mathf.Lerp(1, turnSpeedDampingFactor, Mathf.InverseLerp(0, baseMaxSpeed, currentSpeed));

        Vector2 smoothedTurning = turnInput * Mathf.Pow(turnInput.magnitude, 2);
        transform.Rotate(new Vector3(smoothedTurning.y, smoothedTurning.x, 0) * baseTurnSpeed * turnDamping * Time.fixedDeltaTime, Space.Self);

        transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
    }
}
