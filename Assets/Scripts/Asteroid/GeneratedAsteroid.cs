using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GeneratedAsteroid : MonoBehaviour
{

    private Rigidbody _rb;
    private Collider _col;
    private Renderer _re;

    private AsteroidVolume parentVolume;
    private Transform parentVolumeCenter;
    private float parentVolumeSize;
    private float parentVolumePhysicsMax;

    [SerializeField] private float minScale = 1;
    [SerializeField] private float maxScale = 1;

    private bool inBounds = true;
    private bool spawnBlocked = false;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _re = GetComponent<Renderer>();
    }

    private bool physicsActive = true;

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
            CheckPhysicsDistance(distToCenter);
        }
    }

    private void CheckPhysicsDistance(Vector3 distToCenter) {
        if (!spawnBlocked) {
            if (!_rb.IsSleeping() && distToCenter.magnitude > parentVolumePhysicsMax) {
                physicsActive = false;
                // _rb.isKinematic = true;
                _rb.Sleep();
            } else if (_rb.IsSleeping() && distToCenter.magnitude < parentVolumePhysicsMax) {
                physicsActive = true;
                // _rb.isKinematic = false;
                _rb.WakeUp();
            }
        }
    }

    public void Regenerate(AsteroidVolume parent) {
        parentVolume = parent;
        parentVolumeCenter = parentVolume.FieldCenter;
        parentVolumeSize = parentVolume.FieldSize;
        parentVolumePhysicsMax = parentVolume.MaxPhysicsDistance;
        transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        _rb.SetDensity(10);
        _rb.mass = _rb.mass;
        inBounds = true;
        spawnBlocked = AsteroidBlocker.TestAsteroidBlock(transform.position);
        _col.enabled = !spawnBlocked;
        _re.enabled = !spawnBlocked;
        _rb.isKinematic = spawnBlocked;
        Vector3 distToCenter = transform.position - parentVolumeCenter.position;
        CheckPhysicsDistance(distToCenter);
    }
}
