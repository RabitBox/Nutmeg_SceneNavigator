namespace Nutmeg
{
	public partial class SceneNavigator
	{
		/// <summary>
		/// 【非推奨】シーンを読み込む
		/// </summary>
		/// <param name="sceneName"></param>
		public async void LoadSceneAsync(string sceneName)
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			_handle.Enqueue(new LoadSceneAsyncHandle(sceneName));

			await _handle?.RunAll();
			_handle = null;
		}

		/// <summary>
		/// バンドルされたシーンを一括ロード
		/// </summary>
		/// <param name="bundleName"></param>
		public async void LoadSceneBundle(string bundleName)
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
