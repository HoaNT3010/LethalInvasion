using UnityEngine;

public class PlayerTwoMoveRestrict : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerOneLeftBlock"))
        {
            PlayerTwoMovement.walkingRight = false;
        }
        if (other.gameObject.CompareTag("PlayerOneRightBlock"))
        {
            PlayerTwoMovement.walkingLeft = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerOneLeftBlock"))
        {
            PlayerTwoMovement.walkingRight = true;
        }
        if (other.gameObject.CompareTag("PlayerOneRightBlock"))
        {
            PlayerTwoMovement.walkingLeft = true;
        }
    }
}
