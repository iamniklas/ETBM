using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class GameMenuCanvas : MonoBehaviour
{
    //Event für Pause
    //Pausieren von TankControls und MouseLook
    public event Action<bool> OnPauseChange = (m) => { };

    bool mIsPaused = false;

    [SerializeField] GameObject mPausePanel = null;
    [SerializeField] GameObject mDeathPanel = null;
    [SerializeField] GameObject mDisconnectPanel = null;
    [SerializeField] Text mShotsLeftText = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mIsPaused = !mIsPaused;
            //Event aufrufen
            OnPauseChange.Invoke(mIsPaused);
            mPausePanel.SetActive(mIsPaused);
        }
    }

    public void SetShotsLeftValue(int _shots)
    {
        mShotsLeftText.text = _shots.ToString();
    }

    public void ContinueGame()
    {
        mIsPaused = false;
        //Event aufrufen
        OnPauseChange.Invoke(mIsPaused);
        mPausePanel.SetActive(mIsPaused);
    }

    public void LeaveRoomDisconnected()
    {
        SceneManager.LoadScene(0);   
    }

    public void LeaveSession()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ShowDeathScreen()
    {
        mDeathPanel.SetActive(true);
    }

    public void ShowDisconnectPanel()
    {
        mDisconnectPanel.SetActive(true);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}