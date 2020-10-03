using UnityEngine;
using UnityEngine.UI;

public class ListElement : MonoBehaviour
{
    [SerializeField] Button mElementButton = null;
    [SerializeField] Text mGameInfo = null;
    Color mAvailable = Color.white;
    Color mNotAvailable = Color.red;

    public void ClickSelect()
    {
        //Übergeben des eigenen Index an die GameList
        transform
            .parent
            .parent
            .parent
            .GetComponent<GameList>()
            .HandleRoomJoinClick(transform.GetSiblingIndex());
    }

    //Button Name und Farbe setzen
    public void SetData(string _name, string _players, bool _maxPlayersReached)
    {
        ColorBlock buttonColorBlock = default;
        Color buttonColor = !_maxPlayersReached ? mAvailable : mNotAvailable;
        
        buttonColorBlock.normalColor = buttonColor;
        buttonColorBlock.selectedColor = buttonColor;
        buttonColorBlock.highlightedColor = buttonColor;
        buttonColorBlock.pressedColor = buttonColor;
        buttonColorBlock.colorMultiplier = 1.0f;

        mElementButton.colors = buttonColorBlock;
        mGameInfo.text = $"{_name}\n{_players} Spieler";
    }
}