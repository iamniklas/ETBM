using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameInfoPanel : MonoBehaviour
{
    [SerializeField] Text mRoomNameText = null;
    string mRoomName;
    [SerializeField] Text mPlayersText = null;
    [SerializeField] Text mRoomHostText = null;
    [SerializeField] Text mLevelText = null;
    [SerializeField] Text mLevelDescriptionText = null; 
    [SerializeField] Image mLevelImageText = null;

    //Übergeben der Rauminformationen an das GameInfoPanel und
    //Eintragen der Informationen
    public void SetData(string _roomName,
                        string _roomHost,
                        string _players,
                        string _level,
                        string _leveldescript,
                        Sprite _image)
    {
        mRoomName = _roomName;
        mRoomNameText.text = mRoomName;
        mRoomHostText.text = _roomHost;
        mPlayersText.text = _players;
        mLevelText.text = _level;
        mLevelDescriptionText.text = _leveldescript;
        mLevelImageText.sprite = _image;
    }

    public void OnJoinButtonClick()
    {
        PhotonNetwork.JoinRoom(mRoomName);
    }
}
