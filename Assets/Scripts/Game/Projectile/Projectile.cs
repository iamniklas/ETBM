using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float mBulletSpeed = 50f;
    [SerializeField] GameObject mExplosion = null;
    public string ProjectilePlayer { get; private set; }
    public float Damage { get; } = 10;

    //Abschuss des Projektils im ersten Frame mit Bullet-Speed
    void Start()
    {
        ProjectilePlayer = PhotonNetwork.NickName;
        GetComponent<Rigidbody2D>()
            .AddForce(transform.up * mBulletSpeed, 
                      ForceMode2D.Impulse);

        //Sicherheit, dass Projektil wirklich zerstört wird
        Destroy(gameObject, 5);
    }

    //Dem Objekt mit Tag "Player" Schaden geben
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.PLAYER))
        {
            collision.gameObject.GetComponent<TankController>()
                .ReceiveDamage(Damage, ProjectilePlayer);
        }

        //Explision spawnen
        Instantiate(mExplosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}