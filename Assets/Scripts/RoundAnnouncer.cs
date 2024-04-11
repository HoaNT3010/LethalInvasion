using System.Collections;
using TMPro;
using UnityEngine;

public class RoundAnnouncer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject fightText;
    [SerializeField] private float pauseTime = 1.0f;
    [SerializeField] private AudioClip fightSound;
    [SerializeField] private AudioClip[] roundAnnouncementSounds;
    [SerializeField] private AudioSource backgroundMusicPlayer;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load appropriate round sound
        audioSource.clip = roundAnnouncementSounds[SaveScript.roundCounter - 1];
        // Set correct round text
        roundText.text = $"Round {SaveScript.roundCounter}";

        roundText.gameObject.SetActive(false);
        fightText.gameObject.SetActive(false);
        StartCoroutine(RoundSet());
    }

    IEnumerator RoundSet()
    {
        yield return new WaitForSeconds(0.5f);
        roundText.gameObject.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(pauseTime);
        roundText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        fightText.gameObject.SetActive(true);
        audioSource.clip = fightSound;
        audioSource.Play();
        yield return new WaitForSeconds(pauseTime);
        fightText.gameObject.SetActive(false);
        backgroundMusicPlayer.Play();
        SaveScript.isTimeOut = false;
        gameObject.SetActive(false);
    }
}
