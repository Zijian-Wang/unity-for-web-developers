using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  RollingMovement m_rollingMovement;

  // Start is called before the first frame update
  void Start()
  {
    m_rollingMovement = GetComponent<RollingMovement>();
  }

  public void OnMove(InputAction.CallbackContext context)
  {
    // get movement direction from input
    Vector2 movementDirection = context.ReadValue<Vector2>();
    // set movement direction on RollingMovement
    m_rollingMovement.m_movementDirection = new Vector3(movementDirection.x, 0f, movementDirection.y);
  }
}
