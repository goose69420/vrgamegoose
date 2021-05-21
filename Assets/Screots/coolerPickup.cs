using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine.UI;
using TMPro;
public class coolerPickup : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;
    public Hand NotVRHand;

    Transform Canvas;
    TextMeshProUGUI ammo1;
    TextMeshProUGUI ammo2;

    Throwable throwable;

    public bool attached;
    public bool attachedlast;

    public bool fullAuto;
    Hand attachedHand;
    public Transform bulletSpawnPosition;
    public GameObject bulletPrefab;
    public long ShootCoolDown;
    public long MagazineBullets;


    long timenow;
    long lastShot;

    public float rotationSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = FindObjectOfType<Canvas>().transform;
        ammo1 = Canvas.GetChild(0).GetComponent<TextMeshProUGUI>();
        ammo2 = Canvas.GetChild(1).GetComponent<TextMeshProUGUI>();
        throwable = GetComponent<Throwable>();
    }

    // Update is called once per frame
    void Update()
    {
        attached = throwable.attached;
        checkHand();



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



        if (attached)
        {

            if (attachedHand == leftHand)
            {
                ammo2.gameObject.SetActive(true);
                ammo2.text = "AmmoL: " + MagazineBullets;
            }
            else
            {
                ammo1.gameObject.SetActive(true);
                ammo1.text = "AmmoR: " + MagazineBullets;
            }
        }
        if (leftHand.currentAttachedObject == null)
        {
            ammo2.gameObject.SetActive(false);
        }

        if (rightHand.currentAttachedObject == null && NotVRHand.currentAttachedObject == null)
        {
            ammo1.gameObject.SetActive(false);
        }

        


    }

    void checkHand()
    {
        if (leftHand.currentAttachedObject == gameObject)
        {
            attachedHand = leftHand;
            return;
        }

        if (rightHand.currentAttachedObject == gameObject)
        {
            attachedHand = rightHand;
            return;
        }

    }
}
