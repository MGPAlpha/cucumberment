using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DockingField : MonoBehaviour
{

    private static string dockedStationName;
    private static string dockedStationScene;
    private static bool inStation;

    private static bool dockingLoaded = false;

    [field: SerializeField] public StationData Station {get; private set;}

    [SerializeField] private Transform spawnPoint;

    private Collider _col;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (!dockingLoaded) {
            if (PlayerPrefs.HasKey("dockedStation")) {
                dockedStationName = PlayerPrefs.GetString("dockedStation");
                dockedStationScene = PlayerPrefs.GetString("dockedScene");
                inStation = 1 == PlayerPrefs.GetInt("inStation");
            }
            dockingLoaded = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider>();
        if (spawnPoint && dockedStationName == Station.displayName) {
            PlayerShipController.Main.transform.position = spawnPoint.transform.position;
            PlayerShipController.Main.transform.rotation = spawnPoint.transform.rotation;
            WorldMover mover = GameObject.FindObjectOfType<WorldMover>();
            if (mover) {
                mover.Recenter();
            }
            foreach(AsteroidVolume asteroidVolume in GameObject.FindObjectsOfType<AsteroidVolume>()) {
                asteroidVolume.RespawnAllAsteroids();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent.TryGetComponent<PlayerShipController>(out PlayerShipController controller)) {
            controller.RegisterDockingField(this);
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.parent.TryGetComponent<PlayerShipController>(out PlayerShipController controller)) {
            controller.RemoveDockingField(this);
        }
    }

    public static void SetCurrentStation(string name, string scene, bool interior) {
        dockedStationName = name;
        if (scene != "") dockedStationScene = scene;
        inStation = interior;
    }

    public static void SaveLocation() {
        PlayerPrefs.SetString("dockedStation", dockedStationName);
        PlayerPrefs.SetString("dockedScene", dockedStationScene);
        PlayerPrefs.SetInt("inStation", inStation ? 1 : 0);
    }

    public void Dock() {
        dockedStationName = Station.displayName;
        dockedStationScene = Station.sceneName;
        inStation = true;

        SceneManager.LoadScene(Station.sceneName);
        SaveSystem.SaveGame();
    }

    public static void Rescue() {
        inStation = true;
        SceneManager.LoadScene(dockedStationScene);
        SaveSystem.SaveGame();
    }
}
