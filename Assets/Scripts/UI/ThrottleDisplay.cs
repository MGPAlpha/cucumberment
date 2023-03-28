using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleDisplay : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerShipController.Main) {
            float throttle = PlayerShipController.Main.Throttle;

            slider.value = throttle;
        }
    }
}
