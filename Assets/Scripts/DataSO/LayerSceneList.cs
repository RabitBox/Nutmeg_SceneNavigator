using System.Collections.Generic;
using UnityEngine;


namespace RV.SpiceKit.Nutmeg.SO
{
	[CreateAssetMenu(fileName = "LayerSceneList", menuName = "Nutmeg/Scriptable Objects/LayerSceneList")]
	public class LayerSceneList : ScriptableObject
	{
		[field : SerializeField]
		public List<string> SceneList { get; private set; }
	}
}
