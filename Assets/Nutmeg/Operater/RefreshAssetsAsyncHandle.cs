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

