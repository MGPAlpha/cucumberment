using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GeneratedAsteroid : MonoBehaviour
{

    private Rigidbody _rb;
    private Collider _col;

    [SerializeField] private float minScale = 1;
    [SerializeField] private float maxScale = 1;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Regenerate() {
        transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        _rb.SetDensity(10);
        _rb.mass = _rb.mass;
    }
}
