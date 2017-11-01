using UnityEngine;
using System;
using UnityEngine.Events;

namespace PeenTalk
{
	//##########################
	// Class Declaration
	//##########################
	[Serializable]
	public class EventDialogueItemOpen : UnityEvent<Dialogue,Item>
	{
	}
}