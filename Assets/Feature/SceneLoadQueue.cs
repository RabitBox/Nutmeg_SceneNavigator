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
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using ZeroMessenger;
using SpiceKit.Nutmeg.Messages;

namespace SpiceKit.Nutmeg
{
	public class SceneLoadQueue
	{
		private readonly Queue<SceneCommand> _queue = new();
		private bool _isProcessing = false;

		public void Load(string sceneName)
			=> Enqueue(new SceneCommand(SceneCommandType.Load, sceneName));

		public void Unload(string sceneName)
			=> Enqueue(new SceneCommand(SceneCommandType.Unload, sceneName));

		/// <summary>
		/// ロード / アンロード コマンドのスタック
		/// </summary>
		/// <param name="command"></param>
		private void Enqueue(SceneCommand command)
		{
			_queue.Enqueue(command);

			if (!_isProcessing)
			{
				ProcessQueueAsync().Forget();
			}
		}
		
		/// <summary>
		/// 非同期プロセス
		/// </summary>
		/// <returns></returns>
		private async UniTask ProcessQueueAsync()
		{
			bool needsRefresh = false;

			_isProcessing = true;
			PublishMessage(SceneQueueEvent.ProcessType.Started);

			while (_queue.Count > 0)
			{
				var command = _queue.Dequeue();

				switch (command.Type)
				{
					case SceneCommandType.Load:
						PublishMessage(SceneQueueEvent.ProcessType.Load);
						await LoadAsync( command.SceneName );
						break;

					case SceneCommandType.Unload:
						PublishMessage(SceneQueueEvent.ProcessType.Unload);
						await UnloadAsync( command.SceneName );
						needsRefresh = true;
						break;
				}
			}

			if (needsRefresh)
			{
				PublishMessage(SceneQueueEvent.ProcessType.Refresh);
				await RefreshAssets();
			}

			_isProcessing = false;
			PublishMessage(SceneQueueEvent.ProcessType.Complated);
		}

		/// <summary>
		/// 指定した名前のシーンを読み込む
		/// </summary>
		/// <param name="sceneName"></param>
		/// <returns></returns>
		private async UniTask LoadAsync(string sceneName)
			=> await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();

		/// <summary>
		/// 指定した名前のシーンをアンロードする
		/// </summary>
		/// <param name="sceneName"></param>
		/// <returns></returns>
		private async UniTask UnloadAsync(string sceneName)
			=> await SceneManager.UnloadSceneAsync(sceneName).ToUniTask();

		/// <summary>
		/// 使用されていないアセットのアンロード
		/// </summary>
		/// <returns></returns>
		private async UniTask RefreshAssets()
			=> await Resources.UnloadUnusedAssets().ToUniTask();

		/// <summary>
		/// 外部に読み込みステータスを通知
		/// </summary>
		/// <param name="type"></param>
		private void PublishMessage(SceneQueueEvent.ProcessType type)
			=> MessageBroker<SceneQueueEvent>.Default.Publish(new SceneQueueEvent(type));
	}

	public enum SceneCommandType
	{
		Load,
		Unload
	}

	public struct SceneCommand
	{
		public SceneCommandType Type { get; }
		public string SceneName { get; }

		public SceneCommand( SceneCommandType type, string sceneName )
		{
			Type = type;
			SceneName = sceneName;
		}
	}
}
