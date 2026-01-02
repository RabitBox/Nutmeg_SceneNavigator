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
using System.Collections.Generic;
using System.Linq;

namespace RV.SpiceKit.Nutmeg
{
	public class LoadingController
	{
		public async UniTask RunAsync(IReadOnlyList<ILoadingTask> tasks)
		{
			// 全タスクを実行
			var runners = tasks.Select(task => task.RunAsync()).ToArray();

			// タスク進捗を監視
			while (!runners.All(task => task.Status.IsCompleted()))
			{
				// 全体の進行の平均を通知
				//float progress = tasks.Count > 0 ? tasks.Average(t => t.Progress) : 1f;

				// 待機
				await UniTask.Yield();
			}

			// 全タスク完了をチェック
			await UniTask.WhenAll( runners );
		}
	}
}