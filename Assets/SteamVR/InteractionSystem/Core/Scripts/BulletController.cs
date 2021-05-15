//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class BulletController : MonoBehaviour
    {
        public long speed;
        CharacterController controller;
        public GameObject parent;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 transform1 = transform.position;
            if (transform1.x < 0 || transform1.y < 0 || transform1.z < 0)
            {
                Destroy(gameObject);
            }
            if (transform1.x > 1000 || transform1.y > 1000 || transform1.z > 1000)
            {
                Destroy(gameObject);
            }

        }

        private void OnTriggerEnter(Collider other)
        {

            if ((parent != null && other.gameObject == parent)) return;
            if (other.CompareTag("Bullet")) return;

            

            Destroy(gameObject);
        }
    }
}