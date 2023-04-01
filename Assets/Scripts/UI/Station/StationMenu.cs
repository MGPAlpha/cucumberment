using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Yarn.Unity;
using TMPro;

public class StationMenu : MonoBehaviour
{

    [SerializeField] private string currentStation;
    [SerializeField] private GameObject menuItemPrefab;
    [SerializeField] private GameObject menuOptionsPanel;
    [SerializeField] private DialogueRunner dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveStation() {
        DockingField.SetCurrentStation(currentStation, "", false);
        SaveSystem.SaveGame();
        SceneManager.LoadScene("Space");
    }

    private void CreateMenuItem(string text, UnityAction onClick, RectTransform parent) {
        GameObject newMenuItem = Instantiate(menuItemPrefab, Vector3.zero, Quaternion.identity, parent);
        Button newButton = newMenuItem.GetComponent<Button>();
        TextMeshProUGUI itemText = newMenuItem.GetComponentInChildren<TextMeshProUGUI>();
        itemText.text = text;
        newButton.onClick.AddListener(onClick);
    }
}
