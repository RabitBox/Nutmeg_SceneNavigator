using UnityEngine;
using SpiceKit.Nutmeg;
using SpiceKit.Nutmeg.Data;

public class AwakeCall : MonoBehaviour
{
	[SerializeField]
	private LayerSceneList sceneList;

	private void Awake()
	{
		//Debug.Log("【シーン読み込み完了】");
	}

	private void Start()
	{
		if (sceneList is null) return;
		SceneService.LoadBatch(sceneList);
	}

	private void OnDisable()
	{
		//Debug.Log("【無効化】");
		SceneService.UnloadBatch(sceneList);
	}

	private void OnDestroy()
	{
		//Debug.Log("【シーン削除】");
	}
}
