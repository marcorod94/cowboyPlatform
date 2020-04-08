using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private const float FLOOR_LIMIT = -15;
    private Rigidbody2D rb;
    private Animator animator;
    public LayerMask maskFloor;
    public Transform testFloor;
    public float jumpForce = 7f;
    private bool isWalking;
    private bool isOnFloor = true;
    private float radio = 0.07f;
    private bool isSecondJump = false;
    private float speed = 3f;
    public float factor = 1f;
    private KeyCode currentKey;
    private bool isCurrentKeyPress = true;
    private Vector2 initialPosition;
    public Text txtScore;
    private int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.initialPosition = this.transform.position;
        this.txtScore.text = "Score: " + this.score.ToString();
    }

    private void FixedUpdate()
    {
        this.isOnFloor = Physics2D.OverlapCircle(this.testFloor.position, this.radio, this.maskFloor);
        this.animator.SetBool("isJumping", !this.isOnFloor);
        if (this.isOnFloor)
        {
            this.isSecondJump = false;
        }
        if (this.transform.position.y < FLOOR_LIMIT)
        {
            this.transform.position = this.initialPosition;
        }
    }

    private void OnGUI()
    {
        if (Event.current.isKey)
        {
            this.SetCurrentKey();
            this.HandleAction();
        }
        
    }

    private void SetCurrentKey()
    {
        this.currentKey = Event.current.keyCode;
        if (Event.current.type == EventType.KeyDown)
        {
            this.isCurrentKeyPress = true;
        }
        if (Event.current.type == EventType.KeyUp)
        {
            this.isCurrentKeyPress = false;
        }
    }

    private void HandleAction()
    {
        if (this.currentKey == KeyCode.W && this.isCurrentKeyPress)
        {
            this.Jump();
        }
        if (this.currentKey == KeyCode.D || this.currentKey == KeyCode.A)
        {
            if (this.isCurrentKeyPress)
            {
                this.StartWalking(this.currentKey);
            }
            else
            {
                this.StopWalking();
            }
        }
        if (this.currentKey == KeyCode.M)
        {
            if (this.isCurrentKeyPress)
            {
                this.factor = 5;
            }
            else
            {
                this.factor = 1;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {        
        if (isWalking)
        {
            this.rb.velocity = new Vector2(this.speed * this.factor, this.rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "move")
        {
            this.transform.parent = collision.transform;
        }
        if (collision.transform.name == "top")
        {
            Destroy(collision.transform.parent.gameObject);
        }
        if (collision.transform.name == "body")
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene("Loose");
        }
        if (collision.transform.tag == "gem")
        {
            Destroy(collision.transform.gameObject);
            this.score++;
            this.txtScore.text = "Score: " + this.score.ToString();
        }
        if (collision.transform.tag == "door")
        {
            SceneManager.LoadScene("Win");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "move")
        {
            this.transform.parent = null;
        }
    }

    private void Jump()
    {
        if (this.isOnFloor || !this.isSecondJump)
        {
            this.rb.velocity = new Vector2(this.rb.velocity.x, this.jumpForce);
            this.rb.AddForce(new Vector2(0, this.jumpForce));
            if (!this.isSecondJump || !this.isOnFloor)
            {
                this.isSecondJump = true;
            }
        }
    }
    private void StartWalking(KeyCode keyCode)
    {
        this.speed = 3;
        float yDirection = 0;
        this.factor = 1;
        if (keyCode == KeyCode.A)
        {
            this.speed = -3;
            yDirection = 180;
        }
        this.transform.localRotation = Quaternion.Euler(0, yDirection, 0);
        this.Walk(true);
    }

    private void StopWalking()
    {
        this.Walk(false);
    }

    private void Walk(bool isWalking)
    {
        this.animator.SetBool("isWalking", isWalking);
        this.isWalking = isWalking;
    }
}
