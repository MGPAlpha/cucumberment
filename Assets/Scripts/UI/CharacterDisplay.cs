using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CharacterDisplay : MonoBehaviour
{
    public static CharacterDisplay Main {get; private set;}
    [SerializeField] private CharacterLibrary instanceCharacterLibrary;
    private static CharacterLibrary characterLibrary;
    
    private CanvasGroup _cg;
    [SerializeField] private Image char1Image;
    [SerializeField] private Image char2Image;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
        if (!characterLibrary) characterLibrary = instanceCharacterLibrary;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cg = GetComponent<CanvasGroup>();
        HideDisplay();
    }

    [YarnCommand("hideDisplay")]
    public void HideDisplay() {
        _cg.alpha = 0;
    }

    [YarnCommand("showSoloCharacter")]
    public void ShowSoloCharacter(string name) {
        ShowCharacterOne(name);
        HideCharacterTwo();
    }

    [YarnCommand("showTwoCharacters")]
    public void ShowTwoCharacters(string name1, string name2) {
        ShowCharacterOne(name1);
        ShowCharacterTwo(name2);
    }

    public void ShowCharacterOne(string name) {
        CharacterData character = characterLibrary.characters.Find(c => c.characterName == name);
        char1Image.sprite = character.icon;
        char1Image.enabled = true;
        _cg.alpha = 1;
    }

    public void ShowCharacterTwo(string name) {
        CharacterData character = characterLibrary.characters.Find(c => c.characterName == name);
        char2Image.sprite = character.icon;
        char2Image.enabled = true;
        _cg.alpha = 1;
    }

    public void HideCharacterOne() {
        char1Image.enabled = false;
    }

    public void HideCharacterTwo() {
        char2Image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
