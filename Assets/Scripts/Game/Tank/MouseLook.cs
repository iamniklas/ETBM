using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Rotation des Schussrohrs zur Maus
//Basiert auf Lösung von diesem Thread:
//https://answers.unity.com/questions/760900/how-can-i-rotate-a-gameobject-around-z-axis-to-fac.html?_ga=2.62141035.671988987.1593800033-2042080064.1593800033
public class MouseLook : MonoBehaviour
{
    [SerializeField] Transform mPipe = null;
    [SerializeField] Vector3 mEulerAnglesAddition = new Vector3(0, 0, -90f);

    Vector2 mMouseWorldPosition = Vector2.zero;

    PhotonView mPhotonView = null;

    bool mIsPaused = false;

    private void Start()
    {
        mPhotonView = transform.parent.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (mPhotonView.IsMine && !mIsPaused)
        {
            //Immer zur Maus schauen - siehe Quelle
            mMouseWorldPosition = 
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            float AngleRad = Mathf.Atan2(mMouseWorldPosition.y - 
                                            mPipe.position.y, 
                                         mMouseWorldPosition.x - 
                                            mPipe.position.x);
            
            float AngleDeg = AngleRad * Mathf.Rad2Deg;

            mPipe.rotation = (Quaternion.Euler(mEulerAnglesAddition.x, 
                                               mEulerAnglesAddition.y, 
                                               AngleDeg + 
                                               mEulerAnglesAddition.z));
        }
    }

    public void SetPauseState(bool _paused)
    {
        mIsPaused = _paused;
    }
}
