using System.Threading.Tasks;

namespace Nutmeg
{
	public partial class SceneNavigator
	{
		/// <summary>
		/// 【非推奨】シーンを読み込む
		/// </summary>
		/// <param name="sceneName"></param>
		public async Task LoadSceneAsync(string sceneName)
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			_handle.Enqueue(new LoadSceneAsyncHandle(sceneName));

			await _handle?.RunAll();
			_handle = null;
		}

		/// <summary>
		/// 永続シーンをロード
		/// </summary>
		public async Task LoadPermanentSceneAsync()
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			foreach (var name in _config.Permanents)
			{
				_handle.Enqueue(new LoadSceneAsyncHandle(name));
			}

			await _handle?.RunAll();
			_handle = null;
		}

		/// <summary>
		/// バンドルされたシーンを一括ロード
		/// </summary>
		/// <param name="bundleName"></param>
		public async Task LoadSceneBundleAsync(string bundleName)
		{
			if (_handle is not null) return; // 実行中

			foreach (var bundle in _config.SceneBundles)
			{
				if (bundle.Name != bundleName)
				{
					continue;
				}

				_handle = new HandleQueue();
				foreach (var name in bundle.SceneNames)
				{
					_handle.Enqueue(new LoadSceneAsyncHandle(name));
				}
				break;
			}

			await _handle?.RunAll();
			_handle = null;
		}
	}
}
