using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RV.SpiceKit.Nutmeg
{
	/// <summary>
	/// ロード状況監視クラス
	/// </summary>
	/// <typeparam name="TTag">アセットに付与するタグ</typeparam>
	public sealed class AddressableLoadMonitor<TTag>
		where TTag : notnull
	{
		private readonly Dictionary<TTag, HashSet<AsyncOperationHandle>> _handles = new();
		private readonly Dictionary<AssetReference, AsyncOperationHandle> _referenceHandles = new();

		public async UniTask LoadAsync<T>(TTag tags, object key, Action<T> onLoaded)
			where T : UnityEngine.Object
		{
			if (onLoaded == null)
				throw new ArgumentNullException(nameof(onLoaded));


		}

		public async UniTask LoadAsync<T>(TTag tags, AssetReference reference, Action<T> onLoaded)
			where T : UnityEngine.Object
		{
			if (onLoaded == null)
				throw new ArgumentNullException(nameof(onLoaded));


		}

		public void Release(TTag releaseTag)
		{

		}

		public void ReleaseAll()
		{
			foreach (var list in _handles.Values)
			{
				foreach (var handle in list)
				{
					Addressables.Release(handle);
				}
			}

			_handles.Clear();
			_referenceHandles.Clear();
		}

		//--------------------------------------------------
		// internal
		//--------------------------------------------------
		private void Register(TTag tags, AsyncOperationHandle handle)
		{
			if (!_handles.TryGetValue(tags, out var list))
			{
				list = new HashSet<AsyncOperationHandle>();
				_handles[tags] = list;
			}

			list.Add(handle);
		}

		private void RemoveReferenceHandle(AsyncOperationHandle handle)
		{
			var pair = _referenceHandles.FirstOrDefault(p => p.Value.Equals(handle));

			if (!pair.Equals(default(KeyValuePair<AssetReference, AsyncOperationHandle>)))
			{
				_referenceHandles.Remove(pair.Key);
			}
		}

		private static bool HasAnyFlag<T>(T value, T flags)
			where T : Enum
		{
			ulong v = Convert.ToUInt64(value);
			ulong f = Convert.ToUInt64(flags);
			return (v & f) != 0;
		}

		private static bool IsSingleFlag<T>(T tag)
			where T : Enum
		{
			ulong v = Convert.ToUInt64(tag);
			return v != 0 && (v & (v - 1)) == 0;
		}
	}
}
