using System;
using System.Collections.Generic;

namespace PeenTalk
{
	//##########################
	// Class Declaration
	//##########################
	public class Item
	{
		//=======================
		// Variables
		//=======================
		public float duration;
		public Item nextItem; // forward list
		public List<Condition> conditions;
		
		//=======================
		// Constructor
		//=======================
		public Item()
		{
		}
		
		//=======================
		// Destructor
		//=======================
		~Item()
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
		public virtual bool validate( Dialogue tDialogue )
		{
			// Validate conditions
			if ( conditions != null )
			{
				for ( int i = ( conditions.Count - 1 ); i >= 0; --i )
				{
					if ( conditions[i].validate( tDialogue, this ) )
					{
						return true;
					}
				}
				
				return false;
			}
			
			return true;
		}
		
		public virtual void open( Dialogue tDialogue )
		{
		}
		
		public virtual void close( Dialogue tDialogue )
		{
			// Goes to next Item
			if ( tDialogue != null )
			{
				tDialogue.setItem( nextItem );
			}
		}
	}
}