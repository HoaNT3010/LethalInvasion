using System.Collections;
using UnityEngine;

public class AIPlayerTwoMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float walkingSpeed = 0.01f;
    //[SerializeField] private float jumpingSpeed = 0.02f;
    [SerializeField] private GameObject playerTwo;

    [SerializeField] private AudioClip lightPunch;
    [SerializeField] private AudioClip heavyPunch;
    [SerializeField] private AudioClip lightKick;
    [SerializeField] private AudioClip heavyKick;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private GameObject restrictObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private Collider capsuleCollider;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float blockDuration = 2.0f;

    private GameObject opponent;
    //private bool isJumping = false;
    private AnimatorStateInfo playerTwoLayerZero;
    private bool canWalkLeft = true;
    private bool canWalkRight = true;
    private Vector3 opponentPosition;
    private float moveSpeed;
    private float opponentDistance;
    private bool canAIMove = true;
    private int defensiveMoveType = 0;
    private bool isBlocking = false;

    public static bool facingLeft = false;
    public static bool facingRight = true;
    public static bool walkingLeft = true;
    public static bool walkingRight = true;
    public static bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        opponent = GameObject.Find("Player One");

        // Reset player when new round begin
        facingLeft = false;
        facingRight = true;
        walkingLeft = true;
        walkingRight = true;
        isAttack = false;
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
        }
        if (!SaveScript.isTimeOut)
        {
            // Get distance from opponent
            opponentDistance = Vector3.Distance(opponent.transform.position, playerTwo.transform.position);

            // Check if knocked-out
            if (SaveScript.PlayerTwoHealth <= 0)
            {
                animator.SetTrigger("KnockedOut");
                playerTwo.GetComponent<AIPlayerTwoActions>().enabled = false;
                StartCoroutine(KnockedOut());
            }
            if (SaveScript.PlayerOneHealth <= 0)
            {
                animator.SetTrigger("Victory");
                playerTwo.GetComponent<AIPlayerTwoActions>().enabled = false;
                enabled = false;
            }

            // Listen to animator
            playerTwoLayerZero = animator.GetCurrentAnimatorStateInfo(0);

            if (playerTwoLayerZero.IsTag("React"))
            {
                SaveScript.isPlayerTwoReacting = true;
            }
            else
            {
                SaveScript.isPlayerTwoReacting = false;
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

            // Only move if character is not dazed
            if (!AIPlayerTwoActions.dazed)
            {
                // Flip player to face opponent
                if (opponentPosition.x > playerTwo.transform.position.x)
                {
                    StartCoroutine(FacingLeft());

                    // Start walking right
                    if (playerTwoLayerZero.IsTag("Motion"))
                    {
                        // Reset timescale after opponent is hit
                        Time.timeScale = 1.0f;

                        animator.SetBool("CanAttack", false);
                        if (opponentDistance > attackDistance && canWalkRight && canAIMove)
                        {
                            if (walkingRight)
                            {
                                animator.SetBool("Forward", true);
                                animator.SetBool("Backward", false);
                                isAttack = false;
                                transform.Translate(walkingSpeed, 0, 0);
                            }
                        }
                        else if (opponentDistance <= attackDistance && canWalkRight && canAIMove)
                        {
                            canAIMove = false;
                            animator.SetBool("Forward", false);
                            animator.SetBool("Backward", false);
                            animator.SetBool("CanAttack", true);
                            StartCoroutine(WalkingPause());
                        }
                    }
                }
                if (opponentPosition.x < playerTwo.transform.position.x)
                {
                    StartCoroutine(FacingRight());

                    // Start walking left
                    if (playerTwoLayerZero.IsTag("Motion"))
                    {
                        // Reset timescale after opponent is hit
                        Time.timeScale = 1.0f;

                        animator.SetBool("CanAttack", false);
                        if (opponentDistance > attackDistance && canWalkLeft && canAIMove)
                        {
                            if (walkingLeft)
                            {
                                animator.SetBool("Backward", true);
                                animator.SetBool("Forward", false);
                                isAttack = false;
                                transform.Translate(-walkingSpeed, 0, 0);
                            }
                        }
                        else if (opponentDistance <= attackDistance && canWalkLeft && canAIMove)
                        {
                            canAIMove = false;
                            animator.SetBool("Forward", false);
                            animator.SetBool("Backward", false);
                            animator.SetBool("CanAttack", true);
                            StartCoroutine(WalkingPause());
                        }
                    }
                }
            }

            ////Jumping
            //if (Input.GetAxis("Vertical Alternative") > 0)
            //{
            //    if (!isJumping)
            //    {
            //        isJumping = true;
            //        animator.SetTrigger("Jump");
            //        StartCoroutine(JumpPause());
            //    }
            //}

            // Defensive moves
            // Crouching
            if (defensiveMoveType == 3)
            {
                animator.SetBool("Crouch", true);
                defensiveMoveType = 0;
            }
            // Blocking
            if (defensiveMoveType == 2)
            {
                if (!isBlocking)
                {
                    isBlocking = true;
                    animator.SetTrigger("BlockOn");
                    StartCoroutine(DisableBlock());
                }
            }
            // Jumping
            if (defensiveMoveType == 4)
            {
                animator.SetTrigger("Jump");
                defensiveMoveType = 0;
            }

            // Resets character's restriction if no collision
            if (!restrictObject.gameObject.activeInHierarchy)
            {
                walkingLeft = true;
                walkingRight = true;
            }

            // Blocking logic
            if (playerTwoLayerZero.IsTag("Block"))
            {
                GetComponent<Rigidbody>().isKinematic = true;
                boxCollider.enabled = false;
                capsuleCollider.enabled = false;
            }
            else if (playerTwoLayerZero.IsTag("Motion"))
            {
                boxCollider.enabled = true;
                capsuleCollider.enabled = true;
                GetComponent<Rigidbody>().isKinematic = false;
            }

            // Disable box collider when crouching and sweeping
            if (playerTwoLayerZero.IsTag("Crouching") || playerTwoLayerZero.IsTag("Sweep"))
            {
                boxCollider.enabled = false;
            }

            //// Check flip forward/back
            //if (AIPlayerTwoActions.FlyingJump)
            //{
            //    walkingSpeed = jumpingSpeed;
            //}
            //else
            //{
            //    walkingSpeed = moveSpeed;
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!SaveScript.isPlayerTwoReacting)
        {
            // Play reacting animation only if main colliders is enable
            // and not in react tag
            if ((boxCollider.enabled || capsuleCollider.enabled) && !playerTwoLayerZero.IsTag("React"))
            {
                if (other.gameObject.CompareTag("FistLight"))
                {
                    animator.SetTrigger("HeadReact");
                    audioPlayer.clip = lightPunch;
                    audioPlayer.Play();
                    defensiveMoveType = Random.Range(0, 5);
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
                    defensiveMoveType = Random.Range(0, 5);
                }
            }
        }
    }

    //IEnumerator JumpPause()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    isJumping = false;
    //}

    IEnumerator FacingLeft()
    {
        if (facingLeft)
        {
            facingLeft = false;
            facingRight = true;
            yield return new WaitForSeconds(0.15f);
            playerTwo.transform.Rotate(0, -180, 0);
            animator.SetLayerWeight(1, 0);
        }
    }

    IEnumerator FacingRight()
    {
        if (facingRight)
        {
            facingRight = false;
            facingLeft = true;
            yield return new WaitForSeconds(0.15f);
            playerTwo.transform.Rotate(0, 180, 0);
            animator.SetLayerWeight(1, 1);
        }
    }

    IEnumerator KnockedOut()
    {
        yield return new WaitForSeconds(0.1f);
        enabled = false;
    }

    IEnumerator WalkingPause()
    {
        yield return new WaitForSeconds(0.5f);
        canAIMove = true;
    }

    IEnumerator DisableBlock()
    {
        yield return new WaitForSeconds(blockDuration);
        isBlocking = false;
        animator.SetTrigger("BlockOff");
        defensiveMoveType = 0;
    }
}
