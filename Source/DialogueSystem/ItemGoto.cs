using System;

namespace PeenTalk
{
	public class ItemGoto : Item
	{
		//=======================
		// Variables
		//=======================	
		public string bookmark;
		
		//=======================
		// Constructor
		//=======================
		public ItemGoto() : base()
		{
		}
		
		public ItemGoto( string tBookmark ) : base()
		{
			bookmark = tBookmark;
		}

		//=======================
		// Flow
		//=======================		
		public override void open( Dialogue tDialogue )
		{
			// Goes to bookmark
			if ( tDialogue != null )
			{
				tDialogue.setBookmark( bookmark );
			}
		}
		
		public override void close( Dialogue tDialogue )
		{
		}
	}
}