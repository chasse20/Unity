using UnityEngine;
using System;
using System.Collections.Generic;

namespace WaltsGame
{
	//##########################
	// Class Declaration
	//##########################
	public class Level : MonoBehaviour
	{	
		//=======================
		// Variables
		//=======================
		protected HashSet<Level> _required;
		protected HashSet<Level> _requiredBy;
		
		//=======================
		// Initialization
		//=======================		
		protected virtual void Start()
		{
			// Notify Game
			Game tempGame = Game.GetInstance<Game>();
			if ( !ReferenceEquals( tempGame, null ) )
			{
				tempGame.addLevel( this );
			}
		}

		//=======================
		// Deconstruction
		//=======================
		protected virtual void OnDestroy()
		{
			// Notify Game
			Game tempGame = Game.instance;
			if ( !ReferenceEquals( tempGame, null ) )
			{
				tempGame.removeLevel( this );
			}
			
			// Clear
			if ( _required != null )
			{
				List<Level> tempList = new List<Level>( _required );
				for ( int i = ( tempList.Count - 1 ); i >= 0; --i )
				{
					removeRequired( tempList[i] );
				}
				
				if ( _required != null )
				{
					_required.Clear();
					_required = null;
				}
			}
			
			if ( _requiredBy != null )
			{
				List<Level> tempList = new List<Level>( _requiredBy );
				for ( int i = ( tempList.Count - 1 ); i >= 0; --i )
				{
					removeRequiredBy( tempList[i] );
				}
				
				if ( _requiredBy != null )
				{
					_requiredBy.Clear();
					_requiredBy = null;
				}
			}
		}
		
		//=======================
		// Reset
		//=======================
		public virtual void reset()
		{
		}
		
		//=======================
		// Required
		//=======================	
		public virtual List<Level> required
		{
			get
			{
				return _required == null ? null : new List<Level>( _required );
			}
		}
		
		public virtual bool hasRequired( Level tRequired )
		{
			return _required != null && _required.Contains( tRequired );
		}
		
		public virtual bool addRequired( Level tLevel, bool tIsRequiredNotified = true )
		{
			if ( tLevel != null )
			{
				if ( _required == null )
				{
					_required = new HashSet<Level>();
				}
				
				if ( _required.Add( tLevel ) )
				{
					effectsAddRequired( tLevel, tIsRequiredNotified );
					
					return true;
				}
			}
			
			return false;
		}
		
		protected virtual void effectsAddRequired( Level tLevel, bool tIsRequiredNotified )
		{
			// Notify required
			if ( tIsRequiredNotified )
			{
				tLevel.addRequiredBy( this, false );
			}
		}
		
		public virtual bool removeRequired( Level tLevel, bool tIsRequiredNotified = true )
		{
			if ( _required != null && _required.Remove( tLevel ) )
			{
				if ( _required.Count == 0 )
				{
					_required = null;
				}
				
				effectsRemoveRequired( tLevel, tIsRequiredNotified );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveRequired( Level tLevel, bool tIsRequiredNotified )
		{
			// Notify required
			if ( tIsRequiredNotified && tLevel != null )
			{
				tLevel.removeRequiredBy( this, false );
			}
		}
		
		//=======================
		// Required By
		//=======================	
		public virtual List<Level> requiredBy
		{
			get
			{
				return _requiredBy == null ? null : new List<Level>( _requiredBy );
			}
		}
		
		public virtual bool hasRequiredBy( Level tRequiredBy )
		{
			return _requiredBy != null && _requiredBy.Contains( tRequiredBy );
		}
		
		public virtual bool addRequiredBy( Level tLevel, bool tIsRequiredByNotified = true )
		{
			if ( tLevel != null )
			{
				if ( _requiredBy == null )
				{
					_requiredBy = new HashSet<Level>();
				}
				
				if ( _requiredBy.Add( tLevel ) )
				{
					effectsAddRequiredBy( tLevel, tIsRequiredByNotified );
					
					return true;
				}
			}
			
			return false;
		}
		
		protected virtual void effectsAddRequiredBy( Level tLevel, bool tIsRequiredByNotified )
		{
			// Notify requiredby
			if ( tIsRequiredByNotified )
			{
				tLevel.addRequired( this, false );
			}
		}
		
		public virtual bool removeRequiredBy( Level tLevel, bool tIsRequiredByNotified = true )
		{
			if ( _requiredBy != null && _requiredBy.Remove( tLevel ) )
			{
				if ( _requiredBy.Count == 0 )
				{
					_requiredBy = null;
				}
				
				// Effects
				effectsRemoveRequiredBy( tLevel, tIsRequiredByNotified );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveRequiredBy( Level tLevel, bool tIsRequiredByNotified )
		{
			// Notify requiredby
			if ( tLevel != null && tIsRequiredByNotified )
			{
				tLevel.removeRequired( this, false );
			}
			
			// Check Game to determine if this Level should be destroyed if it's no longer required
			if ( _requiredBy == null )
			{
				Game tempGame = Game.instance;
				if ( ReferenceEquals( tempGame, null ) || !tempGame.checkIfSceneRequired( gameObject.scene.name ) )
				{
					Destroy( gameObject ); // may have to delay this by a frame
				}
			}
		}
	}
}