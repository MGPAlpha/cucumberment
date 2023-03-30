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
        progressBar.fillAmount = PlayerShipController.Main.DockingProgress;
        if (PlayerShipController.Main.ActiveDockingField) {
            promptText.text = "Hold E to Dock at\n" + PlayerShipController.Main.ActiveDockingField.Name;
        } else {
            promptText.text = "";
        }
    }
}
