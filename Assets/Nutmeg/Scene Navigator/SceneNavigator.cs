using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nutmeg
{
	public partial class SceneNavigator : MonoBehaviour
	{
		public static SceneNavigator Instance { get; private set; }

		[Header("Assets")]
		/// <summary>
		/// シーンコンフィグデータ
		/// </summary>
		[SerializeField] private SceneConfigObject _config;

		[Header("Runtime")]
		/// <summary>
		/// 読み込み済シーン一覧
		/// </summary>
		[SerializeField] private List<string> _scenes;

		/// <summary>
		/// 非同期処理ハンドル
		/// </summary>
		private HandleQueue _handle = null;

		private void Awake()
		{
			// シングルトン化
			Instance = this;

			// シーン一覧を更新
			RefreshLoadedScenes();
		}

		/// <summary>
		/// 読み込み済みシーン一覧を更新
		/// 永続シーンは除外
		/// </summary>
		public void RefreshLoadedScenes()
		{
			_scenes.Clear();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);
				if (IsPermanent(scene.name) == false)
				{
					_scenes.Add(scene.name);
				}
			}
		}

		/// <summary>
		/// 常駐シーンかを調べる
		/// </summary>
		/// <param name="sceneName"></param>
		/// <returns></returns>
		private bool IsPermanent(string sceneName)
		{
			if (this.gameObject.scene.name == sceneName)
			{
				return true;
			}

			foreach(var item in _config.Permanents)
			{
				if (item == sceneName)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 未使用のアセットを開放する
		/// </summary>
		public async Task RefreshAssetsAsync()
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			_handle.Enqueue(new RefreshAssetsAsyncHandle());

			await _handle?.RunAll();
			_handle = null;
		}

		/// <summary>
		/// シーンの切り替えを行う
		/// </summary>
		public async void SwitchSceneAsync(string bundleName)
		{
			await UnloadSceneAllAsync();
			await LoadSceneBundleAsync(bundleName);
			await RefreshAssetsAsync();
			RefreshLoadedScenes();
		}
	}
}
