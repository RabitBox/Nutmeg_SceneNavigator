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
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using ZeroMessenger;
using RV.SpiceKit.Nutmeg.Messages;

namespace RV.SpiceKit.Nutmeg
{
	/// <summary>
	/// 読み込み処理の実体
	/// </summary>
	public sealed class SceneFlowUseCase
	{
		private readonly SceneConfigObject _config;
		private readonly LoadingController _loading;
		private string _currentBundleName = null;
		private IReadOnlyList<string> _currentScenes = Array.Empty<string>();

		public SceneFlowUseCase(SceneConfigObject config, LoadingController loading)
		{
			_config = config;
			_loading = loading;
		}

		/// <summary>
		/// 常駐シーンを読み込む
		/// </summary>
		/// <returns></returns>
		public async UniTask InitializeAsync()
		{
			Publish(LoadingPhaseChanged.Phase.Loading);

			var tasks = _config.Permanents
				.Select(s => new AdditiveSceneLoadTask(s))
				.Cast<ILoadingTask>()
				.ToList();
			await _loading.RunAsync(tasks);

			Publish(LoadingPhaseChanged.Phase.Complate);
		}

		/// <summary>
		/// シーンバンドル切り替え
		/// </summary>
		/// <param name="bundleName">バンドル名</param>
		/// <returns></returns>
		public async UniTask LoadBundleAsync(string bundleName)
		{
			// バンドル取得
			var bundle = _config.SceneBundles
				.First(b => b.Name == bundleName);

			// 0. 読み込み済バンドルチェック
			if (_currentBundleName == bundle.Name) {
				return;
			}

			// 1. シーンのアンロード
			Publish(LoadingPhaseChanged.Phase.Unloading);
			var unloadTasks = _currentScenes
			   .Except(_config.Permanents)
			   .Select(s => new SceneUnloadTask(s))
			   .Cast<ILoadingTask>()
			   .ToList();
			if (unloadTasks.Count > 0) {
				await _loading.RunAsync(unloadTasks);
			}

			// 2. 不使用アセットのクリーンアップ
			Publish(LoadingPhaseChanged.Phase.Cleaning);
			await _loading.RunAsync(new ILoadingTask[] {
				new UnloadUnusedAssetsTask()
			});

			// 3. バンドルシーンの読み込み
			Publish(LoadingPhaseChanged.Phase.Loading);
			var loadTasks = bundle.SceneNames
				.Select(s => new AdditiveSceneLoadTask(s))
				.Cast<ILoadingTask>()
				.ToList();
			await _loading.RunAsync(loadTasks);

			// 現在のシーンリストを更新
			_currentBundleName = bundle.Name;
			_currentScenes = bundle.SceneNames;
			Publish(LoadingPhaseChanged.Phase.Complate);
		}

		/// <summary>
		/// 進捗通知
		/// </summary>
		/// <param name="phase"></param>
		private void Publish(LoadingPhaseChanged.Phase phase)
			=> MessageBroker<LoadingPhaseChanged>.Default.Publish( new LoadingPhaseChanged(phase) );
	}
}
