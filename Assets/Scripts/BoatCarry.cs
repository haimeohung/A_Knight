using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class BoatCarry : MonoBehaviour
{
    private class LocalCollider : MonoBehaviour
    {
        public float velocity;

        private PlayerControler2D player;
        private Rigidbody2D rb;
        private Animator ani;
        private float time = float.NegativeInfinity;
        private void Start()
        {
            player = GameObject.FindObjectOfType<PlayerControler2D>();
            ani = gameObject.GetComponentInParent<Animator>();
            rb = gameObject.GetComponentInParent<Rigidbody2D>();

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8)
            {
                if (Time.time - time > 2f)
                {
                    ani.SetTrigger("DuckTrigger");
                    time = Time.time;
                }
                player.transform.SetParentWithoutChangeScale(transform.parent);
                rb.velocity = new Vector2(velocity, 0f);
                player.Carring = rb.velocity;
            }
            if (collision.gameObject.layer == 10)
            {
                player.transform.SetParentWithoutChangeScale(null);
                rb.velocity = new Vector2(0, 0f);
                player.Carring = rb.velocity;
                Destroy(this);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        { 
            player.transform.SetParentWithoutChangeScale(null);
            rb.velocity = new Vector2(0, 0f);
            player.Carring = rb.velocity;
        }
    }

    [SerializeField] private float velocity;
    private Animator ani;

    void Start()
    {
        transform.GetChild(0).gameObject.AddComponent<LocalCollider>();
        transform.GetChild(0).GetComponent<LocalCollider>().velocity = velocity;
        ani = gameObject.GetComponent<Animator>();
    }

    public void EndDuck()
    {
        ani.SetTrigger("EndDuck");
    }
}
