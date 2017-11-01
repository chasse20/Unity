using System;

namespace PeenTalk
{
	public class ItemTopic : Item
	{
		//=======================
		// Variables
		//=======================	
		public string speaker;
		public string text;
		
		//=======================
		// Constructor
		//=======================
		public ItemTopic() : base()
		{
		}
		
		public ItemTopic( string tSpeaker, string tText ) : base()
		{
			speaker = tSpeaker;
			text = tText;
		}
	}
}