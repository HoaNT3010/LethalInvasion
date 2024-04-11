using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image playerOneGreen;
    [SerializeField] private Image playerTwoGreen;
    [SerializeField] private Image playerOneRed;
    [SerializeField] private Image playerTwoRed;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float levelTime = 90;
    [SerializeField] private GameObject roundEndAnnouncer;
    [SerializeField] private Image p1Win1;
    [SerializeField] private Image p1Win2;
    [SerializeField] private Image p2Win1;
    [SerializeField] private Image p2Win2;

    private void Awake()
    {
        SaveScript.roundCounter++;
        if(SaveScript.roundCounter > 5)
        {
            SaveScript.roundCounter = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        SaveScript.isTimeOut = true;

        if(SaveScript.playerOneWins == 1)
        {
            p1Win1.gameObject.SetActive(true);
        }
        if (SaveScript.playerOneWins == 2)
        {
            p1Win1.gameObject.SetActive(true);
            p1Win2.gameObject.SetActive(true);
        }
        if (SaveScript.playerTwoWins == 1)
        {
            p2Win1.gameObject.SetActive(true);
        }
        if (SaveScript.playerTwoWins == 2)
        {
            p2Win1.gameObject.SetActive(true);
            p2Win2.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!SaveScript.isTimeOut)
        {
            // Round timer logic
            if (levelTime > 0)
            {
                levelTime -= 1 * Time.deltaTime;
            }
            if (levelTime <= 0.1f)
            {
                SaveScript.isTimeOut = true;
                roundEndAnnouncer.SetActive(true);
                roundEndAnnouncer.gameObject.GetComponent<RoundEndAnnouncer>().enabled = true;
            }
            timerText.text = Mathf.Round(levelTime).ToString();

            // Players health logic
            playerOneGreen.fillAmount = SaveScript.PlayerOneHealth;
            playerTwoGreen.fillAmount = SaveScript.PlayerTwoHealth;

            // Check timer for red health bars
            if (SaveScript.PlayerOneTimer > 0)
            {
                SaveScript.PlayerOneTimer -= 2.0f * Time.deltaTime;
            }
            if (SaveScript.PlayerTwoTimer > 0)
            {
                SaveScript.PlayerTwoTimer -= 2.0f * Time.deltaTime;
            }

            // Start reducing red health bars fill amount
            if (SaveScript.PlayerOneTimer <= 0)
            {
                if (playerOneRed.fillAmount > SaveScript.PlayerOneHealth)
                {
                    playerOneRed.fillAmount -= 0.002f;
                }
            }
            if (SaveScript.PlayerTwoTimer <= 0)
            {
                if (playerTwoRed.fillAmount > SaveScript.PlayerTwoHealth)
                {
                    playerTwoRed.fillAmount -= 0.002f;
                }
            }
        }
    }
}
