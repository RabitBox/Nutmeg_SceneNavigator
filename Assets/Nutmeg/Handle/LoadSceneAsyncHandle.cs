using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Nutmeg
{
	public class LoadSceneAsyncHandle : IHandle
	{
		string _name;

		public LoadSceneAsyncHandle(string name) => _name = name;

		public Task Run()
		{
			var tcs = new TaskCompletionSource<bool>();

			var operation  = SceneManager.LoadSceneAsync(_name, LoadSceneMode.Additive);
			if (operation != null)
			{
				tcs.SetException(new System.Exception($"シーン '{_name}' が見つかりません"));
			}
			else
			{
				operation.completed += _ => tcs.SetResult(true);
			}

			return tcs.Task;
		}
	}
}

