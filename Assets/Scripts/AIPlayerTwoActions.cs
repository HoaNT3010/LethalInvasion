using System.Collections;
using UnityEngine;

public class AIPlayerTwoActions : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpingForce = 0.1f;
    [SerializeField] private GameObject playerTwo;
    [SerializeField] private float punchSlideAmount = 2f;
    [SerializeField] private float heavyReactSlideAmount = 2f;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip punchWoosh;
    [SerializeField] private AudioClip kickWoosh;
    [SerializeField] private float attackRate = 1.0f;
    [SerializeField] private float dazedDuration = 3.0f;

    private bool heavyReact = false;
    private bool heavyMoving = false;
    private AnimatorStateInfo playerTwoLayerZero;
    private int attackNumber = 1;
    private bool isAttacking = true;

    public static bool Attack = false;
    public static bool FlyingJump = false;
    public static bool dazed = false;

    // Start is called before the first frame update
    void Start()
    {
        attackRate = attackRate * SaveScript.AIDifficultyRate;
        dazed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SaveScript.isTimeOut)
        {
            // Listen to animator
            playerTwoLayerZero = animator.GetCurrentAnimatorStateInfo(0);

            // Heavy punch slide
            if (heavyMoving)
            {
                if (AIPlayerTwoMovement.facingRight)
                {
                    playerTwo.transform.Translate(punchSlideAmount * Time.deltaTime, 0, 0);
                }
                if (AIPlayerTwoMovement.facingLeft)
                {
                    playerTwo.transform.Translate(-punchSlideAmount * Time.deltaTime, 0, 0);
                }
            }

            // Heavy reaction slide
            if (heavyReact)
            {
                if (AIPlayerTwoMovement.facingRight)
                {
                    playerTwo.transform.Translate(-heavyReactSlideAmount * Time.deltaTime, 0, 0);
                }
                if (AIPlayerTwoMovement.facingLeft)
                {
                    playerTwo.transform.Translate(heavyReactSlideAmount * Time.deltaTime, 0, 0);
                }
            }

            // Cancel blocking
            if (playerTwoLayerZero.IsTag("Block"))
            {
                if (Input.GetButtonUp("Block Alternative"))
                {
                    animator.SetTrigger("BlockOff");
                }
            }

            // Standing attacks
            if (playerTwoLayerZero.IsTag("Motion"))
            {
                if (isAttacking)
                {
                    isAttacking = false;
                    if (attackNumber == 1)
                    {
                        animator.SetTrigger("LightPunch");
                        Attack = false;
                        StartCoroutine(AttackPause());
                    }
                    if (attackNumber == 2)
                    {
                        animator.SetTrigger("HeavyPunch");
                        Attack = false;
                        StartCoroutine(AttackPause());
                    }
                    if (attackNumber == 3)
                    {
                        animator.SetTrigger("LightKick");
                        Attack = false;
                        StartCoroutine(AttackPause());
                    }
                    if (attackNumber == 4)
                    {
                        animator.SetTrigger("HeavyKick");
                        Attack = false;
                        StartCoroutine(AttackPause());
                    }
                    //if (Input.GetButtonDown("Block Alternative"))
                    //{
                    //    animator.SetTrigger("BlockOn");
                    //}
                }
            }

            // Crouching attack
            if (playerTwoLayerZero.IsTag("Crouching"))
            {
                animator.SetTrigger("LightKick");
                Attack = false;
                animator.SetBool("Crouch", false);
                StartCoroutine(AttackPause());
            }
            //// Aerial attack
            //if (playerTwoLayerZero.IsTag("Jumping"))
            //{
            //    if (Input.GetButtonDown("Jump Alternative"))
            //    {
            //        animator.SetTrigger("HeavyKick");
            //        Attack = false;
            //    }
            //}
        }
    }

    public void HeavyReaction()
    {
        StartCoroutine(HeavyReactionSlide());
        attackNumber = 3;
    }

    public void JumpUp()
    {
        playerTwo.transform.Translate(0, jumpingForce, 0);
    }

    public void FlipUp()
    {
        playerTwo.transform.Translate(0, jumpingForce, 0);
        FlyingJump = true;
    }

    public void FlipBack()
    {
        playerTwo.transform.Translate(0, jumpingForce, 0);
        FlyingJump = true;
    }

    public void IdleSpeed()
    {
        FlyingJump = false;
    }

    public void HeavyPunchMove()
    {
        StartCoroutine(PunchSlide());
    }

    IEnumerator PunchSlide()
    {
        heavyMoving = true;
        yield return new WaitForSeconds(0.1f);
        heavyMoving = false;
    }

    IEnumerator HeavyReactionSlide()
    {
        heavyReact = true;
        dazed = true;
        yield return new WaitForSeconds(0.3f);
        heavyReact = false;
        yield return new WaitForSeconds(dazedDuration);
        dazed = false;
    }

    public void PunchWooshSound()
    {
        audioPlayer.clip = punchWoosh;
        audioPlayer.Play();
    }

    public void KickWooshSound()
    {
        audioPlayer.clip = kickWoosh;
        audioPlayer.Play();
    }

    public void RandomAttack()
    {
        attackNumber = Random.Range(1, 5);
        //StartCoroutine(AttackPause());
    }

    IEnumerator AttackPause()
    {
        yield return new WaitForSeconds(attackRate);
        isAttacking = true;
    }
}
