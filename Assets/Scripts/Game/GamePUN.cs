using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GamePUN : MonoBehaviourPunCallbacks
{
    GameMenuCanvas mCanvas = null;

    private void Start()
    {
        mCanvas = GameObject.FindGameObjectWithTag(Tags.CANVAS)
            .GetComponent<GameMenuCanvas>();
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        //Fehler-Nachricht übergeben
        ErrorHandler.ErrorInGame = true;
        ErrorHandler.Message = cause.ToString();
    }
    
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
}