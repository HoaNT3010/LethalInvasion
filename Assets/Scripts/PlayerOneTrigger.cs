using UnityEngine;

public class PlayerOneTrigger : MonoBehaviour
{
    [SerializeField] private float damageAmount = 0.05f;
    [SerializeField] private bool emitEffect = false;
    [SerializeField] private float slowSpeed = 0.8f;
    [SerializeField] private string particleName = "P2FX1";
    [SerializeField] GameObject chosenParticle;

    private Collider attackCollider;
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        attackCollider = gameObject.GetComponentInParent<Collider>();
        chosenParticle = GameObject.Find(particleName);
        particles = chosenParticle.gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerOneActions.Attack)
        {
            attackCollider.enabled = true;
        }
        else
        {
            attackCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!SaveScript.isPlayerTwoReacting)
        {
            if (other.gameObject.CompareTag("PlayerTwo") && !other.gameObject.CompareTag("PlayerTwoJumpDetector"))
            {
                if (emitEffect)
                {
                    particles.Play();
                    Time.timeScale = slowSpeed;
                }

                PlayerOneActions.Attack = true;

                SaveScript.PlayerTwoHealth -= damageAmount;
                if (SaveScript.PlayerTwoTimer < 2.0f)
                {
                    SaveScript.PlayerTwoTimer += 2.0f;
                }
            }
        }
    }
}
