using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nutmeg
{
	public partial class SceneNavigator : MonoBehaviour
	{
		public SceneNavigator Instance { get; private set; }

		[Header("Assets")]
		/// <summary>
		/// シーンコンフィグデータ
		/// </summary>
		[SerializeField] private SceneConfigObject _config;

		[Header("Runtime")]
		/// <summary>
		/// 読み込み済シーン一覧
		/// </summary>
		[SerializeField] private List<string> _scenes;

		private void Awake()
		{
			// シングルトン化
			Instance = this;

			// シーン一覧を更新
			RefreshLoadedScenes();
		}

		/// <summary>
		/// 読み込み済みシーン一覧を更新
		/// </summary>
		private void RefreshLoadedScenes()
		{
			_scenes.Clear();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);
				if (this.gameObject.scene.name != scene.name)
				{
					_scenes.Add(scene.name);
				}
			}
		}
	}
}
