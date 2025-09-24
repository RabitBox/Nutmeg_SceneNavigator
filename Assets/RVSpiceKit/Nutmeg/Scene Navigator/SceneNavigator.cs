// zlib/libpng License
//
// Copyright (c) 2025 RabitBox
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the authors be held liable for any damages arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software.
//    If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RV.SpiceKit.Nutmeg
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
		/// 現在の読み込み済シーン一覧
		/// </summary>
		[SerializeField] private List<string> _scenes;

		/// <summary>
		/// 非同期処理ハンドル
		/// </summary>
		private HandleQueue _handle = null;

		/// <summary>
		/// このスクリプトを持つゲームオブジェクトの存在するシーン名
		/// </summary>
		private string Current => this.gameObject.scene.name;

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
			// 常駐シーンは矯正除外
			if (Current == sceneName)
			{
				return true;
			}

			// 常駐リストと一致するものは除外
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
