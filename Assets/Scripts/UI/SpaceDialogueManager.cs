using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SpaceDialogueManager : MonoBehaviour
{
    public static SpaceDialogueManager Main {get; private set;}
    
    private DialogueRunner runner;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        runner = GetComponent<DialogueRunner>();
    }

    [YarnCommand("waitRealtime")]
    public static IEnumerator YarnWaitRealTime(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        
    }

    public void BeginDialogue(string nodeName) {
        print("Running node " + nodeName);
        Cursor.lockState = CursorLockMode.None;
        runner.StartDialogue(nodeName);
        CockpitCamera.Main.EnterCockpit();
        ShipHUD.Main.DisableHUD();
        Time.timeScale = 0;
    }

    public void DialogueComplete() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        CockpitCamera.Main.ExitCockpit();
        ShipHUD.Main.EnableHUD();
    }
}