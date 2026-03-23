using System.Collections.Generic;
using UnityEngine;


namespace SpiceKit.Nutmeg.SO
{
	[CreateAssetMenu(fileName = "LayerSceneList", menuName = "Nutmeg/Scriptable Objects/LayerSceneList")]
	public class LayerSceneList : ScriptableObject
	{
		[field : SerializeField]
		public List<string> SceneList { get; private set; }
	}
}
