using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class FuelTerminal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentFuelText; 
    [SerializeField] private TextMeshProUGUI maxFuelText;
    [SerializeField] private TextMeshProUGUI weightText; 

    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI chosenMoneyText;
    [SerializeField] private TextMeshProUGUI addedFuelText;
    [SerializeField] private TextMeshProUGUI addedWeightText;
    
    [SerializeField] private int fuelPerCoin = 50;

    private int selectedMoney = 1;

    public void OpenFuelScreen() {
        selectedMoney = 1;
        UpdateFuelScreen();
        gameObject.SetActive(true);
    }

    public UnityEvent OnClose;

    public void CloseFuelScreen() {
        gameObject.SetActive(false);
        OnClose.Invoke();
    }

    int addedFuel;
    float addedWeight;
    float maxFuelBuy;
    int coinsForMaxFuel;
    int maxCoinSpend;

    private void UpdateFuelScreen() {
        FuelManager fuelManager = PlayerDataSingleton.FuelManager;
        currentFuelText.text = fuelManager.CurrentFuel.ToString("F0");
        maxFuelText.text = fuelManager.Capacity.ToString("F0");
        weightText.text = fuelManager.GetWeight().ToString("F1");
        costText.text = fuelPerCoin.ToString();
        chosenMoneyText.text = selectedMoney.ToString();
        addedFuel = fuelPerCoin * selectedMoney;
        addedWeight = addedFuel * fuelManager.Density;
        addedFuelText.text = addedFuel.ToString("D");
        addedWeightText.text = addedWeight.ToString("F1");


        maxFuelBuy = fuelManager.Capacity - fuelManager.CurrentFuel;
        print ("Max fuel buy " + maxFuelBuy);
        coinsForMaxFuel = (int)Mathf.Ceil(maxFuelBuy / fuelPerCoin);
        print("coins for max fuel " + coinsForMaxFuel);
        print("Available coins " + PlayerDataSingleton.Cargo.Money);
        maxCoinSpend = Mathf.Min(coinsForMaxFuel, PlayerDataSingleton.Cargo.Money);
        maxCoinSpend = Mathf.Max(1, maxCoinSpend);
        print("max coind spend" + maxCoinSpend);
    }

    public void SelectMaximumSpend() {
        
        selectedMoney = maxCoinSpend;
        UpdateFuelScreen();
    }

    public void SelectMinimumSpend() {
        selectedMoney = 1;
        UpdateFuelScreen();
    }

    public void IncreaseSpend() {
        selectedMoney++;
        selectedMoney = Mathf.Clamp(selectedMoney, 1, maxCoinSpend);
        UpdateFuelScreen();
    }

    public void DecreaseSpend() {
        selectedMoney--;
        selectedMoney = Mathf.Clamp(selectedMoney, 1, maxCoinSpend);
        UpdateFuelScreen();
    }

    public void PurchaseSelectedFuel() {
        if (selectedMoney > PlayerDataSingleton.Cargo.Money) return;
        if (PlayerDataSingleton.FuelManager.CurrentFuel + addedFuel - fuelPerCoin >= PlayerDataSingleton.FuelManager.Capacity) return;
        PlayerDataSingleton.Cargo.SpendMoney(selectedMoney);
        PlayerDataSingleton.FuelManager.AddFuel(addedFuel);
        selectedMoney = 1;
        UpdateFuelScreen();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
