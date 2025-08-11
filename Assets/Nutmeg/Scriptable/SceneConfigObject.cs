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

		[field: SerializeField, Tooltip("実行中、永続的に残るシーン")]
		public List<string> Permanents;

		[field: SerializeField, Tooltip("まとめてロードしたいシーンの組み合わせ")]
		public List<SceneBundle> SceneBundles;
	}
}
