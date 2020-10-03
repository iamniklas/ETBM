using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DisconnectPanel : MonoBehaviour
{
    [SerializeField] Text mDisconnectDetail = null;

    public void SetDisconnectDetail(string _text)
    {
        mDisconnectDetail.text = _text;
    }

    public void Reconnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        gameObject.SetActive(false);
    }
}
