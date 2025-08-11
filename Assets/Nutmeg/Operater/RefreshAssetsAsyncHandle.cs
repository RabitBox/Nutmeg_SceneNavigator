using System.Threading.Tasks;
using UnityEngine;

namespace Nutmeg
{
	/// <summary>
	/// 不使用アセットの開放
	/// </summary>
	public class RefreshAssetsAsyncHandle : IHandle
	{
		public Task Run()
		{
			var tcs = new TaskCompletionSource<bool>();

			var operation = Resources.UnloadUnusedAssets();
			operation.completed += _ => tcs.SetResult(true);

			return tcs.Task;
		}
	}
}

