using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Nutmeg
{
	/// <summary>
	/// シーンの非同期ロード
	/// </summary>
	public class LoadSceneAsyncHandle : IHandle
	{
		string _name;

		public LoadSceneAsyncHandle(string name) => _name = name;

		public Task Run()
		{
			var tcs = new TaskCompletionSource<bool>();

			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				var loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.name == _name)
				{
					tcs.SetResult(true);
					return tcs.Task;
				}
			}

			var operation  = SceneManager.LoadSceneAsync(_name, LoadSceneMode.Additive);
			if (operation == null)
			{
				tcs.SetException(new System.Exception($"シーン '{_name}' が見つかりません"));
				return tcs.Task;
			}

			operation.completed += _ => tcs.SetResult(true);
			return tcs.Task;
		}
	}
}

