using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nutmeg
{
	/// <summary>
	/// シーンの非同期アンロード
	/// </summary>
	public class UnloadSceneAsyncHandle : IHandle
	{
		string _name;

		public UnloadSceneAsyncHandle(string name) => _name = name;

		public Task Run()
		{
			var tcs = new TaskCompletionSource<bool>();

#if UNITY_EDITOR
			bool exist = false;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				var loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.name == _name)
				{
					exist = true;
					break;
				}
			}
			if (exist == false)
			{
				Debug.LogError($"{_name} はロードされていません");
				tcs.SetResult(false);
				return tcs.Task;
			}
#endif

			var operation = SceneManager.UnloadSceneAsync(_name);
			if (operation == null)
			{
				tcs.SetResult(true);
				return tcs.Task;
			}
			
			operation.completed += _ => tcs.SetResult(true);
			return tcs.Task;
		}
	}
}
