using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class GameList : MonoBehaviour
{
    [SerializeField] RectTransform mContent = null;
    [SerializeField] GameObject mElement = null;
    List<GameObject> elements = new List<GameObject>();
    [SerializeField] PUNConnector mPunConnector = null;
    [SerializeField] GameInfoPanel mGameInfoPanel = null;
    [SerializeField] MainCanvasManager mCanvasManager = null;
    [SerializeField] RoomSets mRoomSets = null;

    //Erzeugen eines neuen Listenelements
    public void Add(string _name, int _currentPlayers, int _maxPlayers)
    {
        GameObject tempElement = Instantiate(mElement, mContent);

        tempElement.GetComponent<ListElement>()
            .SetData(_name, $"{_currentPlayers} / {_maxPlayers}",
            _currentPlayers == _maxPlayers);
        elements.Add(tempElement);
    }

    //Alle Listenelemente löschen
    public void Clear()
    {
        for (int i = 0; i < mContent.childCount; i++)
        {
            Destroy(mContent.GetChild(i).gameObject);
        }
        mContent.rect.Set(0, 0, 0, 0);
    }

    //Übergeben der Informationen aus RoomInfo von PUN Connector
    //an GameInfoPanel 
    public void HandleRoomJoinClick(int _index)
    {
        RoomInfo roomInfo = mPunConnector.RoomInfo[_index];
        string levelname = 
            roomInfo.CustomProperties[NetProperties.LEVELNAME] as string;
        int levelindex = 
            Convert.ToInt32(
                roomInfo.CustomProperties[NetProperties.LEVELINDEX]) - 1;
        mGameInfoPanel.SetData(roomInfo.Name,
                               roomInfo
                                .CustomProperties[NetProperties.MASTERCLIENT] 
                                    as string,
                               $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}",
                               mRoomSets.LevelNames[levelindex],
                               mRoomSets.LevelDescripts[levelindex],
                               mRoomSets.LevelImages[levelindex]);

        mCanvasManager.SwitchMenu(1);
    }
}