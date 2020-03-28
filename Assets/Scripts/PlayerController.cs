using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
  private Rigidbody2D rb;
  private Animator anim;
  private Collider2D col;

  [SerializeField] private LayerMask ground;

  [SerializeField] private Vector2 velocity;

  [SerializeField] private float speed = 5f;

  [SerializeField] private int sprintMultiplier = 2;
  [SerializeField] private float jump = 7f;

  [SerializeField] private int cherries = 0;

  [SerializeField] private Text cherryCount;

  private enum State { idle, running, jumping, falling, sprinting }
  private State state = State.idle;

  // Start is called before the first frame update
  void Start() {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    col = GetComponent<Collider2D>();
  }

  // Update is called once per frame
  void Update() {
    detectInput();
    velocity = rb.velocity;
    cherryCount.text = cherries.ToString();
  }

  private void detectInput() {
    float hDirection = Input.GetAxis("Horizontal");

    int speedMultiplier = 1;

    if (Input.GetKey(KeyCode.LeftShift)) {
      speedMultiplier = sprintMultiplier;
    }

    if (hDirection < 0) {
      rb.velocity = new Vector2(-1 * speed * speedMultiplier, rb.velocity.y);
      transform.localScale = new Vector2(-1, 1);
    } else if (hDirection > 0) {
      rb.velocity = new Vector2(speed * speedMultiplier, rb.velocity.y);
      transform.localScale = new Vector2(1, 1);
    }

    if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground)) {
      state = State.jumping;
      rb.velocity = new Vector2(rb.velocity.x, jump);
    }

    velocityState(speedMultiplier);
    anim.SetInteger("state", (int) state);
  }

  private void velocityState(int speedMultiplier) {
    //if we've just jumped
    if (state == State.jumping) {
      if (rb.velocity.y < .1f) {
        state = State.falling;
      }
    }
    //if we're falling but havent detected it yet
    else if (rb.velocity.y < -.1f) {
      state = State.falling;
    }
    //if we're falling
    else if (state == State.falling) {
      if (col.IsTouchingLayers(ground)) {
        state = State.idle;
      }
    }
    //if we're moving
    else if (Mathf.Abs(rb.velocity.x) > 2f) {
      state = State.running;
      if (speedMultiplier > 1f) {
        state = State.sprinting;
      } else if (speedMultiplier == 1f) {
        state = State.running;
      }
    } else {
      state = State.idle;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.tag == "Collectable") {
      cherries++;
      Destroy(collision.gameObject);
    }
  }

}
