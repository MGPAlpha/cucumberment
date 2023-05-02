using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateController : MonoBehaviour
{
    
    private ShipCore shipCore;
    private PositionBroadcaster positionBroadcaster;

    private Vector3 anchorPoint;
    private Vector3 patrolPoint;

    [SerializeField] private float playerDetectDistance = 2000;

    [SerializeField] private float patrolDistance = 1000;
    [SerializeField] private float patrolSuccessMax = 50;
    [SerializeField] private float awarenessTimeLimit = 5;
    private float awarenessTimer;

    private bool awareOfPlayer = false;
    
    // Start is called before the first frame update
    void Start()
    {

        shipCore = GetComponent<ShipCore>();
        positionBroadcaster = GetComponent<PositionBroadcaster>();
        positionBroadcaster.enabled = false;
        anchorPoint = transform.position;
        SetNewPatrolPoint();
    }

    public void AdjustTrackedPositions(Vector3 offset) {
        anchorPoint += offset;
        patrolPoint += offset;
    }

    void SetNewPatrolPoint() {
        patrolPoint = Random.onUnitSphere * Random.Range(0, patrolDistance) + anchorPoint;
    }

    void AimToward(Vector3 target) {
        Vector3 toTarget = target - transform.position;

        Debug.Log("dist to target " + toTarget.magnitude);
        
        Vector3 localDirection = transform.InverseTransformDirection(toTarget);

        Vector2 turn = Vector2.ClampMagnitude(new Vector2(localDirection.x, localDirection.y), 1);
        shipCore.SetTurning(turn);
    }

    void UpdatePatrol() {
        Vector3 distToPatrolPoint = transform.position - patrolPoint;
        if (distToPatrolPoint.magnitude < patrolSuccessMax) {
            SetNewPatrolPoint();
        }

        AimToward(patrolPoint);
        shipCore.SetThrottle(.5f);
    }

    void UpdateTargetPlayer() {
        Vector3 playerPos = PlayerShipController.Main.transform.position;
        AimToward(playerPos);

        Vector3 currAimDir = transform.forward;
        Vector3 dirToPlayer = playerPos - transform.position;

        float dot = Vector3.Dot(currAimDir.normalized, dirToPlayer.normalized);

        shipCore.SetThrottle(Mathf.InverseLerp(0, 1, dot));
    }

    

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = PlayerShipController.Main.transform.position - transform.position;
        
        if (!awareOfPlayer && toPlayer.magnitude <= playerDetectDistance) {
            awareOfPlayer = true;
            positionBroadcaster.enabled = true;
        } else if (awareOfPlayer) {
            if (toPlayer.magnitude > playerDetectDistance) {
                awarenessTimer += Time.deltaTime;
                if (awarenessTimer >= awarenessTimeLimit) {
                    awareOfPlayer = false;
                    positionBroadcaster.enabled = false;
                }
            } else {
                awarenessTimer = 0;
            }
        }

        if (!awareOfPlayer) {
            UpdatePatrol();
        } else {
            UpdateTargetPlayer();
        }
    }


    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, patrolDistance);
    }
}
