using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class Benchmark04 : MonoBehaviour
    {

        [FormerlySerializedAs("SpawnType")] public int m_spawnType = 0;

        [FormerlySerializedAs("MinPointSize")] public int m_minPointSize = 12;
        [FormerlySerializedAs("MaxPointSize")] public int m_maxPointSize = 64;
        [FormerlySerializedAs("Steps")] public int m_steps = 4;

        private Transform m_transform;
        //private TextMeshProFloatingText floatingText_Script;
        //public Material material;


        void Start()
        {
            m_transform = transform;

            float lineHeight = 0;
            float orthoSize = Camera.main.orthographicSize = Screen.height / 2;
            float ratio = (float)Screen.width / Screen.height;

            for (int i = m_minPointSize; i <= m_maxPointSize; i += m_steps)
            {
                if (m_spawnType == 0)
                {
                    // TextMesh Pro Implementation
                    GameObject go = new GameObject("Text - " + i + " Pts");

                    if (lineHeight > orthoSize * 2) return;

                    go.transform.position = m_transform.position + new Vector3(ratio * -orthoSize * 0.975f, orthoSize * 0.975f - lineHeight, 0);

                    TextMeshPro textMeshPro = go.AddComponent<TextMeshPro>();

                    //textMeshPro.fontSharedMaterial = material;
                    //textMeshPro.font = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TextMeshProFont)) as TextMeshProFont;
                    //textMeshPro.anchor = AnchorPositions.Left;
                    textMeshPro.rectTransform.pivot = new Vector2(0, 0.5f);

                    textMeshPro.enableWordWrapping = false;
                    textMeshPro.extraPadding = true;
                    textMeshPro.isOrthographic = true;
                    textMeshPro.fontSize = i;

                    textMeshPro.text = i + " pts - Lorem ipsum dolor sit...";
                    textMeshPro.color = new Color32(255, 255, 255, 255);

                    lineHeight += i;
                }
                else
                {
                    // TextMesh Implementation
                    // Causes crashes since atlas needed exceeds 4096 X 4096
                    /*
                    GameObject go = new GameObject("Arial " + i);

                    //if (lineHeight > orthoSize * 2 * 0.9f) return;

                    go.transform.position = m_Transform.position + new Vector3(ratio * -orthoSize * 0.975f, orthoSize * 0.975f - lineHeight, 1);
                                       
                    TextMesh textMesh = go.AddComponent<TextMesh>();
                    textMesh.font = Resources.Load("Fonts/ARIAL", typeof(Font)) as Font;
                    textMesh.renderer.sharedMaterial = textMesh.font.material;
                    textMesh.anchor = TextAnchor.MiddleLeft;
                    textMesh.fontSize = i * 10;

                    textMesh.color = new Color32(255, 255, 255, 255);
                    textMesh.text = i + " pts - Lorem ipsum dolor sit...";

                    lineHeight += i;
                    */
                }
            }
        }

    }
}
