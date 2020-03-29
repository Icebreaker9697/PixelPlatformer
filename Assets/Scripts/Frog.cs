using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy {
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 5f;

    [SerializeField] private float jumpHeight = 5f;

    private bool facingLeft = true;
    private LayerMask ground;

    protected override void Start() {
        base.Start();
        ground = LayerMask.GetMask("Ground");
    }

    private void Update() {
        if ((rb.velocity.y < .1f) && anim.GetBool("Jumping")) {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
        } else if (anim.GetBool("Falling") && (col.IsTouchingLayers(ground))) {
            anim.SetBool("Falling", false);
        }
    }

    //Called via an animation event
    private void Move() {
        if (facingLeft) {
            //test to see if we are beyond the leftCap
            if (transform.position.x > leftCap) {
                //make sure sprite is facing left
                if (transform.localScale.x != 1) {
                    transform.localScale = new Vector3(1, transform.localScale.y);
                }

                //test to see of frog is on the ground. If so, jump.
                if (col.IsTouchingLayers(ground)) {
                    print("We are on the ground");
                    JumpLeft();
                    anim.SetBool("Jumping", true);
                }
            } else {
                facingLeft = false;
            }
        } else {
            //test to see if we are beyond the leftCap
            if (transform.position.x < rightCap) {
                //make sure sprite is facing left
                if (transform.localScale.x != -1) {
                    transform.localScale = new Vector3(-1, transform.localScale.y);
                }

                //test to see of frog is on the ground. If so, jump.
                if (col.IsTouchingLayers(ground)) {
                    JumpRight();
                    anim.SetBool("Jumping", true);
                }
            } else {
                facingLeft = true;
            }
        }
    }

    private void JumpLeft() {
        rb.velocity = new Vector2(-jumpLength, jumpHeight);
    }

    private void JumpRight() {
        rb.velocity = new Vector2(jumpLength, jumpHeight);
    }
}
