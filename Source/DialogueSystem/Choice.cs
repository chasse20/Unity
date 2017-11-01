using System;
using System.Collections.Generic;

namespace PeenTalk
{
	public class Choice
	{
		//=======================
		// Variables
		//=======================
		public string text;
		public string bookmark;
		public List<Condition> conditions;
		
		//=======================
		// Constructor
		//=======================
		public Choice()
		{
		}
		
		public Choice( string tText, string tBookmark )
		{
			text = tText;
			bookmark = tBookmark;
		}
		
		//=======================
		// Destructor
		//=======================
		~Choice()
		{
			// Clear memory			
			if ( conditions != null )
			{
				conditions.Clear();
				conditions = null;
			}
		}
		
		//=======================
		// Flow
		//=======================
		public virtual bool validate( Dialogue tDialogue, ItemChoices tChoices )
		{
			// Validate conditions
			if ( conditions != null )
			{
				for ( int i = ( conditions.Count - 1 ); i >= 0; --i )
				{
					if ( conditions[i].validate( tDialogue, tChoices, this ) )
					{
						return true;
					}
				}
				
				return false;
			}
			
			return true;
		}
		
		//=======================
		// Selection
		//=======================
		public virtual void choose( Dialogue tDialogue, ItemChoices tChoices )
		{
			// Goto bookmark
			if ( tDialogue != null )
			{
				tDialogue.setBookmark( bookmark );
			}
		}
	}
}