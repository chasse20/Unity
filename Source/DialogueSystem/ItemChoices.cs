using System;
using System.Collections.Generic;

namespace PeenTalk
{
	public class ItemChoices : Item
	{
		//=======================
		// Variables
		//=======================	
		public List<Choice> choices;
		public HashSet<Choice> choicesValidated;
		
		//=======================
		// Constructor
		//=======================
		public ItemChoices() : base()
		{
		}
		
		public ItemChoices( List<Choice> tChoices ) : base()
		{
			choices = tChoices;
		}
		
		//=======================
		// Destructor
		//=======================
		~ItemChoices()
		{
			// Clear memory
			if ( choices != null )
			{
				choices.Clear();
				choices = null;
			}
		}
		
		//=======================
		// Flow
		//=======================
		public override bool validate( Dialogue tDialogue )
		{
			// Validate choices
			if ( base.validate( tDialogue ) )
			{
				if ( choices == null )
				{
					choicesValidated = null;
				}
				else
				{
					choicesValidated = new HashSet<Choice>();
					for ( int i = ( choices.Count - 1 ); i >= 0; --i )
					{
						if ( choices[i].validate( tDialogue, this ) )
						{
							choicesValidated.Add( choices[i] );
						}
					}
				}
			
				return true;
			}
			
			return false;
		}
		
		//=======================
		// Selection
		//=======================
		public virtual void choose( Dialogue tDialogue, int tChoice )
		{
			if ( tChoice >= 0 && choices != null && tChoice < choices.Count )
			{
				choose( tDialogue, choices[ tChoice ] );
			}
		}
		
		public virtual void choose( Dialogue tDialogue, Choice tChoice )
		{
			if ( tChoice != null && choicesValidated != null && choicesValidated.Contains( tChoice ) )
			{
				tChoice.choose( tDialogue, this );
			}
		}
	}
}