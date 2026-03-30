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
using UnityEngine;
using SpiceKit.Nutmeg;
using SpiceKit.Nutmeg.Data;
using SpiceKit.Nutmeg.Messages;

/// <summary>
/// シーン読み込みの中継クラス
/// </summary>
public class SceneFlowController : MonoBehaviour
{
	[SerializeField]
	private ContextSceneList _context;

	private int _prev;


	public void Awake()
	{
		_prev = 0;
		SceneService.Load(_context.Default);
	}

	public void Load(int id)
	{
		Debug.Log($"Load to {id}");
		if (_prev == id) return;
		if (_context.TryGetSceneName(GetName(id), out string loadName))
		{
			if (_context.TryGetSceneName(GetName(_prev), out string unloadName)) SceneService.Unload(unloadName, false);
			SceneService.Load(loadName);
			_prev = id;
		}
	}

	private string GetName(int id)
	{
		switch (id)
		{
			case 0: 
				return "A";
			case 1: 
				return "B";
		}
		return "";
	}
}