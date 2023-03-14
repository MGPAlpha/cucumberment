using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipCore))]
public class PlayerShipController : MonoBehaviour
{

    private ShipCore shipCore;

    private float throttle;
    [SerializeField] private float throttleSpeed = 1;

    private Vector2 turnInput;
    [SerializeField] private float turnSensitivity = 5f;

    // Start is called before the first frame update
    void Start()
    {
        shipCore = GetComponent<ShipCore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            throttle = Mathf.MoveTowards(throttle, 1, throttleSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.S)) {
            throttle = Mathf.MoveTowards(throttle, 0, throttleSpeed * Time.deltaTime);
        }

        shipCore.SetThrottle(throttle);

        Vector2 newTurnInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * turnSensitivity *.01f;
        float aspRatio = Screen.width / Screen.height;
        newTurnInput.y /= aspRatio;
        turnInput += newTurnInput;

        turnInput = Vector2.ClampMagnitude(turnInput, 1);

        shipCore.SetTurning(turnInput);

        float rollInput = 0;
        if (Input.GetKey(KeyCode.D)) rollInput++;
        if (Input.GetKey(KeyCode.A)) rollInput--;
        shipCore.SetRoll(rollInput);
    }
}
