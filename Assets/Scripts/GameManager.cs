using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public sealed class GameManager : MonoBehaviour
{
    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;

    public Text inputScore;
    public TMP_InputField inputName;
    public TMP_InputField inputTG;

    public GameObject gameOverUI;
    public GameObject mainMenuUI;
    public Text scoreText;
    public Text livesText;

    public GameObject targetObject; // object to which skins are applied
    public Sprite[] skins; // ark skin list for NFT holders
    public Sprite noNFTskin; // skin sprite for non NFT players

    private SoundManager soundManager; // SoundManager
    private bool isGameMusicPlaying = false; // SoundManager EnterMenu Bug Fixer

    public int score { get; private set; }
    public int lives { get; private set; }

    bool ownNFT;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
    }

    private void Start() { // start function
        player.killed += OnPlayerKilled;
        mysteryShip.killed += OnMysteryShipKilled;
        invaders.killed += OnInvaderKilled;

        ownNFT = PlayerPrefs.GetInt("OwnNFT", 0) == 1; // getting ownNFT from previous scene
        soundManager = SoundManager.Instance; // SoundManager
        MainMenu();
    }

    private void MainMenu() { // main menu
        mainMenuUI.SetActive(true);
        gameOverUI.SetActive(false);

        invaders.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
    }

    private void Update() { // wait Return in menus
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            soundManager.playSoundPressEnter(); // SoundManager

            if (!isGameMusicPlaying) {
                soundManager.playGameMusic(); // SoundManager
                isGameMusicPlaying = true;
            }

            NewGame();
        }

        if (mainMenuUI.activeSelf && Input.GetKeyDown(KeyCode.Return)) {

            mainMenuUI.SetActive(false);
            soundManager.playSoundPressEnter(); // SoundManager

            if (!isGameMusicPlaying) {
                soundManager.playGameMusic(); // SoundManager
                isGameMusicPlaying = true;
            }

            NewGame();
        }
    }

    private void NewGame() {
        gameOverUI.SetActive(false);
        ApplyRandomSkin(); // apply random skin to the player

        SetScore(0);
        SetLives(3);
        NewRound();
        
    }

    private void NewRound() {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);
        Respawn();
    }

    private void Respawn() { // respawn sets player to start possition
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver() { // GAME OVER
    
        soundManager.stopGameMusic(); // SoundManager
        soundManager.playSoundGameOver(); // SoundManager
        soundManager.playMusicGameOver(); // SoundManager
        

        SubmitScore(); // send the finall score to the server
        gameOverUI.SetActive(true); // after death UI
        invaders.gameObject.SetActive(false); // delete invader from the scene
    }

    private void SetScore(int score) {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives) {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = "Lives: " + lives.ToString();
    }

    private void OnPlayerKilled() { // player was killed
        SetLives(lives - 1); // reduce lives by 1
        player.gameObject.SetActive(false); // delete player from scene

        if (lives > 0) { // if has lives
            Invoke(nameof(NewRound), 1f);
        } else { // if no lives
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader) { // invader was killed
        SetScore(score + invader.score); // add points to the score
        if (invaders.AmountKilled == invaders.TotalAmount) { // if all invaders was killed
            NewRound(); // start the new round
        }
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip) { // mystery ship was killed
        SetScore(score + mysteryShip.score); // add points to the score
    }

    public void ApplyRandomSkin() { // apply random skin from the list function
        if (ownNFT) { // pick random skin for NFT holders only
            int randomSkinIndex = Random.Range(0, skins.Length);
            if (targetObject != null && randomSkinIndex < skins.Length) {
                targetObject.GetComponent<SpriteRenderer>().sprite = skins[randomSkinIndex];
            }
        }
        else { // set the skin for non NFT holders
            targetObject.GetComponent<SpriteRenderer>().sprite = noNFTskin;
        }
    }

    public UnityEvent<string, int, string> submitScoreEvent;
    public void SubmitScore() {
        if (ownNFT) { // Check if 'ownNFT' is true
            submitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text), inputTG.text);
            print(inputTG);
        }
        else {
            print("Cannot submit score. You don't own the NFT.");
        }
    }

}