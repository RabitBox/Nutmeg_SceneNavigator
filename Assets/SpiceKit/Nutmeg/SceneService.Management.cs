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
using UnityEngine.SceneManagement;

namespace SpiceKit.Nutmeg
{
	public partial class SceneService
	{
		/// <summary>
		/// 既にロード済のシーンかどうかを調べる
		/// </summary>
		/// <param name="name">調べたいシーン名</param>
		/// <returns>ロード済なら true を返す</returns>
		public static bool AlreadyLoadedScene(string name)
		{
			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				if (SceneManager.GetSceneAt(i).name.Equals(name))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 現在ロード済のシーン数
		/// </summary>
		public static int SceneCount => SceneManager.sceneCount;
	}
}