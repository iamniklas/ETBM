using UnityEngine;

public class ParticleSystemDestroyer : MonoBehaviour
{
    private ParticleSystem ps;
    float lifeTime = 0.0f;

    //Partikelsystem nach der Zeit eines Durchlaufs zerstören
    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        lifeTime = ps.main.duration;
        Destroy(gameObject, lifeTime);
    }
}