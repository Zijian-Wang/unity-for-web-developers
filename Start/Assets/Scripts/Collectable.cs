using UnityEngine;

public class Collectable : MonoBehaviour
{
  [SerializeField] float m_spawnRange = 0.4f;
  Vector3 m_startPosition;

  void Start()
  {
    m_startPosition = transform.position;
  }

  void OnTriggerStay(Collider other)
  {
    other.gameObject.GetComponent<Score>().IncrementScore();

    transform.position = new Vector3(Random.Range(-m_spawnRange, m_spawnRange),
                                     transform.position.y,
                                     Random.Range(-m_spawnRange, m_spawnRange));

  }

  public void ResetPosition()
  {
    transform.position = m_startPosition;
  }
}
