using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointMarker : MonoBehaviour
{
    private PositionBroadcaster target;
    private RectTransform rectTransform;
    private RectTransform parent;

    [SerializeField] private Image pointer;

    public void Initialize(PositionBroadcaster target) {
        this.target = target;
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = (RectTransform)transform;
        parent = (RectTransform)rectTransform.parent;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

        if (Vector3.Dot(Camera.main.transform.forward, target.transform.position - Camera.main.transform.position) < 0) {
            int width = Screen.width;
            int height = Screen.height;
            if (screenPos.x < width/2) {
                screenPos.x = float.PositiveInfinity;
            } else {
                screenPos.x = float.NegativeInfinity;
            }

            if (screenPos.y < height/2) {
                screenPos.y = float.PositiveInfinity;
            } else {
                screenPos.y = float.NegativeInfinity;
            }
        }

        // Debug.Log("screen pos " + screenPos);
        Vector3[] parentCorners = new Vector3[4];
        parent.GetWorldCorners(parentCorners);
        Rect parentRect = new Rect(parentCorners[0].x, parentCorners[0].y, parentCorners[2].x - parentCorners[0].x, parentCorners[2].y - parentCorners[0].y);

        // Debug.Log(parent.name + " Rect: " + parentCorners);

        Vector2 clampedScreenPos;

        clampedScreenPos.x = Mathf.Clamp(screenPos.x, parentRect.min.x, parentRect.max.x);
        clampedScreenPos.y = Mathf.Clamp(screenPos.y, parentRect.min.y, parentRect.max.y);

        Vector2 borderedScreenPos = clampedScreenPos;

        float borderAmount = WaypointDisplay.Main.ArrowFitBorder;
        Rect arrowFitBorder = parentRect;
        arrowFitBorder.min += new Vector2(borderAmount, borderAmount);
        arrowFitBorder.max -= new Vector2(borderAmount, borderAmount);
        
        borderedScreenPos.x = Mathf.Clamp(borderedScreenPos.x, arrowFitBorder.min.x, arrowFitBorder.max.x);
        borderedScreenPos.y = Mathf.Clamp(borderedScreenPos.y, arrowFitBorder.min.y, arrowFitBorder.max.y);
        

        // if (borderedScreenPos.x != clampedScreenPos.x && borderedScreenPos.y != clampedScreenPos.y) {

        Vector2 borderChange = clampedScreenPos - borderedScreenPos;
        borderChange.x = FloatExtension.SignWithZero(borderChange.x);
        borderChange.y = FloatExtension.SignWithZero(borderChange.y);

        // Debug.Log("Border change " + borderChange);

        float targetRotation;

        if (borderChange != Vector2.zero) {
            targetRotation = Mathf.Atan2(borderChange.y, borderChange.x) * Mathf.Rad2Deg + 90;
        } else {
            targetRotation = 0;
        }

        float currentRotation = transform.eulerAngles.z;
        currentRotation = currentRotation % 360;

        currentRotation = FloatExtension.ChooseClosestTo(targetRotation, currentRotation, currentRotation+360);
        currentRotation = FloatExtension.ChooseClosestTo(targetRotation, currentRotation, currentRotation-360);

        float newRotation = Mathf.MoveTowards(currentRotation, targetRotation, 10* Time.deltaTime * Mathf.Abs(currentRotation - targetRotation));

        transform.rotation = Quaternion.Euler(0, 0, newRotation);

        switch (target.WaypointType) {
            case WaypointType.Station:
                if (QuestManager.IsStationGoal(target.Station)) {
                    rectTransform.localScale = new Vector2(1.2f, 1.2f);
                    pointer.color = Color.white;
                    pointer.enabled = true;
                } else if (QuestManager.IsStationRevealed(target.Station)) {
                    rectTransform.localScale = new Vector2(.5f, 1);
                    pointer.color = Color.cyan;
                    pointer.enabled = true;
                } else {
                    pointer.enabled = false;
                }
                break;
            case WaypointType.Enemy:
                rectTransform.localScale = new Vector2(.5f, .5f);
                pointer.color = Color.red;
                pointer.enabled = true;
                break;
            default:

                rectTransform.localScale = new Vector2(1, .5f);
                break;
        }

        // } else if (borderedScreenPos.x < clampedScreenPos.x) {
        //     transform.rotation = Quaternion.Euler(0, 0, 90);
        // } else if (borderedScreenPos.x > clampedScreenPos.x) {
        //     transform.rotation = Quaternion.Euler(0, 0, -90);
        // } else if (borderedScreenPos.y < clampedScreenPos.y) {
        //     transform.rotation = Quaternion.Euler(0, 0, 180);
        // } else {
        //     transform.rotation = Quaternion.Euler(0, 0, 0);
        // }

        rectTransform.position = clampedScreenPos;

    }
}
