using System.Threading.Tasks;

namespace Nutmeg
{
	public partial class SceneNavigator
	{
		/// <summary>
		/// 【非推奨】シーンをアンロードする
		/// </summary>
		/// <param name="sceneName"></param>
		public async Task UnloadSceneAsync(string sceneName)
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			_handle.Enqueue(new UnloadSceneAsyncHandle(sceneName));

			await _handle?.RunAll();
			_handle = null;
		}

		/// <summary>
		/// 現在のシーンをすべてアンロードする
		/// </summary>
		public async Task UnloadSceneAllAsync()
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			foreach (var scene in _scenes)
			{
				_handle.Enqueue(new UnloadSceneAsyncHandle(name));
			}

			await _handle?.RunAll();
			_handle = null;
		}
	}
}
