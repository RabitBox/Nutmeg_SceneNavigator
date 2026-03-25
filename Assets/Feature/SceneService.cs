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

namespace SpiceKit.Nutmeg
{
	public class SceneService
	{
		/// <summary>
		/// シーンロード処理を管理する内部インスタンス
		/// </summary>
		private static ISceneLoadQueue _instance;

		/// <summary>
		/// SceneLoadQueue のインスタンスを取得する
		/// null の場合は新規生成（Lazy初期化）
		/// </summary>
		private static ISceneLoadQueue Instance => _instance ??= new SceneLoadQueue();

		//==================================================
		// Public API
		//==================================================

		/// <summary>
		/// 指定したシーンをロードする
		/// </summary>
		/// <param name="sceneName"></param>
		public static void Load(string sceneName)
			=> Instance.Load( sceneName );

		/// <summary>
		/// 複数のシーンをまとめてロードする
		/// </summary>
		/// <param name="sceneList"></param>
		public static void LoadBatch(SO.LayerSceneList sceneList)
		{
			foreach (var sceneName in sceneList.SceneList)
			{
				Instance.Load(sceneName, false);
			}
			Instance.Process();
		}

		/// <summary>
		/// 指定したシーンをアンロードする
		/// </summary>
		/// <param name="sceneName"></param>
		public static void Unload(string sceneName)
			=> Instance.Unload(sceneName);

		/// <summary>
		/// 複数のシーンをまとめてアンロードする
		/// </summary>
		/// <param name="sceneList"></param>
		public static void UnloadBatch(SO.LayerSceneList sceneList)
		{
			foreach (var sceneName in sceneList.SceneList)
			{
				Instance.Unload(sceneName, false);
			}
			Instance.Process();
		}
	}
}
