using RV.SpiceKit.Nutmeg;
using RV.SpiceKit.Nutmeg.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZeroMessenger;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private SceneFlowController sceneFlowController;
	[SerializeField] private TMP_Dropdown	dropdown;
	[SerializeField] private Button			button;

	private void Awake()
	{
		MessageBroker<LoadingPhaseChanged>.Default.Subscribe(p => {
			Debug.Log($"読み込み: {p.Current}");
		});
	}

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
