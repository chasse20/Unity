using System;

namespace PeenTalk
{
	public class ItemCompare : Item
	{
		//=======================
		// Variables
		//=======================	
		public string bookmarkTrue;
		public string bookmarkFalse;
		
		//=======================
		// Constructor
		//=======================
		public ItemCompare() : base()
		{
		}
		
		public ItemCompare( string tBookmarkTrue, string tBookmarkFalse ) : base()
		{
			bookmarkTrue = tBookmarkTrue;
			bookmarkFalse = tBookmarkFalse;
		}

		//=======================
		// Flow
		//=======================
		public override bool validate( Dialogue tDialogue )
		{
			return true;
		}
		
		public override void open( Dialogue tDialogue )
		{
			// Go to true bookmark if any condition is true
			if ( conditions != null )
			{
				for ( int i = ( conditions.Count - 1 ); i >= 0; --i )
				{
					if ( conditions[i].validate( tDialogue, this ) )
					{
						tDialogue.setBookmark( bookmarkTrue );
						return;
					}
				}
			}
			
			// Go to false if exists, otherwise go to next item
			if ( String.IsNullOrEmpty( bookmarkFalse ) )
			{
				tDialogue.setItem( nextItem );
			}
			else
			{
				tDialogue.setBookmark( bookmarkFalse );
			}
		}
		
		public override void close( Dialogue tDialogue )
		{
		}
	}
}