using UnityEngine;
using RV.SpiceKit.Nutmeg;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private SceneFlowController sceneFlowController;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		//Nutmeg.SceneNavigator.Instance.LoadSceneBundleAsync("Main");
		//Nutmeg.SceneNavigator.Instance.UnloadSceneAsync("SUB");
		sceneFlowController?.LoadSceneBundleAsync("Main");
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
