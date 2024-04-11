using System.Collections;
using UnityEngine;

public class PlayerOneActions : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpingForce = 0.1f;
    [SerializeField] private GameObject playerOne;
    [SerializeField] private float punchSlideAmount = 2f;
    [SerializeField] private float heavyReactSlideAmount = 2f;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip punchWoosh;
    [SerializeField] private AudioClip kickWoosh;

    private bool heavyReact = false;
    private bool heavyMoving = false;
    private AnimatorStateInfo playerOneLayerZero;

    public static bool Attack = false;
    public static bool FlyingJump = false;

    // Start is called before the first frame update
    void Start()
    {
        FlyingJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SaveScript.isTimeOut)
        {
            // Listen to animator
            playerOneLayerZero = animator.GetCurrentAnimatorStateInfo(0);

            // Heavy punch slide
            if (heavyMoving)
            {
                if (PlayerOneMovement.facingRight)
                {
                    playerOne.transform.Translate(punchSlideAmount * Time.deltaTime, 0, 0);
                }
                if (PlayerOneMovement.facingLeft)
                {
                    playerOne.transform.Translate(-punchSlideAmount * Time.deltaTime, 0, 0);
                }
            }

            // Heavy reaction slide
            if (heavyReact)
            {
                if (PlayerOneMovement.facingRight)
                {
                    playerOne.transform.Translate(-heavyReactSlideAmount * Time.deltaTime, 0, 0);
                }
                if (PlayerOneMovement.facingLeft)
                {
                    playerOne.transform.Translate(heavyReactSlideAmount * Time.deltaTime, 0, 0);
                }
            }

            // Cancel blocking
            if (playerOneLayerZero.IsTag("Block"))
            {
                if (Input.GetButtonUp("Block"))
                {
                    animator.SetTrigger("BlockOff");
                }
            }

            // Standing attacks
            if (playerOneLayerZero.IsTag("Motion"))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    animator.SetTrigger("LightPunch");
                    Attack = false;
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("HeavyPunch");
                    Attack = false;
                }
                if (Input.GetButtonDown("Fire3"))
                {
                    animator.SetTrigger("LightKick");
                    Attack = false;
                }
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("HeavyKick");
                    Attack = false;
                }
                if (Input.GetButtonDown("Block"))
                {
                    animator.SetTrigger("BlockOn");
                }
            }

            // Crouching attack
            if (playerOneLayerZero.IsTag("Crouching"))
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    animator.SetTrigger("LightKick");
                    Attack = false;
                }
            }
            // Aerial attack
            if (playerOneLayerZero.IsTag("Jumping"))
            {
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("HeavyKick");
                    Attack = false;
                }
            }
        }
    }

    public void HeavyReaction()
    {
        StartCoroutine(HeavyReactionSlide());
    }

    public void JumpUp()
    {
        playerOne.transform.Translate(0, jumpingForce, 0);
    }

    public void FlipUp()
    {
        // Only flip once
        if (!FlyingJump)
        {
            playerOne.transform.Translate(0, jumpingForce, 0);
            FlyingJump = true;
        }
    }

    public void FlipBack()
    {

        // Only flip once
        if (!FlyingJump)
        {
            playerOne.transform.Translate(0, jumpingForce, 0);
            FlyingJump = true;
        }
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
        yield return new WaitForSeconds(0.3f);
        heavyReact = false;
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
    }
}
