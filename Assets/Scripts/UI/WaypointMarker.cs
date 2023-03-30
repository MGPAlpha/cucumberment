using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
    private PositionBroadcaster target;
    private RectTransform rectTransform;
    private RectTransform parent;

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
        Debug.Log("screen pos " + screenPos);
        Vector3[] parentRect = new Vector3[4];
        parent.GetWorldCorners(parentRect);

        Debug.Log(parent.name + " Rect: " + parentRect);

        screenPos.x = Mathf.Clamp(screenPos.x, parentRect[0].x, parentRect[2].x);
        screenPos.y = Mathf.Clamp(screenPos.y, parentRect[0].y, parentRect[2].y);

        rectTransform.position = screenPos;

    }
}
