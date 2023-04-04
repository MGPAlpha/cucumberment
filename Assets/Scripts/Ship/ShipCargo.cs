using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCargo : MonoBehaviour, IWeightContributor
{

    [SerializeField] private ItemLibrary instanceItemLibrary;
    public static ItemLibrary ItemLibrary {get; private set;} 

    [SerializeField] public int Capacity {get; private set;} = 25;
    [SerializeField] public int Money {get; private set;} = 0;
    public int CurrentLoad { get {
        int quantity = 0;
        foreach (ItemData item in inventory.Keys) {
            quantity += inventory[item];
        }
        return quantity;
    }}

    public int OpenSlots { get => Capacity - CurrentLoad; }

    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (!ItemLibrary) ItemLibrary = instanceItemLibrary;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(ItemData item, int quantity) {
        inventory[item] = inventory.GetValueOrDefault(item,0) + quantity;
    }

    public void RemoveItem(ItemData item, int quantity) {
        inventory[item] = Mathf.Max(0, inventory.GetValueOrDefault(item, 0) - quantity);
    }

    public int GetItem(ItemData item) {
        return inventory.GetValueOrDefault(item, 0);
    }

    public void LoadCargoData(CargoData data) {
        Capacity = data.capacity;
        Money = data.money;
        inventory = new Dictionary<ItemData, int>();
        foreach (string itemName in data.inventory.Keys) {
            ItemData item = ItemLibrary.items.Find(i => i.itemName == itemName);
            if (item) {
                inventory[item] = data.inventory[itemName];
            }
        }
    }

    public void FillCargoData(CargoData data) {
        data.capacity = Capacity;
        data.money = Money;
        data.inventory = new Dictionary<string, int>();
        foreach (ItemData item in inventory.Keys) {
            data.inventory[item.itemName] = inventory[item];
        }
    }

    public float GetWeight() {
        float weight = 0;
        foreach (ItemData item in inventory.Keys) {
            weight += item.weight * inventory[item];
        }
        return weight;
    }

    public void Empty() {
        inventory.Clear();
    }


    public void AddMoney(int amount) {
        Money += amount;
    }

    public void SpendMoney(int amount) {
        Money -= amount;
    }
}
