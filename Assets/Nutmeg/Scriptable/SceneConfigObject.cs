using System.Collections.Generic;
using UnityEngine;

namespace Nutmeg
{
	[CreateAssetMenu(fileName = "SceneConfig", menuName = "Nutmeg/SceneConfigObject")]
	public class SceneConfigObject : ScriptableObject
	{
		[System.Serializable]
		public struct SceneBundle
		{
			public string Name;
			public List<string> SceneNames;
		}

		/// <summary>
		/// 実行中永続的に残るシーン一覧
		/// </summary>
		[field: SerializeField] public List<string> Permanents;

		/// <summary>
		/// シーンの組み合わせ一覧
		/// </summary>
		[field: SerializeField] public List<SceneBundle> SceneBundles;
	}
}
