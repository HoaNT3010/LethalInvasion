using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterSelectPlayerOne : MonoBehaviour
{
    [Header("Icon Grid Configuration")]
    [SerializeField] private int iconsPerRow = 3;
    [SerializeField] private int totalRows = 2;
    [SerializeField] private float selectionDelayAmount = 0.5f;
    [Header("Preview Characters")]
    [SerializeField] private GameObject ElyP1;
    [SerializeField] private GameObject EveP1;
    [SerializeField] private GameObject MariaP1;
    [SerializeField] private GameObject MorakP1;
    [SerializeField] private GameObject NinjaP1;
    [SerializeField] private GameObject SynthP1;
    [Header("Player Selection Indicators")]
    [SerializeField] private GameObject ElyIndicator;
    [SerializeField] private GameObject EveIndicator;
    [SerializeField] private GameObject MariaIndicator;
    [SerializeField] private GameObject MorakIndicator;
    [SerializeField] private GameObject NinjaIndicator;
    [SerializeField] private GameObject SynthIndicator;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI playerOneName;
    [SerializeField] private TextMeshProUGUI playerTwoIndicator;

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
        ResetSaveScript();
        // Change player two indicator
        ChangeIndicator();
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
        ElyP1.SetActive(false);
        EveP1.SetActive(false);
        MariaP1.SetActive(false);
        MorakP1.SetActive(false);
        NinjaP1.SetActive(false);
        SynthP1.SetActive(false);

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
        playerOneName.text = gameObjectName.Substring(0, gameObjectName.Length - 2);
    }

    private void SwitchChosenCharacterOn()
    {
        // Switch all characters and indicators off
        SwitchAllCharacterOff();
        // Turn on chosen character and indicator
        switch (currentIconNumber)
        {
            case 1:
                ElyP1.SetActive(true);
                ElyIndicator.SetActive(true);
                ChangeCharacterName(ElyP1);
                break;
            case 2:
                EveP1.SetActive(true);
                EveIndicator.SetActive(true);
                ChangeCharacterName(EveP1);
                break;
            case 3:
                MariaP1.SetActive(true);
                MariaIndicator.SetActive(true);
                ChangeCharacterName(MariaP1);
                break;
            case 4:
                MorakP1.SetActive(true);
                MorakIndicator.SetActive(true);
                ChangeCharacterName(MorakP1);
                break;
            case 5:
                NinjaP1.SetActive(true);
                NinjaIndicator.SetActive(true);
                ChangeCharacterName(NinjaP1);
                break;
            case 6:
                SynthP1.SetActive(true);
                SynthIndicator.SetActive(true);
                ChangeCharacterName(SynthP1);
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
                return ExtractCharacterName(ElyP1);
            case 2:
                return ExtractCharacterName(EveP1);
            case 3:
                return ExtractCharacterName(MariaP1);
            case 4:
                return ExtractCharacterName(MorakP1);
            case 5:
                return ExtractCharacterName(NinjaP1);
            case 6:
                return ExtractCharacterName(SynthP1);
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
            SaveScript.PlayerOneSelectedCharacter = GetActiveCharacterName();

            SwitchToNextPlayer();
        }
        // Can add feature to cancel character selection later
    }

    private void ChangeIndicator()
    {
        if(SaveScript.isOnePlayerMode)
        {
            // AI
            playerTwoIndicator.text = "Player AI";
        }
        else
        {
            // Player
            playerTwoIndicator.text = "Player Two";
        }
    }

    private void SwitchToNextPlayer()
    {
        // Versus AI
        if(SaveScript.isOnePlayerMode)
        {
            gameObject.GetComponent<CharacterSelectAI>().enabled = true;
            StartCoroutine(ScriptTurnOffDelay());
        }
        // Versus another player
        else
        {
            gameObject.GetComponent<CharacterSelectPlayerTwo>().enabled = true;
            StartCoroutine(ScriptTurnOffDelay());
        }
    }

    IEnumerator ScriptTurnOffDelay()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<CharacterSelectPlayerOne>().enabled = false;
    }

    private void ResetSaveScript()
    {
        SaveScript.isPlayerOneWin = false;
        SaveScript.isPlayerTwoWin = false;
        SaveScript.playerOneWins = 0;
        SaveScript.playerTwoWins = 0;
        SaveScript.roundCounter = 0;
    }
}
