using UnityEngine;

public class MainCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject mGameInfoPanel = null;
    [SerializeField] GameObject mHostPanel = null;
    [SerializeField] GameObject mDisconnectPanel = null;

    private void Start()
    {
        //Abfangen eines Fehlers im Spiel
        if (ErrorHandler.ErrorInGame)
        {
            mDisconnectPanel.SetActive(true);
            mDisconnectPanel
                .GetComponent<DisconnectPanel>()
                .SetDisconnectDetail(ErrorHandler.Message);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void SwitchMenu(int _menu)
    {
        mHostPanel.SetActive(_menu == 0);
        mGameInfoPanel.SetActive(_menu == 1);
    }
}
