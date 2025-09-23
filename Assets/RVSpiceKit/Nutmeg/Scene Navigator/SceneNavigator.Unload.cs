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

namespace RVSpiceKit.Nutmeg
{
	public partial class SceneNavigator
	{
		/// <summary>
		/// 【非推奨】シーンをアンロードする
		/// </summary>
		/// <param name="sceneName"></param>
		public async Task UnloadSceneAsync(string sceneName)
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			_handle.Enqueue(new UnloadSceneAsyncHandle(sceneName));

			await _handle?.RunAll();
			_handle = null;
		}

		/// <summary>
		/// 現在のシーンをすべてアンロードする
		/// </summary>
		public async Task UnloadSceneAllAsync()
		{
			if (_handle is not null) return; // 実行中

			_handle = new HandleQueue();
			foreach (var scene in _scenes)
			{
				_handle.Enqueue(new UnloadSceneAsyncHandle(name));
			}

			await _handle?.RunAll();
			_handle = null;
		}
	}
}
