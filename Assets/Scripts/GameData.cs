using System.Collections.Generic;

public static class GameData
{
    // Folder paths
    public static readonly string CharacterPrefabsPath = "Assets/Prefabs/Characters/";
    public static readonly string CharacterIconPath = "Assets/Arts/Characters/Icons/";
    public static readonly string VictoryImagePath = "Assets/Arts/Characters/Victory/";
    public static readonly string BackgroundMaterialPath = "Assets/Arts/Backgrounds/Levels/Materials/";
    public static readonly string SceneDirectoryPath = "Assets/Scenes/";

    // Characters' names
    public static readonly string ElyName = "Ely";
    public static readonly string EveName = "Eve";
    public static readonly string MariaName = "Maria";
    public static readonly string MorakName = "Morak";
    public static readonly string NinjaName = "Ninja";
    public static readonly string SynthName = "Synth";

    // Characters' victory quotes
    public static readonly string ElyVictoryText = "Your skills are no match for my determination!";
    public static readonly string EveVictoryText = "Haha! I emerge victorious once again!";
    public static readonly string MariaVictoryText = "I fight with honor and emerge triumphant!";
    public static readonly string MorakVictoryText = "I stand undefeated! Who's next to challenge me?";
    public static readonly string NinjaVictoryText = "You fought valiantly, but in the end, I am the ultimate champion.";
    public static readonly string SynthVictoryText = "You are no match for my cybernetic enhancements!";

    public static readonly Dictionary<string, string> CharacterVictoryQuotes = new Dictionary<string, string>() {
        { ElyName, ElyVictoryText },
        { EveName, EveVictoryText },
        { MariaName, MariaVictoryText },
        { MorakName, MorakVictoryText },
        { NinjaName, NinjaVictoryText },
        { SynthName, SynthVictoryText },
    };
}
