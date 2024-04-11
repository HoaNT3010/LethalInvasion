using UnityEngine;

public class SaveScript : MonoBehaviour
{
    public static float PlayerOneHealth = 1.0f;
    public static float PlayerTwoHealth = 1.0f;
    public static float PlayerOneTimer = 2.0f;
    public static float PlayerTwoTimer = 2.0f;
    public static bool isTimeOut = false;
    public static bool isOnePlayerMode = false;
    public static int playerOneWins = 0;
    public static int playerTwoWins = 0;
    public static int roundCounter = 0;
    public static string PlayerOneSelectedCharacter;
    public static string PlayerTwoSelectedCharacter;
    public static bool isPlayerOneWin = false;
    public static bool isPlayerTwoWin = false;
    public static int selectLevel = 1;
    public static float AIDifficultyRate = 1.0f;
    public static bool isPlayerOneReacting = false;
    public static bool isPlayerTwoReacting = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerOneHealth = 1.0f;
        PlayerTwoHealth = 1.0f;
        isPlayerOneReacting = false;
        isPlayerTwoReacting = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
