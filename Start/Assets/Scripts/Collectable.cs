using UnityEngine;

public class Collectable : MonoBehaviour
{
  [SerializeField] float m_spawnRange = 0.4f;
  Vector3 m_startPosition;
  GameManager m_gameManager;

  void Start()
  {
    m_startPosition = transform.position;
    m_gameManager = GetComponent<GameManager>();
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
    // if (m_gameManager.m_isGamePlaying) return;
    transform.position = m_startPosition;
  }
}
