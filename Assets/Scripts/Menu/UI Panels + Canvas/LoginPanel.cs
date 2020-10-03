using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] GameObject mConnectingPanel = null;
    
    [SerializeField] InputField mInputFieldName = null;
    [SerializeField] Button mConnectButton = null;

    public void OnInputFieldNameChanged()
    {
        //Sicherstellen, dass Name nicht leer ist
        if (string.IsNullOrEmpty(mInputFieldName.text))
        {
            mConnectButton.interactable = false;
        }
        else
        {
            mConnectButton.interactable = true;
        }
    }

    public void OnConnectButtonClicked()
    {
        mConnectingPanel.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = mInputFieldName.text;
        gameObject.SetActive(false);
    }
}
