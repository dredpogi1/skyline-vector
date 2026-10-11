using UnityEngine;

// One gate = a rock on top and a rock on the bottom with an opening between.
// The gate slides left toward the jet and bobs up and down,
// so the player has to guess where the opening will be.
public class Gate : MonoBehaviour
{
    public float bobHeight = 1.5f;      // how far the gate moves up and down
    public float bobSpeed = 1.5f;       // how fast it bobs
    public float destroyAtX = -13f;     // off the left side of the screen

    float centerY;              // the height the gate bobs around
    float bobOffset;            // random, so every gate bobs differently
    bool hasGivenPoint = false;

    void Start()
    {
        centerY = transform.position.y;
        bobOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        if (GameManager.instance.isPlaying == false)
        {
            return;     // freeze in place after a crash
        }

        // Slide left with the world, and bob up and down in a smooth wave.
        Vector3 position = transform.position;
        position.x = position.x - GameManager.instance.speed * Time.deltaTime;
        position.y = centerY + Mathf.Sin(Time.time * bobSpeed + bobOffset) * bobHeight;
        transform.position = position;

        // Give one point as soon as the gate is behind the jet.
        if (hasGivenPoint == false && position.x < GameManager.instance.jet.position.x - 1f)
        {
            hasGivenPoint = true;
            GameManager.instance.AddPoint();
        }

        if (position.x < destroyAtX)
        {
            Destroy(gameObject);
        }
    }
}
