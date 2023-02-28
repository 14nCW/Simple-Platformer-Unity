using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D body;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider2D;
    private float wallJumpCooldown;
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


        //flip sprite 
        if (horizontalInput > 0.01f) {
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        //Set animator bool
        animator.SetBool("isRunning", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());


        //Wall jump
        if (wallJumpCooldown > 0.2f) {
            //move
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() & !isGrounded()) {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
                animator.SetTrigger("grounded");
                animator.SetBool("onWall", true);
            } else {
                body.gravityScale = 3;
                animator.SetBool("onWall", false);
            }


            //jump
            if (Input.GetKey("space")) {
                Jump();
            }
        } else {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump() {
        if (isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetTrigger("grounded");
        } else if (onWall() && !isGrounded()) {
            
            if (horizontalInput == 0) {
                body.transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            } else {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6); //Mathf.Sign <- jeœli to co w nawiasie jest <0 (lewo) to dostajemy -1 a jeœli <0 (prawo) to dostajemy 1 
            }
            wallJumpCooldown = 0;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if (collision.gameObject.tag == "Ground") {
    //    }
    //}

    private bool isGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }

    private bool onWall() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }
}
