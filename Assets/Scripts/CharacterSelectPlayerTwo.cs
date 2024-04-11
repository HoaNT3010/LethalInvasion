using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectPlayerTwo : MonoBehaviour
{
    [Header("Icon Grid Configuration")]
    [SerializeField] private int iconsPerRow = 3;
    [SerializeField] private int totalRows = 2;
    [SerializeField] private float selectionDelayAmount = 0.5f;
    [Header("Preview Characters")]
    [SerializeField] private GameObject ElyP2;
    [SerializeField] private GameObject EveP2;
    [SerializeField] private GameObject MariaP2;
    [SerializeField] private GameObject MorakP2;
    [SerializeField] private GameObject NinjaP2;
    [SerializeField] private GameObject SynthP2;
    [Header("Player Selection Indicators")]
    [SerializeField] private GameObject ElyIndicator;
    [SerializeField] private GameObject EveIndicator;
    [SerializeField] private GameObject MariaIndicator;
    [SerializeField] private GameObject MorakIndicator;
    [SerializeField] private GameObject NinjaIndicator;
    [SerializeField] private GameObject SynthIndicator;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI playerTwoName;
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
        if (Input.GetAxis("Horizontal Alternative") > 0)
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
        if (Input.GetAxis("Horizontal Alternative") < 0)
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
        if (Input.GetAxis("Vertical Alternative") < 0)
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
        if (Input.GetAxis("Vertical Alternative") > 0)
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
        ElyP2.SetActive(false);
        EveP2.SetActive(false);
        MariaP2.SetActive(false);
        MorakP2.SetActive(false);
        NinjaP2.SetActive(false);
        SynthP2.SetActive(false);

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
        playerTwoName.text = gameObjectName.Substring(0, gameObjectName.Length - 2);
    }

    private void SwitchChosenCharacterOn()
    {
        // Switch all characters and indicators off
        SwitchAllCharacterOff();
        // Turn on chosen character and indicator
        switch (currentIconNumber)
        {
            case 1:
                ElyP2.SetActive(true);
                ElyIndicator.SetActive(true);
                ChangeCharacterName(ElyP2);
                break;
            case 2:
                EveP2.SetActive(true);
                EveIndicator.SetActive(true);
                ChangeCharacterName(EveP2);
                break;
            case 3:
                MariaP2.SetActive(true);
                MariaIndicator.SetActive(true);
                ChangeCharacterName(MariaP2);
                break;
            case 4:
                MorakP2.SetActive(true);
                MorakIndicator.SetActive(true);
                ChangeCharacterName(MorakP2);
                break;
            case 5:
                NinjaP2.SetActive(true);
                NinjaIndicator.SetActive(true);
                ChangeCharacterName(NinjaP2);
                break;
            case 6:
                SynthP2.SetActive(true);
                SynthIndicator.SetActive(true);
                ChangeCharacterName(SynthP2);
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
                return ExtractCharacterName(ElyP2);
            case 2:
                return ExtractCharacterName(EveP2);
            case 3:
                return ExtractCharacterName(MariaP2);
            case 4:
                return ExtractCharacterName(MorakP2);
            case 5:
                return ExtractCharacterName(NinjaP2);
            case 6:
                return ExtractCharacterName(SynthP2);
            default:
                return string.Empty;
        }
    }

    private void CharacterConfirm()
    {
        if (Input.GetButtonDown("Fire1 Alternative") && !isCharacterConfirmed)
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
