using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{

    public class TextMeshProFloatingText : MonoBehaviour
    {
        [FormerlySerializedAs("TheFont")] public Font m_theFont;

        private GameObject m_floatingText;
        private TextMeshPro m_textMeshPro;
        private TextMesh m_textMesh;

        private Transform m_transform;
        private Transform m_floatingTextTransform;
        private Transform m_cameraTransform;

        Vector3 m_lastPos = Vector3.zero;
        Quaternion m_lastRotation = Quaternion.identity;

        [FormerlySerializedAs("SpawnType")] public int m_spawnType;
        [FormerlySerializedAs("IsTextObjectScaleStatic")] public bool m_isTextObjectScaleStatic;

        //private int m_frame = 0;

        static WaitForEndOfFrame m_kWaitForEndOfFrame = new WaitForEndOfFrame();
        static WaitForSeconds[] m_kWaitForSecondsRandom = new WaitForSeconds[]
        {
            new WaitForSeconds(0.05f), new WaitForSeconds(0.1f), new WaitForSeconds(0.15f), new WaitForSeconds(0.2f), new WaitForSeconds(0.25f),
            new WaitForSeconds(0.3f), new WaitForSeconds(0.35f), new WaitForSeconds(0.4f), new WaitForSeconds(0.45f), new WaitForSeconds(0.5f),
            new WaitForSeconds(0.55f), new WaitForSeconds(0.6f), new WaitForSeconds(0.65f), new WaitForSeconds(0.7f), new WaitForSeconds(0.75f),
            new WaitForSeconds(0.8f), new WaitForSeconds(0.85f), new WaitForSeconds(0.9f), new WaitForSeconds(0.95f), new WaitForSeconds(1.0f),
        };

        void Awake()
        {
            m_transform = transform;
            m_floatingText = new GameObject(this.name + " floating text");

            // Reference to Transform is lost when TMP component is added since it replaces it by a RectTransform.
            //m_floatingText_Transform = m_floatingText.transform;
            //m_floatingText_Transform.position = m_transform.position + new Vector3(0, 15f, 0);

            m_cameraTransform = Camera.main.transform;
        }

        void Start()
        {
            if (m_spawnType == 0)
            {
                // TextMesh Pro Implementation
                m_textMeshPro = m_floatingText.AddComponent<TextMeshPro>();
                m_textMeshPro.rectTransform.sizeDelta = new Vector2(3, 3);

                m_floatingTextTransform = m_floatingText.transform;
                m_floatingTextTransform.position = m_transform.position + new Vector3(0, 15f, 0);

                //m_textMeshPro.fontAsset = Resources.Load("Fonts & Materials/JOKERMAN SDF", typeof(TextMeshProFont)) as TextMeshProFont; // User should only provide a string to the resource.
                //m_textMeshPro.fontSharedMaterial = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(Material)) as Material;

                m_textMeshPro.alignment = TextAlignmentOptions.Center;
                m_textMeshPro.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                m_textMeshPro.fontSize = 24;
                //m_textMeshPro.enableExtraPadding = true;
                //m_textMeshPro.enableShadows = false;
                m_textMeshPro.enableKerning = false;
                m_textMeshPro.text = string.Empty;
                m_textMeshPro.isTextObjectScaleStatic = m_isTextObjectScaleStatic;

                StartCoroutine(DisplayTextMeshProFloatingText());
            }
            else if (m_spawnType == 1)
            {
                //Debug.Log("Spawning TextMesh Objects.");

                m_floatingTextTransform = m_floatingText.transform;
                m_floatingTextTransform.position = m_transform.position + new Vector3(0, 15f, 0);

                m_textMesh = m_floatingText.AddComponent<TextMesh>();
                m_textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
                m_textMesh.GetComponent<Renderer>().sharedMaterial = m_textMesh.font.material;
                m_textMesh.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                m_textMesh.anchor = TextAnchor.LowerCenter;
                m_textMesh.fontSize = 24;

                StartCoroutine(DisplayTextMeshFloatingText());
            }
            else if (m_spawnType == 2)
            {

            }

        }


        //void Update()
        //{
        //    if (SpawnType == 0)
        //    {
        //        m_textMeshPro.SetText("{0}", m_frame);
        //    }
        //    else
        //    {
        //        m_textMesh.text = m_frame.ToString();
        //    }
        //    m_frame = (m_frame + 1) % 1000;

        //}


        public IEnumerator DisplayTextMeshProFloatingText()
        {
            float countDuration = 2.0f; // How long is the countdown alive.
            float startingCount = Random.Range(5f, 20f); // At what number is the counter starting at.
            float currentCount = startingCount;

            Vector3 startPos = m_floatingTextTransform.position;
            Color32 startColor = m_textMeshPro.color;
            float alpha = 255;
            int intCounter = 0;


            float fadeDuration = 3 / startingCount * countDuration;

            while (currentCount > 0)
            {
                currentCount -= (Time.deltaTime / countDuration) * startingCount;

                if (currentCount <= 3)
                {
                    //Debug.Log("Fading Counter ... " + current_Count.ToString("f2"));
                    alpha = Mathf.Clamp(alpha - (Time.deltaTime / fadeDuration) * 255, 0, 255);
                }

                intCounter = (int)currentCount;
                m_textMeshPro.text = intCounter.ToString();
                //m_textMeshPro.SetText("{0}", (int)current_Count);

                m_textMeshPro.color = new Color32(startColor.r, startColor.g, startColor.b, (byte)alpha);

                // Move the floating text upward each update
                m_floatingTextTransform.position += new Vector3(0, startingCount * Time.deltaTime, 0);

                // Align floating text perpendicular to Camera.
                if (!m_lastPos.Compare(m_cameraTransform.position, 1000) || !m_lastRotation.Compare(m_cameraTransform.rotation, 1000))
                {
                    m_lastPos = m_cameraTransform.position;
                    m_lastRotation = m_cameraTransform.rotation;
                    m_floatingTextTransform.rotation = m_lastRotation;
                    Vector3 dir = m_transform.position - m_lastPos;
                    m_transform.forward = new Vector3(dir.x, 0, dir.z);
                }

                yield return m_kWaitForEndOfFrame;
            }

            //Debug.Log("Done Counting down.");

            yield return m_kWaitForSecondsRandom[Random.Range(0, 19)];

            m_floatingTextTransform.position = startPos;

            StartCoroutine(DisplayTextMeshProFloatingText());
        }


        public IEnumerator DisplayTextMeshFloatingText()
        {
            float countDuration = 2.0f; // How long is the countdown alive.
            float startingCount = Random.Range(5f, 20f); // At what number is the counter starting at.
            float currentCount = startingCount;

            Vector3 startPos = m_floatingTextTransform.position;
            Color32 startColor = m_textMesh.color;
            float alpha = 255;
            int intCounter = 0;

            float fadeDuration = 3 / startingCount * countDuration;

            while (currentCount > 0)
            {
                currentCount -= (Time.deltaTime / countDuration) * startingCount;

                if (currentCount <= 3)
                {
                    //Debug.Log("Fading Counter ... " + current_Count.ToString("f2"));
                    alpha = Mathf.Clamp(alpha - (Time.deltaTime / fadeDuration) * 255, 0, 255);
                }

                intCounter = (int)currentCount;
                m_textMesh.text = intCounter.ToString();
                //Debug.Log("Current Count:" + current_Count.ToString("f2"));

                m_textMesh.color = new Color32(startColor.r, startColor.g, startColor.b, (byte)alpha);

                // Move the floating text upward each update
                m_floatingTextTransform.position += new Vector3(0, startingCount * Time.deltaTime, 0);

                // Align floating text perpendicular to Camera.
                if (!m_lastPos.Compare(m_cameraTransform.position, 1000) || !m_lastRotation.Compare(m_cameraTransform.rotation, 1000))
                {
                    m_lastPos = m_cameraTransform.position;
                    m_lastRotation = m_cameraTransform.rotation;
                    m_floatingTextTransform.rotation = m_lastRotation;
                    Vector3 dir = m_transform.position - m_lastPos;
                    m_transform.forward = new Vector3(dir.x, 0, dir.z);
                }

                yield return m_kWaitForEndOfFrame;
            }

            //Debug.Log("Done Counting down.");

            yield return m_kWaitForSecondsRandom[Random.Range(0, 20)];

            m_floatingTextTransform.position = startPos;

            StartCoroutine(DisplayTextMeshFloatingText());
        }
    }
}
