using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class coolerPickup : MonoBehaviour
{
    public bool attached;

    public bool gun;
    public bool fullAuto;
    Hand attachedHand;
    public Transform bulletSpawnPosition;
    public GameObject bulletPrefab;
    public long ShootCoolDown;
    public long MagazineBullets;


    long timenow;
    long lastShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attached)
        {
            bool checkHand = attachedHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand | SteamVR_Input_Sources.RightHand) || Input.GetKeyDown(KeyCode.Mouse1);
            bool checkHandAuto = attachedHand.grabPinchAction.GetState(SteamVR_Input_Sources.LeftHand | SteamVR_Input_Sources.RightHand) || Input.GetKey(KeyCode.Mouse1);
            if ((fullAuto && checkHandAuto) || (!fullAuto && checkHand))
            {
                timenow = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();


                if (timenow - lastShot <= ShootCoolDown) return;
                if (MagazineBullets <= 0) return;

                lastShot = timenow;
                MagazineBullets--;
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation, null);
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bullet.GetComponent<BulletController>().speed, ForceMode.VelocityChange);
                bullet.GetComponent<BulletController>().parent = gameObject;
            }
        }
    }

    public void onAttach(Hand hand)
    {

    }
    public void onDetach()
    {

    }
}
