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
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RV.SpiceKit.Nutmeg
{
	/// <summary>
	/// ロード状況監視クラス
	/// </summary>
	/// <typeparam name="TTag">アセットに付与するタグ</typeparam>
	public sealed partial class AddressableLoadMonitor<TTag>
		where TTag : Enum
	{
		/// <summary>
		/// タグ別ロードハンドル
		/// </summary>
		private readonly Dictionary<TTag, Dictionary<string, AsyncOperationHandle>> _entries = new();

		// ============================
		// Load
		// ============================

		/// <summary>
		/// アセット読み込み
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="tags"></param>
		/// <param name="key"></param>
		/// <param name="onLoaded"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public async UniTask LoadAsync<T>(
			TTag tags,
			string key,
			Action<T> onLoaded)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException(nameof(key));
			if (onLoaded == null)
				throw new ArgumentNullException(nameof(onLoaded));

			// タグチェック
			if ( !_entries.TryGetValue(tags, out var dict) )
			{
				dict = new Dictionary<string, AsyncOperationHandle>();
				_entries[tags] = dict;
			}

			// キーチェック
			if (dict.TryGetValue(key, out var operationHandle))
			{
				// ロード済チェック
				if (operationHandle.Status == AsyncOperationStatus.Succeeded)
				{
					onLoaded.Invoke((T)operationHandle.Result);
					return;
				}

				// ロード中ならば、完了時処理に追加
				operationHandle.Completed += obj => {
					if(obj.Status == AsyncOperationStatus.Succeeded) onLoaded.Invoke((T)obj.Result);
				};
				await operationHandle.ToUniTask();   // 敢えて待機したい場合の対応
				return;
			}

			// 新規ロード
			var handle = Addressables.LoadAssetAsync<T>(key);
			dict[key] = handle;
			handle.Completed += obj => {
				if(obj.Status == AsyncOperationStatus.Succeeded) onLoaded(obj.Result); 
			};

			await handle.ToUniTask();   // 敢えて待機したい場合の対応
		}

		// ============================
		// Release
		// ============================

		/// <summary>
		/// 指定したタグのアセットを解放
		/// </summary>
		/// <param name="tags"></param>
		public void Release(TTag tags)
		{
			// タグチェック
			if (!_entries.TryGetValue(tags, out var dict))
				return;

			foreach (var handle in dict.Values)
			{
				if (handle.IsValid())
					Addressables.Release(handle);
			}

			dict.Clear();
			_entries.Remove(tags);
		}

		/// <summary>
		/// 保持していたアセットを全て解放
		/// </summary>
		public void ReleaseAll()
		{
			foreach (var dict in _entries.Values)
			{
				foreach (var handle in dict.Values)
				{
					if (handle.IsValid())
						Addressables.Release(handle);
				}
			}

			_entries.Clear();
		}

		// ============================
		// 状態確認
		// ============================

		/// <summary>
		/// ロードが完了していないハンドルがあるか
		/// </summary>
		/// <param name="tags"></param>
		/// <returns>存在する場合はtrue</returns>
		public bool HasLoading(TTag tags)
		{
			// タグチェック
			if (!_entries.TryGetValue(tags, out var dict))
				return false;

			foreach (var handle in dict.Values)
			{
				if (!handle.IsDone)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 指定したタグの未完了のローディングを待つ
		/// </summary>
		/// <param name="tags"></param>
		public async UniTask WaitForTag(TTag tags)
		{
			// タグチェック
			if (!_entries.TryGetValue(tags, out var dict))
				return;

			// 未完了のローディングタスクを積む
			var tasks = new List<UniTask>();
			foreach (var handle in dict.Values)
			{
				if (!handle.IsDone)
					tasks.Add(handle.ToUniTask());
			}

			// あれば待機する
			if (tasks.Count > 0)
				await UniTask.WhenAll(tasks);
		}

		/// <summary>
		/// ローディングの完了状況を調べる
		/// </summary>
		/// <param name="tags"></param>
		/// <returns>0~1のfloat値</returns>
		public float GetProgress(TTag tags)
		{
			// タグチェック
			if (!_entries.TryGetValue(tags, out var dict))
				return 1.0f;

			int total = dict.Count;
			if (total == 0)
				return 1.0f;

			int completed = 0;

			foreach (var handle in dict.Values)
			{
				if (handle.IsDone)
					completed++;
			}

			return (float)completed / total;
		}
	}
}
