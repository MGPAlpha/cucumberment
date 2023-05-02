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
    RectTransform char1Transform;
    Vector2 char1SelectedPos;
    Vector2 char1DeselectedPos;
    RectTransform char2Transform;
    Vector2 char2SelectedPos;
    Vector2 char2DeselectedPos;

    [SerializeField] private float selectedDisplacement;

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

        char1Transform = char1Image.GetComponent<RectTransform>();
        char1SelectedPos = char1Transform.anchoredPosition;
        char1DeselectedPos = char1SelectedPos - new Vector2(0, selectedDisplacement);

        char2Transform = char2Image.GetComponent<RectTransform>();
        char2SelectedPos = char2Transform.anchoredPosition;
        char2DeselectedPos = char2SelectedPos - new Vector2(0, selectedDisplacement);

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
        if (!character || !character.icon) {
            char1Image.enabled = false;
            return;
        }
        if (character) {
            char1 = character;
            char1Image.sprite = character.icon;
            char1Image.enabled = true;
            _cg.alpha = 1;
        }
    }

    public void ShowCharacterTwo(string name) {
        Debug.Log("Attempting to show char " + name);
        CharacterData character = characterLibrary.characters.Find(c => c.characterName == name);
        if (!character || !character.icon) {
            char2Image.enabled = false;
            return;
        }
        if (character) {
            char2 = character;
            char2Image.sprite = character.icon;
            char2Image.enabled = true;
            _cg.alpha = 1;
        }
    }

    public void HideCharacterOne() {
        char1Image.enabled = false;
    }

    public void HideCharacterTwo() {
        char2Image.enabled = false;
    }

    CharacterData char1;
    CharacterData char2;

    public void TryCharacterHighlight(string charName) {
        
        Debug.Log(charName);

        CharacterData selectedChar;
        RectTransform charTransform;
        RectTransform nonSelectedTransform;
        Image charImage;
        Image nonSelectedImage;
        Vector2 selectedPos;
        Vector2 deSelectedPos;

        print("got here with char name " + charName);
        
        if (char1?.characterName == charName) {
            selectedChar = char1;
            charTransform = char1Transform;
            nonSelectedTransform = char2Transform;
            charImage = char1Image;
            nonSelectedImage = char2Image;
            selectedPos = char1SelectedPos;
            deSelectedPos = char2DeselectedPos;
        } else if (char2?.characterName == charName) {
            selectedChar = char2;
            charTransform = char2Transform;
            nonSelectedTransform = char1Transform;
            charImage = char2Image;
            nonSelectedImage = char1Image;
            selectedPos = char2SelectedPos;
            deSelectedPos = char1DeselectedPos;
        } else {
            return;
        }
        
        charTransform.anchoredPosition = selectedPos;
        nonSelectedTransform.anchoredPosition = deSelectedPos;
        charImage.color = Color.white;
        nonSelectedImage.color = Color.gray;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
