using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidVolume : MonoBehaviour
{
    [SerializeField] private Transform fieldCenter;
    [SerializeField] private float fieldSize = 1000;
    [SerializeField] private int asteroidCount = 1000;
    [SerializeField] private List<GeneratedAsteroid> asteroidPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        InitializeField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeField() {
        for (int i = 0; i < asteroidCount; i++) {
            GeneratedAsteroid currPrefab = asteroidPrefabs[i % asteroidPrefabs.Count];

            Transform newAsteroid = Instantiate(currPrefab.transform, new Vector3(Random.Range(-fieldSize, fieldSize), Random.Range(-fieldSize, fieldSize), Random.Range(-fieldSize, fieldSize)) + fieldCenter.position, Random.rotation);
            newAsteroid.GetComponent<GeneratedAsteroid>().Regenerate();
        }
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(fieldCenter.position, new Vector3(fieldSize, fieldSize, fieldSize)*2);
    }
}
