using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Rigidbody2D))]
public class TankController : MonoBehaviourPunCallbacks
{
    //Keycodes zur Steuerung
    [SerializeField] KeyCode mKeyForward = KeyCode.W;
    [SerializeField] KeyCode mKeyBackward = KeyCode.S;
    [SerializeField] KeyCode mKeyLeftTurn = KeyCode.A;
    [SerializeField] KeyCode mKeyRightTurn = KeyCode.D;
    [SerializeField] KeyCode mShoot = KeyCode.Mouse0;

    [SerializeField] float mTankDriveSpeedForward = 20f;
    [SerializeField] float mTankDriveSpeedBackward = 10f;
    [SerializeField] float mTankRotationSpeed = 90f;

    [SerializeField] Transform mProjectileSpawn = null;
    [SerializeField] GameObject mProjectile = null;

    [SerializeField] ParticleSystem mExhaust = null;
    ParticleSystem.EmissionModule mEmissionModule = default;
    [SerializeField] float mParticleCountIdle = 10;
    [SerializeField] float mParticleCountDrive = 50;

    [SerializeField] Image mHealthBar = null;

    Rigidbody2D mRigidbody = null;

    [SerializeField] float mRigInertia = 1.0f;

    MouseLook mMouseLook = null;
    [SerializeField] Text mPlayerNameText = null;

    bool mInMotion = false;

    PhotonView mPhotonView = null;

    GameMenuCanvas mGameMenuCanvas = null;

    bool mIsPaused = false;

    float mHealth = 100f;
    readonly float mMaxHealth = 100f;

    [SerializeField] Vector3 rot;

    [SerializeField] int mMaxBullets = 5;
    int mCurrentBullets = 5;

    float mReloadTime = 1f;
    float mTimer = 0.0f;

    void Start()
    {
        mGameMenuCanvas = 
            GameObject.FindGameObjectWithTag(Tags.CANVAS)
            .GetComponent<GameMenuCanvas>();

        mGameMenuCanvas.OnPauseChange += SetPausedState;

        mRigidbody = GetComponent<Rigidbody2D>();
        //Rigidbody Fix für Rotation
        mRigidbody.inertia = mRigInertia;

        mEmissionModule = mExhaust.emission;

        mPhotonView = transform.parent.GetComponent<PhotonView>();
        mMouseLook = GetComponent<MouseLook>();

        //Synchronisiert Namen aller Spieler
        GetComponent<PhotonView>().RPC("SetNameSync",
                                       RpcTarget.AllViaServer,
                                       GetComponent<PhotonView>().OwnerActorNr,
                                       GetComponent<PhotonView>().Owner.NickName);

        mHealthBar.fillAmount = mHealth / mMaxHealth;
    }

