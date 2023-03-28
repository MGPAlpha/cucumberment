using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAimDisplay : MonoBehaviour
{
    private RectTransform displayTransform;
    [SerializeField] private RectTransform cursor;

    // Start is called before the first frame update
    void Start()
    {
        displayTransform = (RectTransform)transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerShipController.Main) {
            Vector2 aim = PlayerShipController.Main.SmoothedTurning;

            // cursor.localPosition = new Vector3(aim.x, aim.y, 0);
            cursor.anchoredPosition = aim * displayTransform.rect.height/2;
            Debug.Log(aim);
        }
    }
}
