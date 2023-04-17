using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DropdownSample: MonoBehaviour
{
	[FormerlySerializedAs("text")] [SerializeField]
	private TextMeshProUGUI m_text = null;

	[FormerlySerializedAs("dropdownWithoutPlaceholder")] [SerializeField]
	private TMP_Dropdown m_dropdownWithoutPlaceholder = null;

	[FormerlySerializedAs("dropdownWithPlaceholder")] [SerializeField]
	private TMP_Dropdown m_dropdownWithPlaceholder = null;

	public void OnButtonClick()
	{
		m_text.text = m_dropdownWithPlaceholder.value > -1 ? "Selected values:\n" + m_dropdownWithoutPlaceholder.value + " - " + m_dropdownWithPlaceholder.value : "Error: Please make a selection";
	}
}
