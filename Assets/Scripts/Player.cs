using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D body;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider2D;
    private float horizontalInput;

    private Animator animator;

    // Start is called before the first frame update
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        horizontalInput = Input.GetAxis("Horizontal");
        //move
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);


        //jump
        if (Input.GetKey("space")) {
            Jump();
        } else {
            animator.SetTrigger("grounded");
        }


        //flip sprite 
        if (horizontalInput > 0.01f) {
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        //Set animator bool
        animator.SetBool("isRunning", horizontalInput != 0);
    }

    private void Jump() {
        if (isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetTrigger("grounded");
        }
    }

    private bool isGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }
}
