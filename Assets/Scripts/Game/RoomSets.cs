using UnityEngine;

class RoomSets : MonoBehaviour
{
    //Levelnamen
    public string[] LevelNames { get; } = {"Wüste",
                                           "Wald",
                                           "Schlamm" };
    //Levelbeschreibungen
    public string[] LevelDescripts { get; } = 
        {"Eine unbewohnte und unbebaute Wüste - Pures PVP",
        "Viele Bäume, also viel Schutz. Fliehe oder greife an.\n" +
        "So oder so - es wird episch.",
        "Ein fetter Kampf im Schlamm.\n" +
        "Es gibt Schutzzonen, aber auch eine große offene Fläche. " +
        "Also VORSICHT!"};

    //Levelbilder
    [SerializeField] Sprite[] mImages = null;
    
    public Sprite[] LevelImages
    {
        get { return mImages; }
    }
}