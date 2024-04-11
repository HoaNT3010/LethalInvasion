using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectAI : MonoBehaviour
{
    [Header("Icon Grid Configuration")]
    [SerializeField] private int iconsPerRow = 3;
    [SerializeField] private int totalRows = 2;
    [SerializeField] private float selectionDelayAmount = 0.5f;
    [Header("Preview Characters")]
    [SerializeField] private GameObject ElyAI;
    [SerializeField] private GameObject EveAI;
    [SerializeField] private GameObject MariaAI;
    [SerializeField] private GameObject MorakAI;
    [SerializeField] private GameObject NinjaAI;
    [SerializeField] private GameObject SynthAI;
    [Header("Player Selection Indicators")]
    [SerializeField] private GameObject ElyIndicator;
    [SerializeField] private GameObject EveIndicator;
    [SerializeField] private GameObject MariaIndicator;
    [SerializeField] private GameObject MorakIndicator;
    [SerializeField] private GameObject NinjaIndicator;
    [SerializeField] private GameObject SynthIndicator;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI playerAIName;
    [SerializeField] private int nextSceneIndex = 1;

    private AudioSource audioSource;
    private int currentIconNumber = 1;
    private int currentRowNumber = 1;
    private float delayBetweenSelect = 0.5f;
    private bool delayCountDown = false;
    private bool isChangeCharacter = false;
    private bool isCharacterConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Reset chosen character
        currentIconNumber = 1;
        SwitchChosenCharacterOn();

        // Reset parameters
        delayBetweenSelect = selectionDelayAmount;
        delayCountDown = false;
        isChangeCharacter = false;
        isCharacterConfirmed = false;
    }

    // Update is called once per frame
    void Update()
    {
        DelayBetweenSelection();
        if (!isCharacterConfirmed)
        {
            HandleSelectionMovement();
            if (isChangeCharacter)
            {
                SwitchChosenCharacterOn();
                isChangeCharacter = false;
            }
            CharacterConfirm();
        }
    }

    // Add delay between character selection
    private void DelayBetweenSelection()
    {
        if (delayCountDown)
        {
            if (delayBetweenSelect > 0.1f)
            {
                delayBetweenSelect -= 1.0f * Time.deltaTime;
            }
            else
            {
                delayBetweenSelect = selectionDelayAmount;
                delayCountDown = false;
            }
        }
    }

    private void HandleSelectionMovement()
    {
        // Move selection cursor to right
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (delayBetweenSelect == selectionDelayAmount)
            {
                if (currentIconNumber < iconsPerRow * currentRowNumber)
                {
                    currentIconNumber++;
                    isChangeCharacter = true;
                    delayCountDown = true;
                }
            }
        }

        // Move selection cursor to left
        if (Input.GetAxis("Horizontal") < 0)
        {
            if (delayBetweenSelect == selectionDelayAmount)
            {
                if (currentIconNumber > iconsPerRow * (currentRowNumber - 1) + 1)
                {
                    currentIconNumber--;
                    isChangeCharacter = true;
                    delayCountDown = true;
                }
            }
        }

        // Move selection cursor down
        if (Input.GetAxis("Vertical") < 0)
        {
            if (delayBetweenSelect == selectionDelayAmount)
            {
                if (currentRowNumber < totalRows)
                {
                    currentIconNumber += iconsPerRow;
                    currentRowNumber++;
                    isChangeCharacter = true;
                    delayCountDown = true;
                }
            }
        }

        // Move selection cursor up
        if (Input.GetAxis("Vertical") > 0)
        {
            if (delayBetweenSelect == selectionDelayAmount)
            {
                if (currentRowNumber > 1)
                {
                    currentIconNumber -= iconsPerRow;
                    currentRowNumber--;
                    isChangeCharacter = true;
                    delayCountDown = true;
                }
            }
        }
    }

    private void SwitchAllCharacterOff()
    {
        // Character models
        ElyAI.SetActive(false);
        EveAI.SetActive(false);
        MariaAI.SetActive(false);
        MorakAI.SetActive(false);
        NinjaAI.SetActive(false);
        SynthAI.SetActive(false);

        // Player indicators
        ElyIndicator.SetActive(false);
        EveIndicator.SetActive(false);
        MariaIndicator.SetActive(false);
        MorakIndicator.SetActive(false);
        NinjaIndicator.SetActive(false);
        SynthIndicator.SetActive(false);
    }

    private void ChangeCharacterName(GameObject activePreviewCharacter)
    {
        string gameObjectName = activePreviewCharacter.name;
        playerAIName.text = gameObjectName.Substring(0, gameObjectName.Length - 2);
    }

    private void SwitchChosenCharacterOn()
    {
        // Switch all characters and indicators off
        SwitchAllCharacterOff();
        // Turn on chosen character and indicator
        switch (currentIconNumber)
        {
            case 1:
                ElyAI.SetActive(true);
                ElyIndicator.SetActive(true);
                ChangeCharacterName(ElyAI);
                break;
            case 2:
                EveAI.SetActive(true);
                EveIndicator.SetActive(true);
                ChangeCharacterName(EveAI);
                break;
            case 3:
                MariaAI.SetActive(true);
                MariaIndicator.SetActive(true);
                ChangeCharacterName(MariaAI);
                break;
            case 4:
                MorakAI.SetActive(true);
                MorakIndicator.SetActive(true);
                ChangeCharacterName(MorakAI);
                break;
            case 5:
                NinjaAI.SetActive(true);
                NinjaIndicator.SetActive(true);
                ChangeCharacterName(NinjaAI);
                break;
            case 6:
                SynthAI.SetActive(true);
                SynthIndicator.SetActive(true);
                ChangeCharacterName(SynthAI);
                break;
            default:
                break;
        }
    }

    private string ExtractCharacterName(GameObject activePreviewCharacter)
    {
        string gameObjectName = activePreviewCharacter.name;
        return gameObjectName.Substring(0, gameObjectName.Length - 2);
    }

    private string GetActiveCharacterName()
    {
        switch (currentIconNumber)
        {
            case 1:
                return ExtractCharacterName(ElyAI);
            case 2:
                return ExtractCharacterName(EveAI);
            case 3:
                return ExtractCharacterName(MariaAI);
            case 4:
                return ExtractCharacterName(MorakAI);
            case 5:
                return ExtractCharacterName(NinjaAI);
            case 6:
                return ExtractCharacterName(SynthAI);
            default:
                return string.Empty;
        }
    }

    private void CharacterConfirm()
    {
        if (Input.GetButtonDown("Fire1") && !isCharacterConfirmed)
        {
            // Play confirmation sound
            audioSource.Play();
            // Lock and prevent character switch
            isCharacterConfirmed = true;

            // Assign chosen character to SaveScript
            SaveScript.PlayerTwoSelectedCharacter = GetActiveCharacterName();

            // Wait and load new scene
            StartCoroutine(ChangeScene());
        }
        // Can add feature to cancel character selection later
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(selectionDelayAmount);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
