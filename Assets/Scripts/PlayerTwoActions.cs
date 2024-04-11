using System.Collections;
using UnityEngine;

public class PlayerTwoActions : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpingForce = 0.1f;
    [SerializeField] private GameObject playerTwo;
    [SerializeField] private float punchSlideAmount = 2f;
    [SerializeField] private float heavyReactSlideAmount = 2f;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip punchWoosh;
    [SerializeField] private AudioClip kickWoosh;

    private bool heavyReact = false;
    private bool heavyMoving = false;
    private AnimatorStateInfo playerTwoLayerZero;

    public static bool Attack = false;
    public static bool FlyingJump = false;

    // Start is called before the first frame update
    void Start()
    {

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
                if (PlayerTwoMovement.facingRight)
                {
                    playerTwo.transform.Translate(punchSlideAmount * Time.deltaTime, 0, 0);
                }
                if (PlayerTwoMovement.facingLeft)
                {
                    playerTwo.transform.Translate(-punchSlideAmount * Time.deltaTime, 0, 0);
                }
            }

            // Heavy reaction slide
            if (heavyReact)
            {
                if (PlayerTwoMovement.facingRight)
                {
                    playerTwo.transform.Translate(-heavyReactSlideAmount * Time.deltaTime, 0, 0);
                }
                if (PlayerTwoMovement.facingLeft)
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
                if (Input.GetButtonDown("Fire1 Alternative"))
                {
                    animator.SetTrigger("LightPunch");
                    Attack = false;
                }
                if (Input.GetButtonDown("Fire2 Alternative"))
                {
                    animator.SetTrigger("HeavyPunch");
                    Attack = false;
                }
                if (Input.GetButtonDown("Fire3 Alternative"))
                {
                    animator.SetTrigger("LightKick");
                    Attack = false;
                }
                if (Input.GetButtonDown("Jump Alternative"))
                {
                    animator.SetTrigger("HeavyKick");
                    Attack = false;
                }
                if (Input.GetButtonDown("Block Alternative"))
                {
                    animator.SetTrigger("BlockOn");
                }
            }

            // Crouching attack
            if (playerTwoLayerZero.IsTag("Crouching"))
            {
                if (Input.GetButtonDown("Fire3 Alternative"))
                {
                    animator.SetTrigger("LightKick");
                    Attack = false;
                }
            }
            // Aerial attack
            if (playerTwoLayerZero.IsTag("Jumping"))
            {
                if (Input.GetButtonDown("Jump Alternative"))
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
        playerTwo.transform.Translate(0, jumpingForce, 0);
    }

    public void FlipUp()
    {
        // Only flip once
        if(!FlyingJump)
        {
            playerTwo.transform.Translate(0, jumpingForce, 0);
            FlyingJump = true;
        }
    }

    public void FlipBack()
    {
        // Only flip once
        if (!FlyingJump)
        {
            playerTwo.transform.Translate(0, jumpingForce, 0);
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
