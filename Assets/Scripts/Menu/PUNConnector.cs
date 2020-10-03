using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PUNConnector : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField] Text mDebug = null;
    [SerializeField] GameList list = null;
    public List<RoomInfo> RoomInfo { get; private set; }
       = new List<RoomInfo>();

    [SerializeField] int mSendRate = 12;

    [SerializeField] GameObject mInitPanel = null;
    [SerializeField] GameObject mMainPanel = null;
    [SerializeField] GameObject mConnectingPanel = null;
    [SerializeField] GameObject mDisconnectedPanel = null;

    MainCanvasManager mMainCanvasManager = null;
    [SerializeField] HostPanel mHostPanel = null;

    int mMainSceneIndex = 0;
    bool mSceneIsActive = true;

    private void Start()
    {
        mMainSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("Connected? " + PhotonNetwork.IsConnected);
        mMainCanvasManager = GetComponent<MainCanvasManager>();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SendRate = mSendRate;
    }

    public void OnJoinRandomClick()
    {
        mDebug.text += "JoinClick \n";
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCreateClick()
    {
        mDebug.text += "CreateClick \n";
        mMainCanvasManager.SwitchMenu(0);
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
        base.OnConnected();
    }

    private void OnLevelWasLoaded(int level)
    {
        mSceneIsActive = (level == mMainSceneIndex);
    }

    public override void OnConnectedToMaster()
    {
        ErrorHandler.ErrorInGame = false;
        ErrorHandler.Message = string.Empty;

        PhotonNetwork.JoinLobby();
        mInitPanel.SetActive(false);
        mMainPanel.SetActive(true);
        mDebug.text += "My Nickname? : " + PhotonNetwork.NickName + "\n";
        base.OnConnectedToMaster();
    }

    public override void OnCreatedRoom()
    {
        mDebug.text += ("OnCreatedRoom  \n");
        mDebug.text += $"Level ID: {(int)mHostPanel.Level}";
        Debug.Log($"Level ID: {(int)mHostPanel.Level}");
        PhotonNetwork.LoadLevel((int)mHostPanel.Level);
        base.OnCreatedRoom();
    }

    public override void OnJoinedLobby()
    {
        mDebug.text += ("OnJoinedLobby  \n");
        base.OnJoinedLobby();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        mDebug.text += ("OnJoinRandomFailed  \n");
        mDebug.text += $"{returnCode}: {message}";
        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        mDebug.text += ("OnJoinRoomFailed  \n");
        mDebug.text += $"{returnCode}: {message}";
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnLeftLobby()
    {
        mDebug.text += ("OnLeftLobby \n");
        base.OnLeftLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        mConnectingPanel.SetActive(false);
        mDisconnectedPanel.SetActive(true);
        mDisconnectedPanel
            .GetComponent<DisconnectPanel>()
            .SetDisconnectDetail(cause.ToString());
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        mDebug.text += "OnRoomListUpdate \n";
        mDebug.text += $"Now there are {roomList.Count} rooms \n\n";
        list.Clear();
        RoomInfo.Clear();
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount > 0)
            {
                list.Add(roomList[i].Name + roomList[i]
                            .CustomProperties["masterclientnickname"],
                         roomList[i].PlayerCount, roomList[i].MaxPlayers);
                RoomInfo.Add(roomList[i]);
                mDebug.text += $"Room ID {i} Name: " + roomList[i].Name + "\n";
            }
        }
    }
}