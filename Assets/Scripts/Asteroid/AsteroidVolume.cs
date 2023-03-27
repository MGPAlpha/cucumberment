using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidVolume : MonoBehaviour
{
    [field: SerializeField] public Transform FieldCenter {get; private set;}
    [field: SerializeField] public float FieldSize {get; private set;} = 1000;
    [field: SerializeField] public int AsteroidCount {get; private set;} = 1000;
    [SerializeField] private List<GeneratedAsteroid> asteroidPrefabs;

    private HashSet<GeneratedAsteroid> outOfBoundsAsteroids = new HashSet<GeneratedAsteroid>();
    private Vector3 lastGenerationCenter;

    // Start is called before the first frame update
    void Start()
    {
        InitializeField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    private void LateUpdate()
    {
        print("Num out of bounds: " + outOfBoundsAsteroids.Count);
        if (outOfBoundsAsteroids.Count > 100) {
            RespawnOutOfBoundsAsteroids();
        }
    }

    private void RespawnOutOfBoundsAsteroids() {
        Vector3 newCenter = FieldCenter.position;
        Vector3 outOfBoundsDisp = newCenter - lastGenerationCenter;
        Vector3 absDisp = new Vector3(Mathf.Abs(outOfBoundsDisp.x), Mathf.Abs(outOfBoundsDisp.y), Mathf.Abs(outOfBoundsDisp.z));
        Vector3 impossibleAreaDim = new Vector3(FieldSize,FieldSize,FieldSize) * 2 - absDisp;
        Bounds sector1Bounds;
        Bounds sector2Bounds;
        Bounds sector3Bounds;
        GetRespawnBounds(out sector1Bounds, out sector2Bounds, out sector3Bounds);
        float s1Area = sector1Bounds.size.ComponentProduct();
        float s2Area = sector2Bounds.size.ComponentProduct();
        float s3Area = sector3Bounds.size.ComponentProduct();
        float s1Prob = s1Area / (s1Area + s2Area + s3Area);
        float s2Prob = s2Area / (s2Area + s3Area);
        foreach (GeneratedAsteroid ast in outOfBoundsAsteroids) {
            Bounds chosenSpawnBounds;
            if (Random.value < s1Prob) chosenSpawnBounds = sector1Bounds;
            else if (Random.value < s2Prob) chosenSpawnBounds = sector2Bounds;
            else chosenSpawnBounds = sector3Bounds;

            Vector3 spawnPoint = chosenSpawnBounds.RandomPoint();
            ast.transform.position = spawnPoint;
            ast.transform.rotation = Random.rotation;
            ast.Regenerate(this);
        }
        lastGenerationCenter = newCenter;
        outOfBoundsAsteroids.Clear();
    }

    void GetRespawnBounds(out Bounds sector1, out Bounds sector2, out Bounds sector3) {

        Vector3 newCenter = FieldCenter.position;
        Vector3 outOfBoundsDisp = newCenter - lastGenerationCenter;
        Vector3 absDisp = new Vector3(Mathf.Abs(outOfBoundsDisp.x), Mathf.Abs(outOfBoundsDisp.y), Mathf.Abs(outOfBoundsDisp.z));
        Vector3 impossibleAreaDim = new Vector3(FieldSize,FieldSize,FieldSize) * 2 - absDisp;
        sector1 = new Bounds(newCenter + new Vector3((FieldSize - absDisp.x/2) * Mathf.Sign(outOfBoundsDisp.x), 0, 0), new Vector3(absDisp.x, FieldSize*2, FieldSize*2));
        sector2 = new Bounds(newCenter + new Vector3(-outOfBoundsDisp.x/2, (FieldSize - absDisp.y/2) * Mathf.Sign(outOfBoundsDisp.y), 0), new Vector3(FieldSize*2-absDisp.x, absDisp.y, FieldSize*2));
        sector3 = new Bounds(newCenter + new Vector3(-outOfBoundsDisp.x/2, -outOfBoundsDisp.y/2, (FieldSize - absDisp.z/2) * Mathf.Sign(outOfBoundsDisp.z)), new Vector3(FieldSize*2-absDisp.x, FieldSize*2-absDisp.y, absDisp.z));
    }

    void InitializeField() {
        for (int i = 0; i < AsteroidCount; i++) {
            GeneratedAsteroid currPrefab = asteroidPrefabs[i % asteroidPrefabs.Count];

            Transform newAsteroid = Instantiate(currPrefab.transform, new Vector3(Random.Range(-FieldSize, FieldSize), Random.Range(-FieldSize, FieldSize), Random.Range(-FieldSize, FieldSize)) + FieldCenter.position, Random.rotation);
            newAsteroid.GetComponent<GeneratedAsteroid>().Regenerate(this);
        }
        lastGenerationCenter = FieldCenter.position;
    }

    public void MarkOutOfBounds(GeneratedAsteroid ast) {
        outOfBoundsAsteroids.Add(ast);
    }

    public void MarkInBounds(GeneratedAsteroid ast) {
        outOfBoundsAsteroids.Remove(ast);
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(FieldCenter.position, new Vector3(FieldSize, FieldSize, FieldSize)*2);
        GetRespawnBounds(out Bounds s1, out Bounds s2, out Bounds s3);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(s1.center, s1.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(s2.center, s2.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(s3.center, s3.size);
    }
}
