using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;

    private AudioSource death;
    protected virtual void Start() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        death = GetComponent<AudioSource>();
    }

    public void JumpedOn() {
        anim.SetTrigger("Death");
        death.Play();
        rb.velocity = Vector2.zero;
    }

    //called via an animation event
    private void Die() {
        Destroy(this.gameObject);
    }

}
