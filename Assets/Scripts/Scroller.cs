using UnityEngine;

// Makes a wide, repeating picture (the sky or the ground) scroll left forever.
// When it has moved exactly one picture-width, it jumps back to where it
// started. Because the picture repeats, the player never sees the jump.
public class Scroller : MonoBehaviour
{
    public float speedMultiplier = 1f;  // 1 = same speed as the gates, smaller = far away
    public float tileWidth = 10f;       // width of ONE copy of the picture

    void Update()
    {
        Vector3 position = transform.position;
        position.x = position.x - GameManager.instance.speed * speedMultiplier * Time.deltaTime;

        if (position.x <= -tileWidth)
        {
            position.x = position.x + tileWidth;
        }

        transform.position = position;
    }
}
