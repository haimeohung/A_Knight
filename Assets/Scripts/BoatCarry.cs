using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class BoatCarry : MonoBehaviour
{
    private class LocalCollider : MonoBehaviour
    {
        public float velocity;

        private Transform playerPos;
        private Rigidbody2D rb;
        private Animator ani;
        private float range = 0;
        private float timer = 0f;
        private void Start()
        {
            playerPos = GameObject.FindObjectOfType<PlayerControler2D>().transform;
            ani = gameObject.GetComponentInParent<Animator>();
            rb = gameObject.GetComponentInParent<Rigidbody2D>();

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.otherCollider.gameObject.layer == 8)
            {
                ani.SetTrigger("DuckTrigger");
                playerPos.SetParentWithoutChangeScale(transform.parent);
                rb.velocity = new Vector2(velocity, 0f);
            }
            if (collision.otherCollider.gameObject.layer == 10)
            {              
                rb.velocity = new Vector2(0f, 0f);
                Destroy(this);
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            playerPos.SetParentWithoutChangeScale(null);
            rb.velocity = new Vector2(0, 0f);
        }
    }

    [SerializeField] private float velocity;

    void Start()
    {
        transform.GetChild(0).gameObject.AddComponent<LocalCollider>();
        transform.GetChild(0).GetComponent<LocalCollider>().velocity = velocity;
    }

  
}
