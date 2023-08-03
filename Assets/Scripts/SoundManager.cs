using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource randomGameMusic;
    private float originalVolume;

    [SerializeField] private AudioSource 
        SoundMM, 
        SoundSkip, 
        SoundPressEnter, 
        SoundGameOver, 
        SoundShoot1,
        SoundShoot2,
        MusicLoginMenu,
        MusicGameOver,
        SoundMysteryShip,
        SoundInputNameFailed;

    [SerializeField] private AudioSource[] GameMusicList;

    private void Awake() // this allows object not to disappear in the next scenes
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // to play the sound from another script, add these lines

    // private SoundManager soundManager; // SoundManager

    // in void Start()
    // soundManager = SoundManager.Instance; // SoundManager

    // and play like soundManager.playSoundSkip();

    public void playSoundMM() {
        SoundMM.Play();
    }

    public void playSoundSkip() {
        SoundSkip.Play();
    }

    public void playSoundPressEnter() {
        SoundPressEnter.Play();
    }

    public void playSoundGameOver() {
        SoundGameOver.Play();
    }

    public void playSoundShoot() {
        (Random.Range(0, 2) == 0 ? SoundShoot1 : SoundShoot2).Play();
    }

    public void playLoginManu() {
        MusicLoginMenu.Play();
    }

    public void playGameMusic() {
        if (MusicLoginMenu.isPlaying) {
            MusicLoginMenu.Stop();
        }

        // Select random music for the game from the list
        int randomIndex = Random.Range(0, GameMusicList.Length);
        randomGameMusic = GameMusicList[randomIndex];

        // Play the selected music for the game
        randomGameMusic.Play();
        // StartCoroutine(WaitAndPlayNextRandomMusic());
    }

    // private IEnumerator WaitAndPlayNextRandomMusic()
    // {
    //         while (randomGameMusic.isPlaying) {
    //             yield return null;
    //         }

    //         // The current music has finished playing, play the next random music
    //         playGameMusic();
    // }

    public void stopGameMusic() {
        print("да но нет");
        if (randomGameMusic.isPlaying) {
            print("даааа сто что");
            randomGameMusic.Stop();
        }
    }

    public void playMusicGameOver() {
        MusicGameOver.Play();
    }

    public void stopMusicGameOver() {
        MusicGameOver.Stop();
    }

    public void playSoundMysteryShip() {
        SoundMysteryShip.Play();
        StartCoroutine(FadeOutGameMusic());
    }

    public void stopSoundMysteryShip() {
        SoundMysteryShip.Stop();
    }

    private IEnumerator FadeOutGameMusic()
    {
        originalVolume = randomGameMusic.volume;
        float fadeDuration = 2.0f; // Duration of smooth volume change (in seconds)
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            randomGameMusic.volume = Mathf.Lerp(originalVolume, 0.0f, t);
            yield return null;
        }

        randomGameMusic.volume = 0.0f; // Set the volume to 0 after the coroutine is finished
    }

    public IEnumerator RestoreGameMusicVolume()
    {
        float fadeDuration = 2.0f; // Duration of smooth volume change (in seconds)
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            randomGameMusic.volume = Mathf.Lerp(0.0f, originalVolume, t);
            yield return null;
        }

        // Restore the original volume after the coroutine is finished
        randomGameMusic.volume = originalVolume; 
    }

    public void playSoundInputNameFailed() {
        SoundInputNameFailed.Play();
    }

}
