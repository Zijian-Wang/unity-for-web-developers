using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI m_gameOverText;
  [SerializeField] TextMeshProUGUI m_timerText;
  [SerializeField] Button m_startGameButton;
  [SerializeField] Score m_playerScore;
  [SerializeField] Score m_opponentScore;
  [SerializeField] float m_gameTimeInSeconds = 60f;
  float m_timer;
  [HideInInspector] public bool m_isGamePlaying;
  public const float RespawnHeight = 0.25f;
  RollingMovement[] m_ballsRollingMovement;
  Collectable m_collectable;
  
  void Start()
  {
    // set the time scale to 0 to pause the game at the start
    Time.timeScale = 0f;
    m_isGamePlaying = false;

    // set the timer to default count down time 
    m_timer = m_gameTimeInSeconds;

    // update timer's text
    UpdateTimerText();
    
    // m_rollingMovements = GameObject.FindObjectsOfType<RollingMovement>();
    m_ballsRollingMovement = gameObject.GetComponentsInChildren<RollingMovement>();
    m_collectable = gameObject.GetComponentInChildren<Collectable>();
  }

  void UpdateTimerText()
  {
    m_timerText.text = $"{Mathf.Floor(m_timer / 60):0}:{Mathf.Floor(m_timer % 60):00}";
  }

  void FixedUpdate()
  {
    if (m_isGamePlaying) {
      UpdateTimerText();

      m_timer -= Time.fixedDeltaTime;

      if (m_timer <= 0)
      {
        GameOver();
        m_timer = 0;
      }
    }
  }

  void GameOver()
  {
    m_isGamePlaying = false;
    Time.timeScale = 0;

    if (m_playerScore.ScoreValue > m_opponentScore.ScoreValue)
      m_gameOverText.text = "You Win!";
    else if (m_playerScore.ScoreValue < m_opponentScore.ScoreValue)
      m_gameOverText.text = "You Loose.";
    else
      m_gameOverText.text = "Tie!";

    m_startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";

    m_gameOverText.gameObject.SetActive(true);
    m_startGameButton.gameObject.SetActive(true);
  }

  public void StartGame()
  {
    foreach (RollingMovement ball in m_ballsRollingMovement)
    {
      ball.ResetPosition();
    }
    m_collectable.ResetPosition();
    
    m_playerScore.ResetScore();
    m_opponentScore.ResetScore();

    m_gameOverText.gameObject.SetActive(false);
    m_startGameButton.gameObject.SetActive(false);

    m_timer = m_gameTimeInSeconds;
    Time.timeScale = 1f;
    m_isGamePlaying = true;
  }
}
