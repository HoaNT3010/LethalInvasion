using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Configuration")]
    [SerializeField] private int nextSceneIndex = 1;
    [Header("Components")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject controlMenu;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private TextMeshProUGUI difficultyNotification;
    [Header("Option Configuration")]
    [SerializeField] private float optionNotificateDuration = 2.0f;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer soundEffectMixer;

    // Start is called before the first frame update
    void Start()
    {
        musicMixer.SetFloat("GameplayMusic", 0);
        soundEffectMixer.SetFloat("PlayerSFX", 0);
    }

    public void SelectOnePlayerMode()
    {
        SaveScript.isOnePlayerMode = true;
        LoadNextScene();
    }

    public void SelectTwoPlayerMode()
    {
        SaveScript.isOnePlayerMode = false;
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenControlMenu()
    {
        controlMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseControlMenu()
    {
        mainMenu.SetActive(true);
        controlMenu.SetActive(false);
    }

    public void OpenOptionMenu()
    {
        optionMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseOptionMenu()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void SetBeginnerDifficulty()
    {
        SaveScript.AIDifficultyRate = 2.0f;
        ShowDifficultyChangeNotification("Beginner");
    }

    public void SetMediumDifficulty()
    {
        SaveScript.AIDifficultyRate = 1.0f;
        ShowDifficultyChangeNotification("Medium");
    }

    public void SetExpertDifficulty()
    {
        SaveScript.AIDifficultyRate = 0.5f;
        ShowDifficultyChangeNotification("Expert");
    }

    private void ShowDifficultyChangeNotification(string difficultyName)
    {
        difficultyNotification.text = "AI's difficulty has been set to " + difficultyName + "!";
        difficultyNotification.gameObject.SetActive(true);
        StartCoroutine(NotificationDisplayDuration());
    }

    IEnumerator NotificationDisplayDuration()
    {
        yield return new WaitForSeconds(optionNotificateDuration);
        difficultyNotification.gameObject.SetActive(false);
    }

    public void OnMusicVolumeChange()
    {
        musicMixer.SetFloat("GameplayMusic", musicSlider.value);
    }

    public void OnSFXVolumeChange()
    {
        soundEffectMixer.SetFloat("PlayerSFX", soundEffectSlider.value);
    }
}