    void Update()
    {
        if (mPhotonView.IsMine && !mIsPaused)
        {
            mInMotion = false;

            if (Input.GetKey(mKeyForward))
            {
                mInMotion = true;
                GetComponent<PhotonView>()
                    .RPC("UpdateExhaustLevel", 
                    RpcTarget.Others, 
                    GetComponent<PhotonView>().OwnerActorNr, 
                    true);

                mRigidbody.AddForce(transform.up * 
                                    mTankDriveSpeedForward * 
                                    Time.deltaTime);
            }
            
            if (Input.GetKey(mKeyBackward))
            {
                mInMotion = true;
                GetComponent<PhotonView>()
                    .RPC("UpdateExhaustLevel", 
                    RpcTarget.Others, 
                    GetComponent<PhotonView>().OwnerActorNr, 
                    true);

                mRigidbody.AddForce(-transform.up * 
                                    mTankDriveSpeedBackward * 
                                    Time.deltaTime);
            }

            if (Input.GetKeyUp(mKeyForward) ||
                Input.GetKeyUp(mKeyBackward))
            {
                GetComponent<PhotonView>()
                    .RPC("UpdateExhaustLevel",
                    RpcTarget.Others,
                    GetComponent<PhotonView>().OwnerActorNr,
                    false);
            }

            if (Input.GetKey(mKeyLeftTurn))
            {
                mRigidbody.AddTorque(mTankRotationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(mKeyRightTurn))
            {
                mRigidbody.AddTorque(-mTankRotationSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(mShoot) && mCurrentBullets > 0)
            {
                mCurrentBullets--;
                mGameMenuCanvas.SetShotsLeftValue(mCurrentBullets);

                //RPC Call - Spawnen des Projektils bei allen anderen
                GetComponent<PhotonView>().RPC("Shoot", 
                    RpcTarget.Others, 
                    GetComponent<PhotonView>().OwnerActorNr,
                    mProjectileSpawn.position, 
                    mProjectileSpawn.rotation);
                    
                //Lokales Schießen
                Shoot(GetComponent<PhotonView>().OwnerActorNr, 
                mProjectileSpawn.position,
                mProjectileSpawn.rotation);
            }

            mTimer += Time.deltaTime;
            if (mTimer > mReloadTime)
            {
                mTimer = 0.0f;
                if (mCurrentBullets < mMaxBullets)
                {
                    mCurrentBullets++;
                    mGameMenuCanvas.SetShotsLeftValue(mCurrentBullets);
                }
            }

            mEmissionModule.rateOverTime = 
                mInMotion ? mParticleCountDrive : mParticleCountIdle;
        }
    }

    //RPC-Call für Update der Health-Bars bei neuem Spieler
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GetComponent<PhotonView>().RPC("UpdateHealthBar",
            RpcTarget.AllViaServer,
            GetComponent<PhotonView>().OwnerActorNr,
            mHealth);

        GetComponent<PhotonView>().RPC("SetNameSync",
                                       RpcTarget.AllViaServer,
                                       GetComponent<PhotonView>().OwnerActorNr,
                                       PhotonNetwork.NickName);
    }

    void SetPausedState(bool _paused)
    {
        mIsPaused = _paused;
        mMouseLook.SetPauseState(_paused);
    }

    public void ReceiveDamage(float _damage, string _by)
    {
        Debug.Log("Received by " + _by);
        if (!_by.Equals(PhotonNetwork.NickName)) { return; }

        mHealth -= _damage;

        GetComponent<PhotonView>().RPC("UpdateHealthBar", 
            RpcTarget.AllViaServer, 
            GetComponent<PhotonView>().OwnerActorNr, 
            mHealth);
        
        if (GetComponent<PhotonView>().IsMine)
        {
            if (mHealth <= 0)
            {
                mHealth = 0;
                mGameMenuCanvas.ShowDeathScreen();
                SetPausedState(true);
                PhotonNetwork.Destroy(transform.parent.gameObject);
            }
        }
    }
    
    //Namen synchronisieren
    [PunRPC]
    public void SetNameSync(int _actorNumber, string _name)
    {
        if (GetComponent<PhotonView>().OwnerActorNr == _actorNumber)
        {
            mPlayerNameText.text = _name;
        }
    }

    //Health-Bar aktualisieren
    [PunRPC]
    void UpdateHealthBar(int _actorNumber, float _health)
    {
        if (GetComponent<PhotonView>().OwnerActorNr == _actorNumber)
        {
            mHealth = _health;
            mHealthBar.fillAmount = (mHealth / mMaxHealth);
        }
    }

    //Auspuff aller Spieler synchronisieren
    [PunRPC]
    void UpdateExhaustLevel(int _actorNumber, bool _moving)
    {
        if (GetComponent<PhotonView>().OwnerActorNr == _actorNumber)
        {
            mEmissionModule.rateOverTime =
                _moving ? mParticleCountDrive : mParticleCountIdle;
        }
    }

    //Schießen eines Projektils
    [PunRPC]
    void Shoot(int _actorNumber, Vector3 _start, Quaternion _rotation)
    {
        Projectile bullet = 
            Instantiate(mProjectile, _start, _rotation)
            .GetComponent<Projectile>();
    }
}