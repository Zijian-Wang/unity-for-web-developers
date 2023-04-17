using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class TMProInstructionOverlay : MonoBehaviour
    {

        public enum FPSCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        [FormerlySerializedAs("AnchorPosition")] public FPSCounterAnchorPositions m_anchorPosition = FPSCounterAnchorPositions.BottomLeft;

        private const string Instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

        private TextMeshPro m_textMeshPro;
        private TextContainer m_textContainer;
        private Transform m_frameCounterTransform;
        private Camera m_camera;

        //private FpsCounterAnchorPositions last_AnchorPosition;

        void Awake()
        {
            if (!enabled)
                return;

            m_camera = Camera.main;

            GameObject frameCounter = new GameObject("Frame Counter");
            m_frameCounterTransform = frameCounter.transform;
            m_frameCounterTransform.parent = m_camera.transform;
            m_frameCounterTransform.localRotation = Quaternion.identity;


            m_textMeshPro = frameCounter.AddComponent<TextMeshPro>();
            m_textMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            m_textMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");

            m_textMeshPro.fontSize = 30;

            m_textMeshPro.isOverlay = true;
            m_textContainer = frameCounter.GetComponent<TextContainer>();

            Set_FrameCounter_Position(m_anchorPosition);
            //last_AnchorPosition = AnchorPosition;

            m_textMeshPro.text = Instructions;

        }




        void Set_FrameCounter_Position(FPSCounterAnchorPositions anchorPosition)
        {

            switch (anchorPosition)
            {
                case FPSCounterAnchorPositions.TopLeft:
                    //m_TextMeshPro.anchor = AnchorPositions.TopLeft;
                    m_textContainer.anchorPosition = TextContainerAnchors.TopLeft;
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 1, 100.0f));
                    break;
                case FPSCounterAnchorPositions.BottomLeft:
                    //m_TextMeshPro.anchor = AnchorPositions.BottomLeft;
                    m_textContainer.anchorPosition = TextContainerAnchors.BottomLeft;
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 0, 100.0f));
                    break;
                case FPSCounterAnchorPositions.TopRight:
                    //m_TextMeshPro.anchor = AnchorPositions.TopRight;
                    m_textContainer.anchorPosition = TextContainerAnchors.TopRight;
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 100.0f));
                    break;
                case FPSCounterAnchorPositions.BottomRight:
                    //m_TextMeshPro.anchor = AnchorPositions.BottomRight;
                    m_textContainer.anchorPosition = TextContainerAnchors.BottomRight;
                    m_frameCounterTransform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 100.0f));
                    break;
            }
        }
    }
}
