using UnityEngine;

// Shows a soft ring, like a water ripple, wherever the player clicks.
// This lets people watching a recording see if the click was on the
// top half or the bottom half of the screen.
// There is only ONE ripple object: each click moves it to the mouse
// and restarts its grow-and-fade animation.
public class ClickRipple : MonoBehaviour
{
    public float duration = 0.45f;      // how long one ripple lasts, in seconds
    public float startSize = 0.2f;      // size when the ripple begins
    public float endSize = 1.1f;        // size when the ripple has faded out
    public float startAlpha = 0.55f;    // how visible it is at first (0 = invisible, 1 = solid)

    SpriteRenderer ring;
    float age = 100f;                   // seconds since the last click (starts "finished")

    void Start()
    {
        ring = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Jump to the spot that was clicked and start a new ripple.
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0f;
            transform.position = clickPosition;
            age = 0f;
        }

        // progress goes from 0 (just clicked) to 1 (ripple finished).
        age = age + Time.deltaTime;
        float progress = Mathf.Clamp01(age / duration);

        // Grow bigger and fade away as the ripple gets older.
        float size = Mathf.Lerp(startSize, endSize, progress);
        transform.localScale = new Vector3(size, size, 1f);

        Color color = ring.color;
        color.a = Mathf.Lerp(startAlpha, 0f, progress);
        ring.color = color;
    }
}
