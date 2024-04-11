using UnityEngine;

public class AIPlayerTwoTrigger : MonoBehaviour
{
    [SerializeField] private float damageAmount = 0.05f;
    [SerializeField] private bool emitEffect = false;
    [SerializeField] private float slowSpeed = 0.8f;
    [SerializeField] private string particleName = "P1FX1";
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
        if (!AIPlayerTwoActions.Attack)
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
        if (!SaveScript.isPlayerOneReacting)
        {
            if (other.gameObject.CompareTag("PlayerOne") && !other.gameObject.CompareTag("PlayerOneJumpDetector"))
            {
                if (emitEffect)
                {
                    particles.Play();
                    Time.timeScale = slowSpeed;
                }

                AIPlayerTwoActions.Attack = true;

                SaveScript.PlayerOneHealth -= damageAmount;
                if (SaveScript.PlayerOneTimer < 2.0f)
                {
                    SaveScript.PlayerOneTimer += 2.0f;
                }
            }
        }
    }
}
