using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Animation variables
    Animator anim;
    public bool moving = false;
    
    //Movement Variables
    Rigidbody2D rb; //create reference for rigidbody bc jump requires physics
    public float jumpForce; //the force that will be added to the vertical component of player's velocity
    public float speed;

    //Melee Attack Variables
    public bool isAttacking = false;
    public float attackDuration = 0.5f; // Adjust as needed
    public float attackCooldown = 1.0f; // Adjust as needed
    private float attackTimer = 0.0f;



    //Ground Check Variables
    public LayerMask groundLayer;//layer information
    public Transform groundCheck;// player position info
    public bool isGrounded;

    SpriteRenderer sprite;

    //Audio BGM onStartup

    public GameObject backgroundMusicObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        AudioSource audioSource = backgroundMusicObject.GetComponent<AudioSource>();

        // Check if AudioSource exists and play the music
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .5f, groundLayer);
        
        Vector3 newPosition = transform.position;
        Vector3 newScale = transform.localScale;
        float currentScale = Mathf.Abs(transform.localScale.x);

        if(Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= speed;
            newScale.x = -currentScale;
            moving = true;
        }

        if(Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += speed;
            newScale.x = currentScale;
            moving = true;
        }

        if((Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if(Input.GetKeyUp("a") || Input.GetKeyUp("d") || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moving = false;
        }

        //Attack Function
        if (Input.GetKeyDown("f") && !isAttacking){ 
            isAttacking = true;
            attackTimer = 0.0f;
            // Add animation or attack behavior here
            // For now, let's simulate an attack by logging a message
            Debug.Log("Melee Attack!");
            
        }

        if (isAttacking){
            attackTimer += Time.deltaTime;
            
            if (attackTimer >= attackDuration){
                isAttacking = false;
                }
            }
        if (!isAttacking && attackTimer >= attackCooldown){
            // Reset attack timer
            attackTimer = 0.0f;
            }

        anim.SetBool("isMoving", moving);
        transform.position = newPosition; 
        transform.localScale = newScale;
    }
     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("end"))
        {
            Debug.Log("hit");
            SceneManager.LoadScene(2); //access SceneManager class for LoadScene function
        }
    }
}