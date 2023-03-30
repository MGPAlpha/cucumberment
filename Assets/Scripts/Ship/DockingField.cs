using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DockingField : MonoBehaviour
{

    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField] public string SceneName {get; private set;}

    private Collider _col;

    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider>();
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

    public void Dock() {
        SceneManager.LoadScene(SceneName);
    }
}
