using UnityEngine;
using System;

namespace WaltsGame
{
	//##########################
	// Class Declaration
	//##########################
	[CreateAssetMenu( fileName = "SceneName", menuName = "Scene Settings" )]
	public class SceneSettings : ScriptableObject
	{
		//=======================
		// Variables
		//=======================
		public bool isDestroyedWhenNotRequired;
		public bool isLocal;
		public string[] requiredScenes;
	}
}