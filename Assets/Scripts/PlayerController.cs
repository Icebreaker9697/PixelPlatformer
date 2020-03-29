using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
  private Rigidbody2D rb;
  private Animator anim;
  private Collider2D col;

  private LayerMask ground;
  [SerializeField] private AudioSource footstep;
  [SerializeField] private AudioSource cherrySound;

  [SerializeField] private Vector2 velocity;

  [SerializeField] private float speed = 5f;

  [SerializeField] private int sprintMultiplier = 2;
  private float speedMultiplier = 1f;
  [SerializeField] private float jump = 7f;

  [SerializeField] private int cherries = 0;

  [SerializeField] private Text cherryCount;

  [SerializeField] private float hurtForce = 10f;

  private enum State { idle, running, jumping, falling, sprinting, hurt }
  private State state = State.idle;

  // Start is called before the first frame update
  void Start() {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    col = GetComponent<Collider2D>();
    ground = LayerMask.GetMask("Ground");
  }

  // Update is called once per frame
  void Update() {
    if (state != State.hurt) {
      detectInput();
    }
    velocity = rb.velocity;
    cherryCount.text = cherries.ToString();

    velocityState();
    anim.SetInteger("state", (int) state);

  }

  //called by walk & sprint animation events
  private void Footstep() {
    footstep.Play();
  }

  private void detectInput() {
    float hDirection = Input.GetAxis("Horizontal");

    setSpeedMultiplier();

    if (hDirection < 0) {
      rb.velocity = new Vector2(-1 * speed * speedMultiplier, rb.velocity.y);
      transform.localScale = new Vector2(-1, 1);
    } else if (hDirection > 0) {
      rb.velocity = new Vector2(speed * speedMultiplier, rb.velocity.y);
      transform.localScale = new Vector2(1, 1);
    }

    if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground)) {
      Jump();
    }
  }

  private void setSpeedMultiplier() {
    speedMultiplier = 1;

    if (Input.GetKey(KeyCode.LeftShift)) {
      speedMultiplier = sprintMultiplier;
    }
  }

  private void Jump() {
    state = State.jumping;
    rb.velocity = new Vector2(rb.velocity.x, jump);
  }

  private void velocityState() {
    //if we've just jumped
    if (state == State.jumping) {
      if (rb.velocity.y < .1f) {
        state = State.falling;
      }
    }
    //if we're falling but havent detected it yet
    else if (rb.velocity.y < -1f) {
      state = State.falling;
    }
    //if we're falling
    else if (state == State.falling) {
      if (col.IsTouchingLayers(ground)) {
        state = State.idle;
      }
    }
    //if we are being knocked back from being hurt
    else if (state == State.hurt) {
      if (Mathf.Abs(rb.velocity.x) < .1f) {
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
      cherrySound.Play();
      cherries++;
      Destroy(collision.gameObject);
    }
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "Enemy") {
      if (state == State.falling) {
        collision.gameObject.GetComponent<Enemy>().JumpedOn();
        Jump();
      } else {
        state = State.hurt;
        if (collision.gameObject.transform.position.x > transform.position.x) {
          //enemy is to my right, therefore I should be damaged and move left
          rb.velocity = new Vector2(-1 * hurtForce, rb.velocity.y);
        } else {
          //enemy is to my left, therefore I should be damaged and move right
          rb.velocity = new Vector2(hurtForce, rb.velocity.y);
        }
      }
    }
  }

}
