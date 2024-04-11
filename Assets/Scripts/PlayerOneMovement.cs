using System.Collections;
using UnityEngine;

public class PlayerOneMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float walkingSpeed = 0.01f;
    [SerializeField] private float jumpingSpeed = 0.02f;
    [SerializeField] private GameObject playerOne;
    [SerializeField] private AudioClip lightPunch;
    [SerializeField] private AudioClip heavyPunch;
    [SerializeField] private AudioClip lightKick;
    [SerializeField] private AudioClip heavyKick;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private GameObject restrictObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private Collider capsuleCollider;
    [SerializeField] private float crouchingTimeLimit = 5.0f;
    [SerializeField] private float crouchingResetDelay = 2.0f;

    private GameObject roundEndAnnouncer;
    private GameObject opponent;
    private bool isJumping = false;
    private AnimatorStateInfo playerOneLayerZero;
    private bool canWalkLeft = true;
    private bool canWalkRight = true;
    private Vector3 opponentPosition;
    private float moveSpeed;
    private float crouchingTimer = 0f;

    public static bool facingLeft = false;
    public static bool facingRight = true;
    public static bool walkingLeft = true;
    public static bool walkingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        opponent = GameObject.Find("Player Two");
        roundEndAnnouncer = GameObject.Find("Round End Announcer");
        roundEndAnnouncer.SetActive(false);

        // Reset player when new round begin
        facingLeft = false;
        facingRight = true;
        walkingLeft = true;
        walkingRight = true;
        StartCoroutine(FacingRight());
        moveSpeed = walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.isTimeOut)
        {
            animator.SetBool("Forward", false);
            animator.SetBool("Backward", false);

            // Get opponent position
            opponentPosition = opponent.transform.position;

            // Flip player to face opponent
            if (opponentPosition.x > playerOne.transform.position.x)
            {
                StartCoroutine(FacingLeft());
            }
            if (opponentPosition.x < playerOne.transform.position.x)
            {
                StartCoroutine(FacingRight());
            }
        }
        if (!SaveScript.isTimeOut)
        {
            // Check if knocked-out
            if (SaveScript.PlayerOneHealth <= 0)
            {
                animator.SetTrigger("KnockedOut");
                playerOne.GetComponent<PlayerOneActions>().enabled = false;
                StartCoroutine(KnockedOut());
                roundEndAnnouncer.gameObject.SetActive(true);
                roundEndAnnouncer.gameObject.GetComponent<RoundEndAnnouncer>().enabled = true;
            }
            if (SaveScript.PlayerTwoHealth <= 0)
            {
                animator.SetTrigger("Victory");
                playerOne.GetComponent<PlayerOneActions>().enabled = false;
                enabled = false;
                roundEndAnnouncer.gameObject.SetActive(true);
                roundEndAnnouncer.gameObject.GetComponent<RoundEndAnnouncer>().enabled = true;
            }

            // Listen to animator
            playerOneLayerZero = animator.GetCurrentAnimatorStateInfo(0);

            if (playerOneLayerZero.IsTag("React"))
            {
                SaveScript.isPlayerOneReacting = true;
            }
            else
            {
                SaveScript.isPlayerOneReacting = false;
            }

            // Bound player movement
            Vector3 screenBounds = Camera.main.WorldToScreenPoint(transform.position);

            if (screenBounds.x > Screen.width - 150)
            {
                canWalkRight = false;
            }
            if (screenBounds.x < 150)
            {
                canWalkLeft = false;
            }
            else if (screenBounds.x > 150 && screenBounds.x < Screen.width - 150)
            {
                canWalkRight = true;
                canWalkLeft = true;
            }

            // Get opponent position
            opponentPosition = opponent.transform.position;

            // Flip player to face opponent
            if (opponentPosition.x > playerOne.transform.position.x)
            {
                StartCoroutine(FacingLeft());
            }
            if (opponentPosition.x < playerOne.transform.position.x)
            {
                StartCoroutine(FacingRight());
            }

            // Horizontal (move left and right)
            if (playerOneLayerZero.IsTag("Motion"))
            {
                // Reset timescale after opponent is hit
                Time.timeScale = 1.0f;

                if (Input.GetAxis("Horizontal") > 0 && canWalkRight)
                {
                    if (walkingRight)
                    {
                        animator.SetBool("Forward", true);
                        transform.Translate(walkingSpeed, 0, 0);
                    }
                }
                if (Input.GetAxis("Horizontal") < 0 && canWalkLeft)
                {
                    if (walkingLeft)
                    {
                        animator.SetBool("Backward", true);
                        transform.Translate(-walkingSpeed, 0, 0);
                    }
                }
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                animator.SetBool("Forward", false);
                animator.SetBool("Backward", false);
            }

            // Vertical (jumping and crouching)
            if (Input.GetAxis("Vertical") > 0)
            {
                if (!isJumping)
                {
                    isJumping = true;
                    animator.SetTrigger("Jump");
                    StartCoroutine(JumpPause());
                }
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                if (crouchingTimer < crouchingTimeLimit)
                {
                    animator.SetBool("Crouch", true);
                    crouchingTimer += Time.deltaTime;
                }
                else
                {
                    animator.SetBool("Crouch", false);
                    StartCoroutine(ResetCrouchTimer());
                }
            }
            if (Input.GetAxis("Vertical") == 0)
            {
                animator.SetBool("Crouch", false);
                crouchingTimer = 0f;
            }

            // Resets character's restriction if no collision
            if (!restrictObject.gameObject.activeInHierarchy)
            {
                walkingLeft = true;
                walkingRight = true;
            }

            // Blocking logic
            if (playerOneLayerZero.IsTag("Block"))
            {
                GetComponent<Rigidbody>().isKinematic = true;
                boxCollider.enabled = false;
                capsuleCollider.enabled = false;
            }
            else if (playerOneLayerZero.IsTag("Motion"))
            {
                boxCollider.enabled = true;
                capsuleCollider.enabled = true;
                GetComponent<Rigidbody>().isKinematic = false;
            }

            // Disable box collider when crouching and sweeping
            if (playerOneLayerZero.IsTag("Crouching") || playerOneLayerZero.IsTag("Sweep"))
            {
                boxCollider.enabled = false;
            }

            // Check flip forward/back
            if (PlayerOneActions.FlyingJump)
            {
                walkingSpeed = jumpingSpeed;
            }
            else
            {
                walkingSpeed = moveSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!SaveScript.isPlayerOneReacting)
        {
            // Play reacting animation only if main colliders is enable
            // and not in react tag
            if ((boxCollider.enabled || capsuleCollider.enabled) && !playerOneLayerZero.IsTag("React"))
            {
                if (other.gameObject.CompareTag("FistLight"))
                {
                    animator.SetTrigger("HeadReact");
                    audioPlayer.clip = lightPunch;
                    audioPlayer.Play();
                }
                if (other.gameObject.CompareTag("FistHeavy"))
                {
                    animator.SetTrigger("BigReact");
                    audioPlayer.clip = heavyPunch;
                    audioPlayer.Play();
                }
                if (other.gameObject.CompareTag("KickHeavy"))
                {
                    animator.SetTrigger("BigReact");
                    audioPlayer.clip = heavyKick;
                    audioPlayer.Play();
                }
                if (other.gameObject.CompareTag("KickLight"))
                {
                    animator.SetTrigger("HeadReact");
                    audioPlayer.clip = lightKick;
                    audioPlayer.Play();
                }
            }
        }
    }

    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        isJumping = false;
    }

    IEnumerator FacingLeft()
    {
        if (facingLeft)
        {
            facingLeft = false;
            facingRight = true;
            yield return new WaitForSeconds(0.15f);
            playerOne.transform.Rotate(0, -180, 0);
            animator.SetLayerWeight(1, 0f);
            animator.SetLayerWeight(0, 1f);
        }
    }

    IEnumerator FacingRight()
    {
        if (facingRight)
        {
            facingRight = false;
            facingLeft = true;
            yield return new WaitForSeconds(0.15f);
            playerOne.transform.Rotate(0, 180, 0);
            animator.SetLayerWeight(1, 1f);
            animator.SetLayerWeight(0, 0f);
        }
    }

    IEnumerator KnockedOut()
    {
        yield return new WaitForSeconds(0.1f);
        enabled = false;
    }

    IEnumerator ResetCrouchTimer()
    {
        yield return new WaitForSeconds(crouchingResetDelay);
        crouchingTimer = 0f;
    }
}
