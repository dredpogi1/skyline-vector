using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// The GameManager is the "boss" of the game.
// It keeps track of the score, the game speed, the screens (Start / Game Over)
// and plays most of the sounds. Other scripts ask it things like
// "are we playing?" and "how fast is the world moving?".
public class GameManager : MonoBehaviour
{
    // Lets any other script reach the GameManager by typing GameManager.instance
    public static GameManager instance;

    // True after the player presses Play Again, so we skip the Start screen.
    static bool skipStartScreen = false;

    [Header("Speed (how fast the world scrolls)")]
    public float startSpeed = 5f;
    public float speedPerGate = 0.2f;   // extra speed for every gate passed
    public float maxSpeed = 9f;         // reached at 20 gates, then the speed stays the same

    [Header("Scene objects")]
    public Transform jet;
    public GameObject startScreen;
    public GameObject gameOverScreen;
    public GameObject newBestLabel;

    [Header("Text")]
    public Text scoreText;
    public Text finalScoreText;
    public Text bestScoreText;

    [Header("Sounds")]
    public AudioSource engineSound;     // looping engine hum
    public AudioSource soundEffects;    // plays the short one-shot sounds
    public AudioClip buttonSound;
    public AudioClip gatePassSound;
    public AudioClip crashSound;
    public AudioClip gameOverSound;

    // These change while the game runs. Other scripts read them.
    [HideInInspector] public float speed;
    [HideInInspector] public bool isPlaying = false;

    int score = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        speed = startSpeed;
        scoreText.text = "";
        gameOverScreen.SetActive(false);

        if (skipStartScreen)
        {
            StartGame();
        }
    }

    void Update()
    {
        // The engine sounds higher as the game gets faster.
        // At the start speed the pitch is 1 (normal).
        engineSound.pitch = 0.5f + 0.5f * (speed / startSpeed);
    }

    // Called by the PLAY button on the Start screen.
    public void StartGame()
    {
        PlaySound(buttonSound);
        startScreen.SetActive(false);
        scoreText.text = "0";
        isPlaying = true;
    }

    // Called by a Gate when the jet flies past it.
    public void AddPoint()
    {
        score = score + 1;
        scoreText.text = score.ToString();
        PlaySound(gatePassSound);

        // Every gate makes the game a little faster, up to a limit.
        speed = Mathf.Min(speed + speedPerGate, maxSpeed);
    }

    // Called by the Jet when it hits a gate.
    public void GameOver()
    {
        isPlaying = false;
        speed = 0f;                 // the whole world stops moving
        engineSound.Stop();
        PlaySound(crashSound);

        // Wait one second so the player can see the crash, then show the screen.
        Invoke(nameof(ShowGameOverScreen), 1f);
    }

    void ShowGameOverScreen()
    {
        // The high score is saved on the computer with PlayerPrefs.
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bool isNewBest = score > bestScore;

        if (isNewBest)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        scoreText.text = "";
        finalScoreText.text = "SCORE  " + score;
        bestScoreText.text = "BEST  " + bestScore;
        newBestLabel.SetActive(isNewBest);
        gameOverScreen.SetActive(true);
        PlaySound(gameOverSound);
    }

    // Called by the PLAY AGAIN button on the Game Over screen.
    public void PlayAgain()
    {
        skipStartScreen = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Plays a short sound one time. Other scripts can use this too.
    public void PlaySound(AudioClip clip)
    {
        soundEffects.PlayOneShot(clip);
    }
}
