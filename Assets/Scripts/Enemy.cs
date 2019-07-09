using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject deathEffect;
    float intPosition;
    public float maxOffset;
    public float speed;
    int direction = -1;
    public Rigidbody2D rb;
    public bool canMove;
    float OffsetPositive;
    float OffsetNegative;

    // Start is called before the first frame update
    void Start()
    {
        float intPosition = transform.position.x;
        OffsetPositive = intPosition + maxOffset;
        OffsetNegative = intPosition - maxOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove) {

            if (transform.position.x >= OffsetPositive)
            {
                Debug.Log("Flipped");
                direction *= -1;
                Flip();
            }

            if (transform.position.x < OffsetNegative)
            {
                Debug.Log("Flipped");
                direction *= -1;
                Flip();
            }

            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
        }
    }

    public void Hurt() {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        Destroy(this.gameObject);
    }

    void Flip() {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
