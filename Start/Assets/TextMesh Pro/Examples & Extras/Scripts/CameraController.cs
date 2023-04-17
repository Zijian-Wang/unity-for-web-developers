using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class CameraController : MonoBehaviour
    {
        public enum CameraModes { Follow, Isometric, Free }

        private Transform m_cameraTransform;
        private Transform m_dummyTarget;

        [FormerlySerializedAs("CameraTarget")] public Transform m_cameraTarget;

        [FormerlySerializedAs("FollowDistance")] public float m_followDistance = 30.0f;
        [FormerlySerializedAs("MaxFollowDistance")] public float m_maxFollowDistance = 100.0f;
        [FormerlySerializedAs("MinFollowDistance")] public float m_minFollowDistance = 2.0f;

        [FormerlySerializedAs("ElevationAngle")] public float m_elevationAngle = 30.0f;
        [FormerlySerializedAs("MaxElevationAngle")] public float m_maxElevationAngle = 85.0f;
        [FormerlySerializedAs("MinElevationAngle")] public float m_minElevationAngle = 0f;

        [FormerlySerializedAs("OrbitalAngle")] public float m_orbitalAngle = 0f;

        [FormerlySerializedAs("CameraMode")] public CameraModes m_cameraMode = CameraModes.Follow;

        [FormerlySerializedAs("MovementSmoothing")] public bool m_movementSmoothing = true;
        [FormerlySerializedAs("RotationSmoothing")] public bool m_rotationSmoothing = false;
        private bool m_previousSmoothing;

        [FormerlySerializedAs("MovementSmoothingValue")] public float m_movementSmoothingValue = 25f;
        [FormerlySerializedAs("RotationSmoothingValue")] public float m_rotationSmoothingValue = 5.0f;

        [FormerlySerializedAs("MoveSensitivity")] public float m_moveSensitivity = 2.0f;

        private Vector3 m_currentVelocity = Vector3.zero;
        private Vector3 m_desiredPosition;
        private float m_mouseX;
        private float m_mouseY;
        private Vector3 m_moveVector;
        private float m_mouseWheel;

        // Controls for Touches on Mobile devices
        //private float prev_ZoomDelta;


        private const string EventSmoothingValue = "Slider - Smoothing Value";
        private const string EventFollowDistance = "Slider - Camera Zoom";


        void Awake()
        {
            if (QualitySettings.vSyncCount > 0)
                Application.targetFrameRate = 60;
            else
                Application.targetFrameRate = -1;

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
                Input.simulateMouseWithTouches = false;

            m_cameraTransform = transform;
            m_previousSmoothing = m_movementSmoothing;
        }


        // Use this for initialization
        void Start()
        {
            if (m_cameraTarget == null)
            {
                // If we don't have a target (assigned by the player, create a dummy in the center of the scene).
                m_dummyTarget = new GameObject("Camera Target").transform;
                m_cameraTarget = m_dummyTarget;
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            GetPlayerInput();


            // Check if we still have a valid target
            if (m_cameraTarget != null)
            {
                if (m_cameraMode == CameraModes.Isometric)
                {
                    m_desiredPosition = m_cameraTarget.position + Quaternion.Euler(m_elevationAngle, m_orbitalAngle, 0f) * new Vector3(0, 0, -m_followDistance);
                }
                else if (m_cameraMode == CameraModes.Follow)
                {
                    m_desiredPosition = m_cameraTarget.position + m_cameraTarget.TransformDirection(Quaternion.Euler(m_elevationAngle, m_orbitalAngle, 0f) * (new Vector3(0, 0, -m_followDistance)));
                }
                else
                {
                    // Free Camera implementation
                }

                if (m_movementSmoothing == true)
                {
                    // Using Smoothing
                    m_cameraTransform.position = Vector3.SmoothDamp(m_cameraTransform.position, m_desiredPosition, ref m_currentVelocity, m_movementSmoothingValue * Time.fixedDeltaTime);
                    //cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 5.0f);
                }
                else
                {
                    // Not using Smoothing
                    m_cameraTransform.position = m_desiredPosition;
                }

                if (m_rotationSmoothing == true)
                    m_cameraTransform.rotation = Quaternion.Lerp(m_cameraTransform.rotation, Quaternion.LookRotation(m_cameraTarget.position - m_cameraTransform.position), m_rotationSmoothingValue * Time.deltaTime);
                else
                {
                    m_cameraTransform.LookAt(m_cameraTarget);
                }

            }

        }



        void GetPlayerInput()
        {
            m_moveVector = Vector3.zero;

            // Check Mouse Wheel Input prior to Shift Key so we can apply multiplier on Shift for Scrolling
            m_mouseWheel = Input.GetAxis("Mouse ScrollWheel");

            float touchCount = Input.touchCount;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || touchCount > 0)
            {
                m_mouseWheel *= 10;

                if (Input.GetKeyDown(KeyCode.I))
                    m_cameraMode = CameraModes.Isometric;

                if (Input.GetKeyDown(KeyCode.F))
                    m_cameraMode = CameraModes.Follow;

                if (Input.GetKeyDown(KeyCode.S))
                    m_movementSmoothing = !m_movementSmoothing;


                // Check for right mouse button to change camera follow and elevation angle
                if (Input.GetMouseButton(1))
                {
                    m_mouseY = Input.GetAxis("Mouse Y");
                    m_mouseX = Input.GetAxis("Mouse X");

                    if (m_mouseY > 0.01f || m_mouseY < -0.01f)
                    {
                        m_elevationAngle -= m_mouseY * m_moveSensitivity;
                        // Limit Elevation angle between min & max values.
                        m_elevationAngle = Mathf.Clamp(m_elevationAngle, m_minElevationAngle, m_maxElevationAngle);
                    }

                    if (m_mouseX > 0.01f || m_mouseX < -0.01f)
                    {
                        m_orbitalAngle += m_mouseX * m_moveSensitivity;
                        if (m_orbitalAngle > 360)
                            m_orbitalAngle -= 360;
                        if (m_orbitalAngle < 0)
                            m_orbitalAngle += 360;
                    }
                }

                // Get Input from Mobile Device
                if (touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;

                    // Handle elevation changes
                    if (deltaPosition.y > 0.01f || deltaPosition.y < -0.01f)
                    {
                        m_elevationAngle -= deltaPosition.y * 0.1f;
                        // Limit Elevation angle between min & max values.
                        m_elevationAngle = Mathf.Clamp(m_elevationAngle, m_minElevationAngle, m_maxElevationAngle);
                    }


                    // Handle left & right 
                    if (deltaPosition.x > 0.01f || deltaPosition.x < -0.01f)
                    {
                        m_orbitalAngle += deltaPosition.x * 0.1f;
                        if (m_orbitalAngle > 360)
                            m_orbitalAngle -= 360;
                        if (m_orbitalAngle < 0)
                            m_orbitalAngle += 360;
                    }

                }

                // Check for left mouse button to select a new CameraTarget or to reset Follow position
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 300, 1 << 10 | 1 << 11 | 1 << 12 | 1 << 14))
                    {
                        if (hit.transform == m_cameraTarget)
                        {
                            // Reset Follow Position
                            m_orbitalAngle = 0;
                        }
                        else
                        {
                            m_cameraTarget = hit.transform;
                            m_orbitalAngle = 0;
                            m_movementSmoothing = m_previousSmoothing;
                        }

                    }
                }


                if (Input.GetMouseButton(2))
                {
                    if (m_dummyTarget == null)
                    {
                        // We need a Dummy Target to anchor the Camera
                        m_dummyTarget = new GameObject("Camera Target").transform;
                        m_dummyTarget.position = m_cameraTarget.position;
                        m_dummyTarget.rotation = m_cameraTarget.rotation;
                        m_cameraTarget = m_dummyTarget;
                        m_previousSmoothing = m_movementSmoothing;
                        m_movementSmoothing = false;
                    }
                    else if (m_dummyTarget != m_cameraTarget)
                    {
                        // Move DummyTarget to CameraTarget
                        m_dummyTarget.position = m_cameraTarget.position;
                        m_dummyTarget.rotation = m_cameraTarget.rotation;
                        m_cameraTarget = m_dummyTarget;
                        m_previousSmoothing = m_movementSmoothing;
                        m_movementSmoothing = false;
                    }


                    m_mouseY = Input.GetAxis("Mouse Y");
                    m_mouseX = Input.GetAxis("Mouse X");

                    m_moveVector = m_cameraTransform.TransformDirection(m_mouseX, m_mouseY, 0);

                    m_dummyTarget.Translate(-m_moveVector, Space.World);

                }

            }

            // Check Pinching to Zoom in - out on Mobile device
            if (touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                float prevTouchDelta = (touch0PrevPos - touch1PrevPos).magnitude;
                float touchDelta = (touch0.position - touch1.position).magnitude;

                float zoomDelta = prevTouchDelta - touchDelta;

                if (zoomDelta > 0.01f || zoomDelta < -0.01f)
                {
                    m_followDistance += zoomDelta * 0.25f;
                    // Limit FollowDistance between min & max values.
                    m_followDistance = Mathf.Clamp(m_followDistance, m_minFollowDistance, m_maxFollowDistance);
                }


            }

            // Check MouseWheel to Zoom in-out
            if (m_mouseWheel < -0.01f || m_mouseWheel > 0.01f)
            {

                m_followDistance -= m_mouseWheel * 5.0f;
                // Limit FollowDistance between min & max values.
                m_followDistance = Mathf.Clamp(m_followDistance, m_minFollowDistance, m_maxFollowDistance);
            }


        }
    }
}