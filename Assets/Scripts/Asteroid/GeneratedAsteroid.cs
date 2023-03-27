using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GeneratedAsteroid : MonoBehaviour
{

    private Rigidbody _rb;
    private Collider _col;

    private AsteroidVolume parentVolume;
    private Transform parentVolumeCenter;
    private float parentVolumeSize;

    [SerializeField] private float minScale = 1;
    [SerializeField] private float maxScale = 1;

    private bool inBounds = true;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parentVolume) {
            Vector3 distToCenter = transform.position - parentVolumeCenter.position;
            bool currentlyInBounds = Mathf.Abs(distToCenter.x) < parentVolumeSize
                && Mathf.Abs(distToCenter.y) < parentVolumeSize
                && Mathf.Abs(distToCenter.z) < parentVolumeSize;
            if (currentlyInBounds != inBounds) {
                inBounds = currentlyInBounds;
                if (inBounds) parentVolume.MarkInBounds(this);
                else parentVolume.MarkOutOfBounds(this);
            }
        }
    }

    public void Regenerate(AsteroidVolume parent) {
        parentVolume = parent;
        parentVolumeCenter = parentVolume.FieldCenter;
        parentVolumeSize = parentVolume.FieldSize;
        transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        _rb.SetDensity(10);
        _rb.mass = _rb.mass;
        inBounds = true;
    }
}
