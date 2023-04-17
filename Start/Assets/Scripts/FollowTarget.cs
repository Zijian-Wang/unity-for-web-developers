using UnityEngine;
using UnityEngine.Serialization;

public class FollowTarget : MonoBehaviour
{
  [SerializeField] Transform m_target;
  [SerializeField] float m_retargetingSpeedLow = 2f;
  [SerializeField] float m_retargetingSpeedHigh = 100f;
  [FormerlySerializedAs("fasterFindDistance")] [SerializeField] float m_fasterFindDistance = 0.15f;
  RollingMovement m_rollingMovement;
  void Start()
  {
    m_rollingMovement = GetComponent<RollingMovement>();
  }

  void FixedUpdate()
  {
    Vector3 targetDirection = m_target.position - transform.position;

    // if collectable is close, than set a high retargeting speed
    // if not, then the speed is m_retargetingSpeed
    float retargetSpeed = Vector3.SqrMagnitude(targetDirection) < m_fasterFindDistance
                          ? m_retargetingSpeedHigh
                          : m_retargetingSpeedLow;

    // lerp the rolling movement between self rolling movement and the target's direction (need to normalize it)
    m_rollingMovement.m_movementDirection = Vector3.Lerp(m_rollingMovement.m_movementDirection,
                                                        targetDirection.normalized,
                                                        Time.fixedDeltaTime * retargetSpeed);
  }
}
