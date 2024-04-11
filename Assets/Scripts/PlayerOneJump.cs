using UnityEngine;

public class PlayerOneJump : MonoBehaviour
{
    [SerializeField] private GameObject playerOne;
    [SerializeField] private float sideForce = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTwoJumpDetector"))
        {
            if (PlayerOneMovement.facingLeft)
            {
                playerOne.transform.Translate(sideForce, 0, 0);
            }
            if (PlayerOneMovement.facingRight)
            {
                playerOne.transform.Translate(-sideForce, 0, 0);
            }
            
        }
    }
}
