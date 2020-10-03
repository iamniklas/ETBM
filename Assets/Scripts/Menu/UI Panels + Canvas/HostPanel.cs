using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class HostPanel : MonoBehaviour
{
    [SerializeField] InputField mRoomName = null;
    [SerializeField] Slider mSlider = null;
    [SerializeField] Text mMaxPlayerSliderIndicator = null;
    [SerializeField] Dropdown mDropdown = null;
    [SerializeField] Image mLevelImage = null;
    [SerializeField] Text mLevelDescription = null;
    [SerializeField] int mMinPlayers = 2;
    [Space(10)]
    [SerializeField] RoomSets mRoomSets = null;
    
    public enum Levels { DESERT = 1, MEADOW, DIRT}

    public string RoomName { get; private set; } = "";
    public byte MaxPlayers { get; private set; } = 2;
    public Levels Level { get; private set; } = Levels.DESERT;

    public void OnRoomNameChanged()
    {
        RoomName = mRoomName.text;
    }

    public void OnMaxPlayerSliderValueChange()
    {
        //+ Mindestspieleranzahl
        MaxPlayers = (byte) (mSlider.value + mMinPlayers);
        mMaxPlayerSliderIndicator.text = MaxPlayers.ToString();
    }

    public void OnLevelSelectionChanged()
    {
        Level = (Levels) (mDropdown.value);
        mLevelImage.sprite = mRoomSets.LevelImages[(int)Level];
        mLevelDescription.text = mRoomSets.LevelDescripts[(int)Level];

        Level++;
    }

    public void OnHostButtonClick()
    {
        //Erstellung einer Hashtable mit eingetragenen Werten
        Hashtable roomProperties = new Hashtable
        {
            { NetProperties.LEVELNAME, Level.ToString() },
            { NetProperties.LEVELINDEX, (int)Level },
            { NetProperties.MASTERCLIENT, PhotonNetwork.NickName }
        };

        //Lobbyinfo String-Array
        //(In einer Lobby werden nur die Properties angezeigt, deren Key
        //auch in den CustomRoomPropertiesForLobby eingetragen sind)
        string[] lobbyInfo = new string[] {NetProperties.LEVELNAME,
                                           NetProperties.MASTERCLIENT,
                                           NetProperties.LEVELINDEX
        };

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = this.MaxPlayers,
            CustomRoomPropertiesForLobby = lobbyInfo,
            CustomRoomProperties = roomProperties
        };
        //Erzeugen eines Raums mit RoomOptions
        PhotonNetwork.CreateRoom(mRoomName.text, options);
    }
}
