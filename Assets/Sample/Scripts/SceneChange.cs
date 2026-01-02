using UnityEngine;
using RV.SpiceKit.Nutmeg;
using UnityEngine.UI;
using TMPro;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private SceneFlowController sceneFlowController;
	[SerializeField] private TMP_Dropdown	dropdown;
	[SerializeField] private Button			button;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		button.onClick.AddListener(OnPressButton);
	}

	private void OnPressButton()
	{
		switch (dropdown.value)
		{
			case 0:
				sceneFlowController?.LoadSceneBundleAsync("Main");
				break;

			case 1:
				sceneFlowController?.LoadSceneBundleAsync("Sub");
				break;
		}
	}
}
