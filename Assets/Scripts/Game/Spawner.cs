using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject mTank = null;
    [SerializeField] GameObject mSpawnHint = null;

    bool mSpawned = false;
    Vector3 mRaycastHitPoint;
    List<string> mNameOfObjects = new List<string>();
    string mNameOfLastElement = "";
    int mTotalObjectsHit;
    
    void Update()
    {
        Vector3 worldMousePosition = 
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Basiert auf: 
        //https://answers.unity.com/questions/634717/how-can-i-raycast-the-direction-my-2d-character-is.html

        //2D-Raycast nach "hinten"
        RaycastHit2D[] hit =
            Physics2D.RaycastAll(worldMousePosition,
                              Vector2.zero,
                              Mathf.Infinity);

        if (hit.Length > 0)
        {
            mRaycastHitPoint = hit[0].point;

            for (int i = 0; i < hit.Length; i++)
            {
                mNameOfObjects.Add(hit[i].transform.name);
                mTotalObjectsHit = mNameOfObjects.Count;
                mNameOfLastElement = mNameOfObjects[mTotalObjectsHit-1];
            }

            mNameOfObjects.Clear();

            if (Input.GetKeyDown(KeyCode.Mouse0) && !mSpawned)
            {
                //Wenn der Boden getroffen wird
                if (hit[0].collider.name == "Ground")
                {
                    GameObject tank = 
                        PhotonNetwork.Instantiate(mTank.name, 
                                                  hit[0].point, 
                                                  Quaternion.identity);
                    
                    Destroy(mSpawnHint);
                    mSpawned = true;
                }
            }
        }
    }
}
