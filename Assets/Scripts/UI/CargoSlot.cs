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
        Init(item, item.count);
    }

    public void Init(ItemQuantity item, int currentQuantity) {
        this.item = item;
        cargoIcon.sprite = item.item.itemIcon;
        cargoName.text = item.item.itemName;
        if (currentQuantity < item.count) {
            cargoQuantity.text = "x" + currentQuantity.ToString() + "/" + item.count.ToString();
        } else {
            cargoQuantity.text = "x" + item.count.ToString();
        }
    }
}
