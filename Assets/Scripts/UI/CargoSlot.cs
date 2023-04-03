using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CargoSlot : MonoBehaviour
{
    [SerializeField] private Image cargoIcon;
    [SerializeField] private TextMeshProUGUI cargoName;
    [SerializeField] private TextMeshProUGUI cargoQuantity;

    private ItemQuantity item;

    public void Init(ItemQuantity item) {
        this.item = item;
        cargoIcon.sprite = item.item.itemIcon;
        cargoName.text = item.item.itemName;
        cargoQuantity.text = "x" + item.count.ToString();
    }
}
