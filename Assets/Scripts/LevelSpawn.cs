using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSpawn : MonoBehaviour
{
    [SerializeField] private Transform playerOneSpawnPoint;
    [SerializeField] private Transform playerTwoSpawnPoint;
    [SerializeField] private Image playerOneIcon;
    [SerializeField] private Image playerTwoIcon;
    [SerializeField] private TextMeshProUGUI playerOneName;
    [SerializeField] private TextMeshProUGUI playerTwoName;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Image victoryImage;
    [SerializeField] private Text victoryText;
    [SerializeField] private float victoryScreenDelay = 2f;
    [SerializeField] private GameObject Background;

    private GameObject playerOneCharacter;
    private GameObject playerTwoCharacter;

    // Start is called before the first frame update
    void Start()
    {
        victoryPanel.SetActive(false);

        // Load character prefabs
        SetPlayerCharacters();

        // Set character icons
        SetCharacterIcons();

        // Set character names
        SetCharacterNames();

        // Set background
        SetBackground();

        // Spawning character
        StartCoroutine(SpawnCharacters());
    }

    private void Update()
    {
        // When one player win the match
        if (SaveScript.isPlayerOneWin || SaveScript.isPlayerTwoWin)
        {
            ShowVictoryScreen();
        }
    }

    IEnumerator SpawnCharacters()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(playerOneCharacter, playerOneSpawnPoint.position, playerOneSpawnPoint.rotation);
        Instantiate(playerTwoCharacter, playerTwoSpawnPoint.position, playerTwoSpawnPoint.rotation);
    }

    private void SetPlayerCharacters()
    {
        // Get character prefabs' name
        string playerOneCharacterName = SaveScript.PlayerOneSelectedCharacter + "P1";
        string playerTwoCharacterName;
        if (SaveScript.isOnePlayerMode)
        {
            // Versus AI
            playerTwoCharacterName = SaveScript.PlayerTwoSelectedCharacter + "AI";
        }
        else
        {
            // Versus second player
            playerTwoCharacterName = SaveScript.PlayerTwoSelectedCharacter + "P2";
        }
        // Set character prefabs' file path
        string playerOneCharacterPath = GameData.CharacterPrefabsPath + SaveScript.PlayerOneSelectedCharacter + "/" + playerOneCharacterName + ".prefab";
        string playerTwoCharacterPath = GameData.CharacterPrefabsPath + SaveScript.PlayerTwoSelectedCharacter + "/" + playerTwoCharacterName + ".prefab";

        // Load prefabs to object
        playerOneCharacter = AssetDatabase.LoadAssetAtPath<GameObject>(playerOneCharacterPath);
        playerTwoCharacter = AssetDatabase.LoadAssetAtPath<GameObject>(playerTwoCharacterPath);
    }

    private void SetCharacterIcons()
    {
        // Get icons' path
        string playerOneIconPath = GameData.CharacterIconPath + SaveScript.PlayerOneSelectedCharacter + "Icon.png";
        string playerTwoIconPath = GameData.CharacterIconPath + SaveScript.PlayerTwoSelectedCharacter + "Icon.png";

        // Load icon
        playerOneIcon.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(playerOneIconPath);
        playerTwoIcon.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(playerTwoIconPath);
    }

    private void SetCharacterNames()
    {
        playerOneName.text = SaveScript.PlayerOneSelectedCharacter;
        playerTwoName.text = SaveScript.PlayerTwoSelectedCharacter;
    }

    public void ShowVictoryScreen()
    {
        if (!SaveScript.isPlayerOneWin && !SaveScript.isPlayerTwoWin)
        {
            return;
        }

        // Get victor's name
        string victorName = "";
        if (SaveScript.isPlayerOneWin)
        {
            victorName = SaveScript.PlayerOneSelectedCharacter;
        }
        else if (SaveScript.isPlayerTwoWin)
        {
            victorName = SaveScript.PlayerTwoSelectedCharacter;
        }
        // Change victory image
        string victorImagePath = GameData.VictoryImagePath + victorName + ".jpg";
        victoryImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(victorImagePath);

        // Set victory text
        victoryText.text = GameData.CharacterVictoryQuotes[victorName];

        // Show the victory screen with a delay beforehand
        StartCoroutine(SetVictoryScreenActive());
    }

    IEnumerator SetVictoryScreenActive()
    {
        yield return new WaitForSeconds(victoryScreenDelay);
        victoryPanel.SetActive(true);
    }

    public void ReturnToCharacterSelect()
    {
        SceneManager.LoadScene(1);
    }

    private void SetBackground()
    {
        // Get chosen material
        Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(GameData.BackgroundMaterialPath + "BG" + SaveScript.selectLevel.ToString() + ".mat");

        Background.GetComponent<Renderer>().material = newMaterial;
    }
}
