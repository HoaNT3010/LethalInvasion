using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEndAnnouncer : MonoBehaviour
{
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private float pauseTime = 1.0f;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip playerOneWinSound;
    [SerializeField] private AudioClip playerTwoWinSound;
    [SerializeField] private GameObject playerOneWinText;
    [SerializeField] private GameObject playerTwoWinText;
    [SerializeField] private int currentSceneIndex = 2;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveScript.isTimeOut = false;

        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        playerOneWinText.gameObject.SetActive(false);
        playerTwoWinText.gameObject.SetActive(false);
        StartCoroutine(EndSet());
    }

    IEnumerator EndSet()
    {
        yield return new WaitForSeconds(0.5f);
        if (SaveScript.PlayerOneHealth > SaveScript.PlayerTwoHealth)
        {
            if (SaveScript.isOnePlayerMode)
            {
                winText.gameObject.SetActive(true);
                audioSource.Play();
                SaveScript.playerOneWins++;
                // If player won enough rounds
                if(SaveScript.playerOneWins > 2)
                {
                    SaveScript.isPlayerOneWin = true;
                }
            }
            else
            {
                playerOneWinText.gameObject.SetActive(true);
                audioSource.clip = playerOneWinSound;
                audioSource.Play();
                SaveScript.playerOneWins++;
                // If player won enough rounds
                if (SaveScript.playerOneWins > 2)
                {
                    SaveScript.isPlayerOneWin = true;
                }
            }
        }
        else
        {
            if (SaveScript.isOnePlayerMode)
            {
                loseText.gameObject.SetActive(true);
                audioSource.clip = loseSound;
                audioSource.Play();
                SaveScript.playerTwoWins++;
                // If player won enough rounds
                if (SaveScript.playerTwoWins > 2)
                {
                    SaveScript.isPlayerTwoWin = true;
                }
            }
            else
            {
                playerTwoWinText.gameObject.SetActive(true);
                audioSource.clip = playerTwoWinSound;
                audioSource.Play();
                SaveScript.playerTwoWins++;
                // If player won enough rounds
                if (SaveScript.playerTwoWins > 2)
                {
                    SaveScript.isPlayerTwoWin = true;
                }
            }
        }

        yield return new WaitForSeconds(pauseTime);

        if(!SaveScript.isPlayerOneWin && !SaveScript.isPlayerTwoWin)
        {
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
