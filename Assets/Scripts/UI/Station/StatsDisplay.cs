using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI cargoText;
    [SerializeField] TextMeshProUGUI weightText;
    
    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = PlayerDataSingleton.Cargo.Money.ToString();
        cargoText.text = PlayerDataSingleton.Cargo.CurrentLoad.ToString();
        weightText.text = PlayerDataSingleton.Cargo.GetWeight().ToString();
    }
}
