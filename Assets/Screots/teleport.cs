using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* К  К   Т Т Т Т Т    О О О   
* К К        Т       О     О  
* К          Т       О     О  
* К К        Т       О     О  
* К  К       Т        О О О   прочитал тот умрет, ПРОСЬБА НЕ СМОТРЕТЬ КОД
*/
public class teleport : MonoBehaviour
{
    [SerializeField] bool needToTeleport;
    public GameObject Player;
    Rigidbody rb;
    
    Valve.VR.InteractionSystem.Throwable Throwable;
    bool picked;
    bool floating;
    bool wasGrounded;
    Vector3 landed;
    public long timeToFloat;
    [SerializeField] long DateNow;
    [SerializeField] long landedAt;

    public GameObject fire1;
    public GameObject fire2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Throwable = GetComponent<Valve.VR.InteractionSystem.Throwable>();
    }

    float numBiggerNum(float num, float lim, float lim2)
    {
        if (num < lim) return lim;
        if (num > lim2) return lim2;
        return num;
    }

    // Update is called once per frame
    void Update()
    {
        DateNow = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        picked = Throwable.attached;

        transform.position = new Vector3(numBiggerNum(transform.position.x, 0.25f, 999.75f), numBiggerNum(transform.position.y, 0.25f, 999.75f), numBiggerNum(transform.position.z, 0.25f, 999.75f));

        Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hit, 0.34710f);

        
        if (hit.collider != null && hit.collider.gameObject.CompareTag("teleportable"))
        {
            if (needToTeleport && !picked)
            {
                Player.transform.position = gameObject.transform.position - new Vector3(0, 0.5f, 0);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                needToTeleport = false;
            }

        } else
        {
            needToTeleport = true;
        }

        if(hit.collider == true && !wasGrounded)
        {
            landedAt = DateNow;
        }

        if (!floating && !picked)
        {
            fire1.SetActive(false);
            fire2.SetActive(false);
        }
        if(floating && !picked && (DateNow - landedAt >= timeToFloat))
        {
            fire1.SetActive(true);
            fire2.SetActive(true);
        }
        if(picked || !floating)
        {
            fire1.SetActive(false);
            fire2.SetActive(false);
        }

        if(floating)
        {
            rb.useGravity = false;
            rb.isKinematic = !false;



            if (!picked)
            {
                if ((DateNow - landedAt >= timeToFloat) && !picked)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1.5f - transform.position.y, 0), 0.3f * Time.deltaTime);
                }
            }
        } else
        {
            rb.useGravity = true;
            rb.isKinematic = !true;
        }

        if(hit.collider != null && !floating)
        {
            floating = true;
        }
        if(picked)
        {
            rb.velocity = Vector3.zero;
            floating = false;
        }

        wasGrounded = hit.collider != null;
        //Debug.Log(picked + " " + floating);
    }
}
