using UnityEngine;

public class RollingMovement : MonoBehaviour
{
  [SerializeField] float m_speed = 1f;
  Rigidbody m_rigidbody;
  [HideInInspector] public Vector3 m_movementDirection;
  Vector3 m_startPosition;
  GameManager m_gameManager;

  // Start is called before the first frame update
  void Start()
  {
    // Gather components and variables for later use
    m_rigidbody = GetComponent<Rigidbody>();
    m_startPosition = transform.position;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    // every physics tick, update position based on movement direction and speed
    m_rigidbody.AddForce(m_movementDirection * m_speed);

    // reset position if Y position is too low (knocked out of the board)
    if (transform.position.y <= GameManager.RespawnHeight)
      ResetPosition();

  }

  public void ResetPosition()
  {
      // reset position to start position
      m_rigidbody.velocity = Vector3.zero;
      m_rigidbody.angularVelocity = Vector3.zero;
      transform.position = m_startPosition;
  }
}
