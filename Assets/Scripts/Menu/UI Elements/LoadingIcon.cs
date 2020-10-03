using UnityEngine;

public class LoadingIcon : MonoBehaviour
{
    [SerializeField] float mSpeed = 360f;

    void Update()
    {
        transform.Rotate(-Vector3.forward * mSpeed * Time.deltaTime);
    }
}
