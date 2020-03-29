using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Enemy {
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    private bool facingLeft = true;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        Move();
    }

    private void Move() {
        if (facingLeft) {
            //test to see if we are beyond the leftCap
            if (transform.position.x > leftCap) {
                //make sure sprite is facing left
                if (transform.localScale.x != 1) {
                    transform.localScale = new Vector3(1, transform.localScale.y);
                }

                MoveLeft();
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

                MoveRight();
            } else {
                facingLeft = true;
            }
        }
    }

    private void MoveLeft() {
        rb.velocity = new Vector2(-5, rb.velocity.y);
    }

    private void MoveRight() {
        rb.velocity = new Vector2(5, rb.velocity.y);
    }
}
