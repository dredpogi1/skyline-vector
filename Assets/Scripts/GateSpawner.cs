using UnityEngine;

// Creates new gates off the right side of the screen.
// Gates are spaced by DISTANCE (not time), so they stay the same distance
// apart even when the game speeds up.
public class GateSpawner : MonoBehaviour
{
    public GameObject gatePrefab;
    public float gateSpacing = 8f;      // distance between two gates
    public float lowestCenterY = -1f;   // lowest height a gate can bob around
    public float highestCenterY = 2f;   // highest height a gate can bob around

    float distanceUntilNextGate = 0f;

    void Update()
    {
        if (GameManager.instance.isPlaying == false)
        {
            return;
        }

        // Count down the distance the world has scrolled.
        distanceUntilNextGate = distanceUntilNextGate - GameManager.instance.speed * Time.deltaTime;

        if (distanceUntilNextGate <= 0f)
        {
            SpawnGate();
            distanceUntilNextGate = gateSpacing;
        }
    }

    void SpawnGate()
    {
        // The gate appears where this spawner object sits, at a random height.
        float y = Random.Range(lowestCenterY, highestCenterY);
        Vector3 position = new Vector3(transform.position.x, y, 0f);
        Instantiate(gatePrefab, position, Quaternion.identity);
    }
}
