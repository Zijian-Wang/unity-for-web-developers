using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class TMPFrameRateCounter : MonoBehaviour
    {
        [FormerlySerializedAs("UpdateInterval")] public float m_updateInterval = 5.0f;
        private float m_lastInterval = 0;
        private int m_frames = 0;

        public enum FPSCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        [FormerlySerializedAs("AnchorPosition")] public FPSCounterAnchorPositions m_anchorPosition = FPSCounterAnchorPositions.TopRight;

        private string m_htmlColorTag;
        private const string FPSLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

        private TextMeshPro m_textMeshPro;
        private Transform m_frameCounterTransform;
        private Camera m_camera;

        private FPSCounterAnchorPositions m_lastAnchorPosition;

        void Awake()
        {
            if (!enabled)
                return;

            m_camera = Camera.main;
            Application.targetFrameRate = 9999;

            GameObject frameCounter = new GameObject("Frame Counter");

            m_textMeshPro = frameCounter.AddComponent<TextMeshPro>();
            m_textMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            m_textMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");


            m_frameCounterTransform = frameCounter.transform;
            m_frameCounterTransform.SetParent(m_camera.transform);
            m_frameCounterTransform.localRotation = Quaternion.identity;

            m_textMeshPro.enableWordWrapping = false;
            m_textMeshPro.fontSize = 24;
            //m_TextMeshPro.FontColor = new Color32(255, 255, 255, 128);
            //m_TextMeshPro.edgeWidth = .15f;
            //m_TextMeshPro.isOverlay = true;

            //m_TextMeshPro.FaceColor = new Color32(255, 128, 0, 0);
            //m_TextMeshPro.EdgeColor = new Color32(0, 255, 0, 255);
            //m_TextMeshPro.FontMaterial.renderQueue = 4000;

            //m_TextMeshPro.CreateSoftShadowClone(new Vector2(1f, -1f));

            Set_FrameCounter_Position(m_anchorPosition);
            m_lastAnchorPosition = m_anchorPosition;


        }

        void Start()
        {
            m_lastInterval = Time.realtimeSinceStartup;
            m_frames = 0;
        }

        void Update()
        {
            if (m_anchorPosition != m_lastAnchorPosition)
                Set_FrameCounter_Position(m_anchorPosition);

            m_lastAnchorPosition = m_anchorPosition;

            m_frames += 1;
            float timeNow = Time.realtimeSinceStartup;

            if (timeNow > m_lastInterval + m_updateInterval)
            {
                // display two fractional digits (f2 format)
                float fps = m_frames / (timeNow - m_lastInterval);
                float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

                if (fps < 30)
                    m_htmlColorTag = "<color=yellow>";
                else if (fps < 10)
                    m_htmlColorTag = "<color=red>";
                else
                    m_htmlColorTag = "<color=green>";

                //string format = System.String.Format(htmlColorTag + "{0:F2} </color>FPS \n{1:F2} <#8080ff>MS",fps, ms);
                //m_TextMeshPro.text = format;

                m_textMeshPro.SetText(m_htmlColorTag + FPSLabel, fps, ms);

                m_frames = 0;
                m_lastInterval = timeNow;
            }
        }


        void Set_FrameCounter_Position(FPSCounterAnchorPositions anchorPosition)
        {
            //Debug.Log("Changing frame counter anchor position.");
            m_textMeshPro.margin = new Vector4(1f, 1f, 1f, 1f);

            switch (anchorPosition)
            {
                case FPSCounterAnchorPositions.TopLeft:
                    m_textMeshPro.alignment = TextAlignmentOptions.TopLeft;
                    m_textMeshPro.rectTransform.pivot = new Vector2(0, 1);
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 1, 100.0f));
                    break;
                case FPSCounterAnchorPositions.BottomLeft:
                    m_textMeshPro.alignment = TextAlignmentOptions.BottomLeft;
                    m_textMeshPro.rectTransform.pivot = new Vector2(0, 0);
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 0, 100.0f));
                    break;
                case FPSCounterAnchorPositions.TopRight:
                    m_textMeshPro.alignment = TextAlignmentOptions.TopRight;
                    m_textMeshPro.rectTransform.pivot = new Vector2(1, 1);
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 100.0f));
                    break;
                case FPSCounterAnchorPositions.BottomRight:
                    m_textMeshPro.alignment = TextAlignmentOptions.BottomRight;
                    m_textMeshPro.rectTransform.pivot = new Vector2(1, 0);
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 100.0f));
                    break;
            }
        }
    }
}
