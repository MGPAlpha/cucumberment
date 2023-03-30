using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipCore))]
public class PlayerShipController : MonoBehaviour
{

    public static PlayerShipController Main {get; private set;}
    public static ShipCore PlayerShip {get => Main ? Main.shipCore : null;}
    public static FuelManager PlayerFuelManager {get => Main ? Main.fuelManager : null;}

    private ShipCore shipCore;
    private FuelManager fuelManager;

    public float Throttle {get; private set;}
    [SerializeField] private float throttleSpeed = 1;

    public Vector2 TurnInput {get; private set;}
    public Vector2 SmoothedTurning {get; private set;}
    [SerializeField] private float turnSensitivity = 5f;

    public DockingField ActiveDockingField {get; private set;}

    [SerializeField] private float dockingTime = 5;
    private float dockingTimer = 0;
    public float DockingProgress { get => dockingTimer / dockingTime; }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Main = this;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        if (Main == this) Main = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        shipCore = GetComponent<ShipCore>();
        fuelManager = GetComponent<FuelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            Throttle = Mathf.MoveTowards(Throttle, 1, throttleSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.S)) {
            Throttle = Mathf.MoveTowards(Throttle, 0, throttleSpeed * Time.deltaTime);
        }

        shipCore.SetThrottle(Throttle);

        Vector2 newTurnInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * turnSensitivity *.01f;
        // float aspRatio = Screen.width / Screen.height;
        // newTurnInput.y /= aspRatio;
        TurnInput += newTurnInput * Mathf.Lerp(1, Mathf.InverseLerp(1, .2f, TurnInput.magnitude), Vector2.Dot(newTurnInput.normalized, TurnInput.normalized));

        TurnInput = Vector2.ClampMagnitude(TurnInput, 1);

        Vector2 deadZonedTurn = TurnInput * Mathf.InverseLerp(.1f, 1, TurnInput.magnitude);

        // SmoothedTurning = deadZonedTurn * (-Mathf.Pow(deadZonedTurn.magnitude-1, 4)+1);
        SmoothedTurning = deadZonedTurn;

        shipCore.SetTurning(SmoothedTurning);

        float rollInput = 0;
        if (Input.GetKey(KeyCode.D)) rollInput++;
        if (Input.GetKey(KeyCode.A)) rollInput--;
        shipCore.SetRoll(rollInput);

        shipCore.SetBoost(Input.GetKey(KeyCode.LeftShift));

        if (ActiveDockingField && Input.GetKey(KeyCode.E)) {
            dockingTimer += Time.deltaTime;
            if (dockingTimer >= dockingTime) {
                ActiveDockingField.Dock();
            }
        } else {
            dockingTimer = 0;
        }
    }

    public void RegisterDockingField(DockingField field) {
        ActiveDockingField = field;
    }

    public void RemoveDockingField(DockingField field) {
        if (ActiveDockingField == field) {
            ActiveDockingField = null;
        }
    }
}
