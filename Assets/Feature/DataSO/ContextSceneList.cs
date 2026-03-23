using System.Collections.Generic;
using UnityEngine;

namespace SpiceKit.Nutmeg.SO
{
	[CreateAssetMenu(fileName = "ContextSceneList", menuName = "Nutmeg/Scriptable Objects/ContextSceneList")]
	public class ContextSceneList : ScriptableObject
	{
		[System.Serializable]
		public struct ContextPair
		{
			public string Name;
			public string Value;
		}

		[field: SerializeField]
		public List<ContextPair> SceneList { get; private set; }
	}
}

