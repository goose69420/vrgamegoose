using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class coolerPickup : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;

    Throwable throwable;

    public bool attached;
    public bool attachedlast;

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
        throwable = GetComponent<Throwable>();
    }

    // Update is called once per frame
    void Update()
    {
        attached = throwable.attached;

        

        if (attached)
        {
            bool checkHand = (attachedHand != null && attachedHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand | SteamVR_Input_Sources.RightHand)) || Input.GetKeyDown(KeyCode.Mouse1);
            bool checkHandAuto = (attachedHand != null && attachedHand.grabPinchAction.GetState(SteamVR_Input_Sources.LeftHand | SteamVR_Input_Sources.RightHand)) || Input.GetKey(KeyCode.Mouse1);
            
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
        attachedlast = attached;
    }

    void checkHand()
    {
        if (leftHand.currentAttachedObject == gameObject) attachedHand = leftHand;
        if (rightHand.currentAttachedObject == gameObject) attachedHand = rightHand;
    }
}
