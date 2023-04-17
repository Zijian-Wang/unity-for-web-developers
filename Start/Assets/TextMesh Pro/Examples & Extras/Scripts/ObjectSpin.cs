using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class ObjectSpin : MonoBehaviour
    {

#pragma warning disable 0414

        [FormerlySerializedAs("SpinSpeed")] public float m_spinSpeed = 5;
        [FormerlySerializedAs("RotationRange")] public int m_rotationRange = 15;
        private Transform m_transform;

        private float m_time;
        private Vector3 m_prevPos;
        private Vector3 m_initialRotation;
        private Vector3 m_initialPosition;
        private Color32 m_lightColor;
        private int m_frames = 0;

        public enum MotionType { Rotation, BackAndForth, Translation };
        [FormerlySerializedAs("Motion")] public MotionType m_motion;

        void Awake()
        {
            m_transform = transform;
            m_initialRotation = m_transform.rotation.eulerAngles;
            m_initialPosition = m_transform.position;

            Light light = GetComponent<Light>();
            m_lightColor = light != null ? light.color : Color.black;
        }


        // Update is called once per frame
        void Update()
        {
            if (m_motion == MotionType.Rotation)
            {
                m_transform.Rotate(0, m_spinSpeed * Time.deltaTime, 0);
            }
            else if (m_motion == MotionType.BackAndForth)
            {
                m_time += m_spinSpeed * Time.deltaTime;
                m_transform.rotation = Quaternion.Euler(m_initialRotation.x, Mathf.Sin(m_time) * m_rotationRange + m_initialRotation.y, m_initialRotation.z);
            }
            else
            {
                m_time += m_spinSpeed * Time.deltaTime;

                float x = 15 * Mathf.Cos(m_time * .95f);
                float y = 10; // *Mathf.Sin(m_time * 1f) * Mathf.Cos(m_time * 1f);
                float z = 0f; // *Mathf.Sin(m_time * .9f);    

                m_transform.position = m_initialPosition + new Vector3(x, z, y);

                // Drawing light patterns because they can be cool looking.
                //if (frames > 2)
                //    Debug.DrawLine(m_transform.position, m_prevPOS, m_lightColor, 100f);

                m_prevPos = m_transform.position;
                m_frames += 1;
            }
        }
    }
}