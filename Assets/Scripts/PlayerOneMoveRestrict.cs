using UnityEngine;

public class PlayerOneMoveRestrict : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTwoLeftBlock"))
        {
            PlayerOneMovement.walkingRight = false;
        }
        if (other.gameObject.CompareTag("PlayerTwoRightBlock"))
        {
            PlayerOneMovement.walkingLeft = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTwoLeftBlock"))
        {
            PlayerOneMovement.walkingRight = true;
        }
        if (other.gameObject.CompareTag("PlayerTwoRightBlock"))
        {
            PlayerOneMovement.walkingLeft = true;
        }
    }
}
