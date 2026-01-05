using UnityEngine;

public class AwakeCall : MonoBehaviour
{
	private void Awake()
	{
		Debug.Log("【シーン読み込み完了】");
	}

	private void OnDisable()
	{
		Debug.Log("【無効化】");
	}

	private void OnDestroy()
	{
		Debug.Log("【シーン削除】");
	}
}
