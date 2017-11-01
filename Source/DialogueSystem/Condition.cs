using System;
using System.Collections.Generic;

namespace PeenTalk
{
	//##########################
	// Class Declaration
	//##########################
	public abstract class Condition
	{
		//=======================
		// Variables
		//=======================	
		public List<Condition> subConditions;
		
		//=======================
		// Constructor
		//=======================
		public Condition()
		{
		}
		
		public Condition( List<Condition> tSubConditions )
		{
			subConditions = tSubConditions;
		}
		
		//=======================
		// Destructor
		//=======================
		~Condition()
		{
			// Clear memory
			if ( subConditions != null )
			{
				subConditions.Clear();
				subConditions = null;
			}
		}
		
		//=======================
		// Validation
		//=======================
		public virtual bool validate( Dialogue tDialogue, Item tItem )
		{
			// Validate conditions (sub conditions treated as Or conditions)
			if ( subConditions != null )
			{
				for ( int i = ( subConditions.Count - 1 ); i >= 0; --i )
				{
					if ( subConditions[i].validate( tDialogue, tItem ) )
					{
						return true;
					}
				}
				
				return false;
			}
			
			return true;
		}
		
		public virtual bool validate( Dialogue tDialogue, Item tItem, Choice tChoice )
		{
			// Validate conditions (sub conditions treated as Or conditions)
			if ( subConditions != null )
			{
				for ( int i = ( subConditions.Count - 1 ); i >= 0; --i )
				{
					if ( subConditions[i].validate( tDialogue, tItem, tChoice ) )
					{
						return true;
					}
				}
				
				return false;
			}
			
			return true;
		}
	}
}