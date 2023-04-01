using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHUD : MonoBehaviour
{
    public static ShipHUD Main {get; private set;}
    
    private Canvas canvas;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void EnableHUD() {
        canvas.enabled = true;
    }

    public void DisableHUD() {
        canvas.enabled = false;
    }

}
