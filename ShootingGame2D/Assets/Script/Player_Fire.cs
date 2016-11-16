using UnityEngine;
using System.Collections;

public class Player_Fire : MonoBehaviour {
    /* Public Object */
    public int MissileMaximumPool = 20;             // Player's missile memory maximum pool
    public Transform RightMissileLocation;          // Player's missile fire location
    public Transform LeftMissileLocation;           // Player's missile fire location
    public AudioSource MissileSound;                // Player's missile fire sound
    public GameObject MissileObject;                // Player's missile object

    /* Private Object */
    private bool FireState;                         // for Control fire cycle;
    private bool DeadCheck;                         // Player Dead Check
    private MemoryPool MPool = new MemoryPool();    // for Player's missile memory pool
    private GameObject[] MissileLeft;               // for Player's missile
    private GameObject[] MissileRight;

    // When application quit, Memory clear
    void OnApplicationQuit ()
    {
        MPool.Dispose();
    }

	// Use this for initialization
	void Start () {
        FireState = true;
        DeadCheck = IsDead();

        // Create Missile
        MPool.Create(MissileObject, MissileMaximumPool);
        MissileLeft = new GameObject[MissileMaximumPool / 2];
        MissileRight = new GameObject[MissileMaximumPool / 2];

        // All missile array initialize
        for (int i = 0; i < MissileMaximumPool / 2; i++)
        {
            MissileLeft[i] = null;
            MissileRight[i] = null;
        }

        MissileSound.Stop();
    }
	
	// Update is called once per frame
	void Update () {
        DeadCheck = IsDead();
        if (!DeadCheck)
        {
            if (!MissileFire())
            {
                return;
            }
        }
    }

    bool MissileFire ()
    {
        if (FireState)
        {
            if (Input.GetButton("Fire1"))
            {
                // Fire Cycle Control
                Invoke("FireCycleControl", Player_Data.FireRate);

                // Missile enable from memory pool
                for(int i = 0; i < MissileMaximumPool / 2; i++)
                {
                    if (MissileLeft[i] == null && MissileRight[i] == null)
                    {
                        MissileLeft[i] = MPool.NewItem();
                        MissileRight[i] = MPool.NewItem();
                        MissileLeft[i].transform.position = LeftMissileLocation.transform.position;
                        MissileRight[i].transform.position = RightMissileLocation.transform.position;
                        break;
                    }
                }
                FireState = false;
                MissileSound.Play();
            }
        }

        // Missile return to memory pool
        for(int i = 0; i < MissileMaximumPool / 2; i++)
        {
            // When left missile is enabled
            if (MissileLeft[i])
            {
                // When missile hit something(The missile hits are Collider disabled)
                if (MissileLeft[i].GetComponent<Collider2D>().enabled == false)
                {
                    MissileLeft[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(MissileLeft[i]);
                    MissileLeft[i] = null;
                }
            }
            // When right missile is enabled
            if (MissileRight[i])
            {
                // When missile hit something(The missile hits are Collider disabled)
                if (MissileRight[i].GetComponent<Collider2D>().enabled == false)
                {
                    MissileRight[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(MissileRight[i]);
                    MissileRight[i] = null;
                }
            }
        }
        return true;
    }

    // Dead Check from Player Gameobject
    bool IsDead ()
    {
        return GameObject.Find("Aircraft Body").GetComponent<Player_Move>().Death;
    }

    // Fire Cycle Control
    private void FireCycleControl ()
    {
        FireState = true;
    }
}
