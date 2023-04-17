using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class EnvMapAnimator : MonoBehaviour {

    //private Vector3 TranslationSpeeds;
    [FormerlySerializedAs("RotationSpeeds")] public Vector3 m_rotationSpeeds;
    private TMP_Text m_textMeshPro;
    private Material m_material;
    

    void Awake()
    {
        //Debug.Log("Awake() on Script called.");
        m_textMeshPro = GetComponent<TMP_Text>();
        m_material = m_textMeshPro.fontSharedMaterial;
    }

    // Use this for initialization
	IEnumerator Start ()
    {
        Matrix4x4 matrix = new Matrix4x4(); 
        
        while (true)
        {
            //matrix.SetTRS(new Vector3 (Time.time * TranslationSpeeds.x, Time.time * TranslationSpeeds.y, Time.time * TranslationSpeeds.z), Quaternion.Euler(Time.time * RotationSpeeds.x, Time.time * RotationSpeeds.y , Time.time * RotationSpeeds.z), Vector3.one);
             matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * m_rotationSpeeds.x, Time.time * m_rotationSpeeds.y , Time.time * m_rotationSpeeds.z), Vector3.one);

            m_material.SetMatrix("_EnvMatrix", matrix);

            yield return null;
        }
	}
}
