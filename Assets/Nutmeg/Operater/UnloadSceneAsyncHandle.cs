using System.Threading.Tasks;
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

			var operation = SceneManager.UnloadSceneAsync(_name);
			if (operation == null)
			{
				tcs.SetResult(true);
			}
			else
			{
				operation.completed += _ => tcs.SetResult(true);
			}

			return tcs.Task;
		}
	}
}
