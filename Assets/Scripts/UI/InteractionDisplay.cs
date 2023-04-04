using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Image progressBar;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerShipController.Main.ActiveDockingField && PlayerShipController.Main.RescueProgress == 0) {
            // Show docking Interaction
            progressBar.fillAmount = PlayerShipController.Main.DockingProgress;
            promptText.text = "Hold E to Dock at\n" + PlayerShipController.Main.ActiveDockingField.Name;
        } else if (PlayerShipController.Main.RescueProgress > 0 || PlayerShipController.PlayerFuelManager.CurrentFuel <= 0) {
            // Show Rescue Interaction
            progressBar.fillAmount = PlayerShipController.Main.RescueProgress;
            promptText.text = "Hold R for rescue teleport\nAll cargo will be lost";
        } else {
            progressBar.fillAmount = 0;
            promptText.text = "";
        }
        
    }
}
