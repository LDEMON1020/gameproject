using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float turnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGround = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isgliding = false;

    public bool isGrounded = true;

    public int coinCount = 0;
    public int totalCoins = 5;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGround = true;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGround = false;
        }
    }


    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        coyoteTimeCounter = 0;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimeLeft = gliderMaxTime;

        coyoteTimeCounter = 0;


    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedState();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if (!isgliding)
            {
                EnableGlider();
                void EnableGlider()
                {
                    isgliding = true;
                    if (gliderObject != null)
                    {
                        gliderObject.SetActive(true);
                    }

                    rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
                }

                gliderTimeLeft -= Time.deltaTime;

                if (gliderTimeLeft <= 0)
                {
                    DisableGlider();
                    void DisableGlider()
                    {
                        isgliding = false;
                        if (gliderObject != null)
                        {
                            gliderObject.SetActive(false);
                        }

                        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    }
                }
            }
            else if (isgliding)
            {
                DisableGlider();
                void DisableGlider()
                {
                    isgliding = false;
                    if (gliderObject != null)
                    {
                        gliderObject.SetActive(false);
                    }

                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                }
            }
            if (isgliding)
            {
                ApplyGliderMovement(moveHorizontal, moveVertical);
                void ApplyGliderMovement(float horizontal, float vertical)
                {
                    Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, -gliderFallSpeed, vertical * gliderMoveSpeed);

                    rb.velocity = gliderVelocity;
                }
            }
            else
            {
                rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
            }

            if (isGrounded)
            {
                if (isgliding)
                {
                    DisableGlider();
                    void DisableGlider()
                    {
                        isgliding = false;
                        if (gliderObject != null)
                        {
                            gliderObject.SetActive(false);
                        }

                        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    }
                }

                gliderTimeLeft = gliderMaxTime;
            }
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                realGround = false;
                coyoteTimeCounter = 0;
            }

        }
       

        //void OnTriggerEnter(Collider other)
        {
           // if (other.CompareTag("Coin"))
            {
            //    coinCount++;
             //   Destroy(other.gameObject);
             //   Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
            }
          //  if (other.CompareTag("Door") && coinCount >= totalCoins)
            {

                Debug.Log("GameClear!");
            }
        }

        void UpdateGroundedState()
        {
            if (realGround)
            {
                coyoteTimeCounter = coyoteTime;
                isGrounded = true;
            }
            else
            {
                if (coyoteTimeCounter > 0)
                {
                    coyoteTimeCounter -= Time.deltaTime;
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
           // void EnableGlider()
            {
              //  isgliding = true;
              //  if (gliderObject != null)
                {
              //      gliderObject.SetActive(true);
                }

                rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
            }

           // void DisableGlider()
            {
              //  isgliding = false;
              //  if (gliderObject != null)
                {
                 //   gliderObject.SetActive(false);
                }

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }

           // void ApplyGliderMovement(float horizontal, float vertical)
            {
               // Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, -gliderFallSpeed, vertical * gliderMoveSpeed);

              //  rb.velocity = gliderVelocity;
            }
        }
    }
}
