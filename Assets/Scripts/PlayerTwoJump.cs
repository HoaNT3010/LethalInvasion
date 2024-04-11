using UnityEngine;

public class PlayerTwoJump : MonoBehaviour
{
    [SerializeField] private GameObject playerTwo;
    [SerializeField] private float sideForce = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerOneJumpDetector"))
        {
            if (PlayerTwoMovement.facingLeft)
            {
                playerTwo.transform.Translate(-sideForce, 0, 0);
            }
            if (PlayerTwoMovement.facingRight)
            {
                playerTwo.transform.Translate(sideForce, 0, 0);
            }
        }
    }
}
