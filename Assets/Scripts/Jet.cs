using UnityEngine;

// Controls the player's jet.
// The jet does NOT fall. It flies in "lanes" (levels) and stays in its lane
// until the player clicks:
//   - click the TOP half of the screen    -> go up one lane
//   - click the BOTTOM half of the screen -> go down one lane
// The Up / Down arrow keys (or W / S) work too.
public class Jet : MonoBehaviour
{
    [Header("Lanes")]
    public float laneHeight = 0.75f;    // distance between two lanes (small = subtle steps)
    public float middleLaneY = 0.5f;    // height of the middle lane
    public int lanesAboveMiddle = 4;    // 4 above + 4 below + middle = 9 lanes

    [Header("Feel")]
    public float moveSpeed = 14f;       // how quickly the jet slides to a new lane
    public float tiltAmount = 20f;      // how much the nose tilts while moving

    [Header("Sound")]
    public AudioClip moveSound;

    // 0 is the middle lane, positive lanes are higher, negative lanes are lower.
    int lane = 0;

    void Update()
    {
        if (GameManager.instance.isPlaying == false)
        {
            return;     // no steering on the Start or Game Over screens
        }

        ReadInput();
        MoveToLane();
    }

    void ReadInput()
    {
        bool wantsUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        bool wantsDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

        // A mouse click (or a finger tap) counts as up or down
        // depending on which half of the screen it is in.
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.y > Screen.height / 2f)
            {
                wantsUp = true;
            }
            else
            {
                wantsDown = true;
            }
        }

        if (wantsUp && lane < lanesAboveMiddle)
        {
            lane = lane + 1;
            GameManager.instance.PlaySound(moveSound);
        }

        if (wantsDown && lane > -lanesAboveMiddle)
        {
            lane = lane - 1;
            GameManager.instance.PlaySound(moveSound);
        }
    }

    void MoveToLane()
    {
        // Slide smoothly toward the height of the current lane.
        float targetY = middleLaneY + lane * laneHeight;
        Vector3 position = transform.position;
        position.y = Mathf.MoveTowards(position.y, targetY, moveSpeed * Time.deltaTime);
        transform.position = position;

        // Tilt the nose up or down while the jet is still moving.
        float distanceLeft = targetY - position.y;
        transform.rotation = Quaternion.Euler(0f, 0f, distanceLeft * tiltAmount);
    }

    // Unity calls this when the jet touches a gate.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance.isPlaying == false)
        {
            return;     // already crashed
        }

        GameManager.instance.GameOver();

        // Let gravity take over so the jet spins and falls off the screen.
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Dynamic;
        body.angularVelocity = -300f;
    }
}
