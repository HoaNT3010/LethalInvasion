using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] levelBackgrounds;
    [Header("Level Select Configuration")]
    [SerializeField] private float selectionDelayAmount = 0.5f;
    [SerializeField] private int nextSceneIndex = 2;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI levelName;

    private bool isDelayed = false;
    private float currentDelayAmount = 0.5f;
    private bool isLevelConfirm = false;
    private bool isLevelChanged = false;
    private int currentLevelNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial level
        currentLevelNumber = 1;
        ChangeLevel();
        isLevelConfirm = false;
    }

    // Update is called once per frame
    void Update()
    {
        DelayBetweenSelection();
        if (!isLevelConfirm)
        {
            HandleSelectionChange();
            LevelConfirm();
        }
    }

    private void DelayBetweenSelection()
    {
        if (isDelayed)
        {
            if (currentDelayAmount > 0.1f)
            {
                currentDelayAmount -= 1.0f * Time.deltaTime;
            }
            else
            {
                currentDelayAmount = selectionDelayAmount;
                isDelayed = false;
            }
        }
    }

    private void HandleSelectionChange()
    {
        // Move to next level (left to right)
        if (Input.GetAxis("Horizontal") > 0)
        {
            if(currentDelayAmount == selectionDelayAmount)
            {
                MoveToNextLevel();
            }
        }

        // Move to previous level (right to left)
        if (Input.GetAxis("Horizontal") < 0)
        {
            if (currentDelayAmount == selectionDelayAmount)
            {
                MoveToPreviousLevel();
            }
        }

        if(isLevelChanged)
        {
            ChangeLevel();
        }
    }

    private void MoveToNextLevel()
    {
        // If current level is not last level, increase by 1
        if (currentLevelNumber < levelBackgrounds.Length)
        {
            currentLevelNumber++;
        }
        // If current level is last level, reset to first level
        else
        {
            currentLevelNumber = 1;
        }
        isDelayed = true;
        isLevelChanged = true;
    }

    private void MoveToPreviousLevel()
    {
        // If current level is not first level, decrease by 1
        if (currentLevelNumber > 1)
        {
            currentLevelNumber--;
        }
        // If current level is first level, reset to last level
        else
        {
            currentLevelNumber = levelBackgrounds.Length;
        }
        isDelayed = true;
        isLevelChanged = true;
    }

    private void ChangeLevel()
    {
        for (int i = 0; i < levelBackgrounds.Length; i++)
        {
            int currentPosition = i + 1;
            if(currentPosition == currentLevelNumber)
            {
                levelBackgrounds[i].SetActive(true);
                ChangeLevelName(currentPosition);
            }
            else
            {
                levelBackgrounds[i].SetActive(false);
            }
        }
        isLevelChanged = false;
    }

    private void ChangeLevelName(int levelNumber)
    {
        levelName.text = "Level " + levelNumber.ToString();
    }

    private void LevelConfirm()
    {
        if (Input.GetButtonDown("Fire1") && !isLevelConfirm)
        {
            SaveScript.selectLevel = currentLevelNumber;

            isLevelConfirm = true;

            SwitchToNextScene();
        }
    }

    private void SwitchToNextScene()
    {
        StartCoroutine(ChangeSceneDelay());
        //SceneManager.LoadScene("Level1");
    }

    IEnumerator ChangeSceneDelay()
    {
        yield return new WaitForSeconds(selectionDelayAmount);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
