using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Helfer zum Rotieren des Canvas vom Panzer
public class CanvasRotator : MonoBehaviour
{
    Transform mTank;
    
    void Start()
    {
        mTank = transform.parent;
    }
    
    void Update()
    {
        Vector3 rotation = mTank.rotation.eulerAngles;
        transform.eulerAngles = Vector3.zero;
    }
}