using UnityEngine;

public class CameraFocusMovement : MonoBehaviour
{
    [SerializeField] Vector3 m_cameraFocusCenter;
    [SerializeField] GameObject m_player;
    [SerializeField] GameObject m_opponent;
    Transform m_playerTransform;
    Transform m_opponentTransform;
    Vector3 m_cameraNewPosition;
    Vector3 m_cameraOffset;
    
    void Start()
    {
        m_cameraOffset = transform.position - m_cameraFocusCenter;
        m_playerTransform = m_player.GetComponent<Transform>();
        m_opponentTransform = m_opponent.GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 targetPosition = GetPlayerOpponentMiddlePoint();
        m_cameraNewPosition = new Vector3(targetPosition.x + m_cameraOffset.x, 
                                            m_cameraOffset.y, 
                                          targetPosition.z + m_cameraOffset.z);

        transform.position = Vector3.Lerp(transform.position, m_cameraNewPosition, Time.deltaTime);
    }

    Vector3 GetPlayerOpponentMiddlePoint()
    {
        return Vector3.Lerp(m_playerTransform.position, m_opponentTransform.position, 0.5f);
    }
}
