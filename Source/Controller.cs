using UnityEngine;
using System;
using System.Collections.Generic;

namespace WaltsGame
{
	//##########################
	// Class Declaration
	//##########################
	public class Controller : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		[SerializeField]
		protected View _view;
		[SerializeField]
		protected List<Pawn> _pawns;
		protected int _localIndex = -1;
		/*[SerializeField]
		protected AI _ai;*/
		
		//=======================
		// Initialization
		//=======================		
		protected virtual void Start()
		{
			// Effects
			if ( _pawns != null )
			{
				for ( int i = ( _pawns.Count - 1 ); i >= 0; --i )
				{
					effectsAddPawn( _pawns[i], i );
				}
			}
			effectsView( _view );
		}
		
		//=======================
		// Deconstruction
		//=======================
		protected virtual void OnDestroy()
		{
			// Try to remove from Game's Local Controllers
			Game tempGame = Game.instance;
			if ( !ReferenceEquals( tempGame, null ) )
			{
				tempGame.removeLocalController( this );
			}
		
			// Clear
			setView( null );
			if ( _pawns != null )
			{
				for ( int i = ( _pawns.Count - 1 ); i >= 0; --i )
				{
					removePawn( i );
				}
				
				if ( _pawns != null )
				{
					_pawns.Clear();
					_pawns = null;
				}
			}
		}
		
		//=======================
		// View
		//=======================
		public virtual View view
		{
			get
			{
				return _view;
			}
			set
			{
				setView( value );
			}
		}
		
		public virtual bool setView( View tView )
		{
			if ( tView != _view )
			{
				View tempOld = _view;
				_view = tView;
				effectsView( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsView( View tOld )
		{
			if ( _view != tOld )
			{
				// Remove from old
				if ( tOld != null )
				{
					tOld.removeViewer( this );
				}
				
				// Add to new
				if ( _view != null )
				{
					_view.addViewer( this );
				}
			}
		}
		
		//=======================
		// Local Index
		//=======================
		public int localIndex
		{
			get
			{
				return _localIndex;
			}
		}
		
		public virtual bool setLocalIndex( Game tGame, int tIndex )
		{
			// Remove from View
			int tempOld = _localIndex;
			if ( tIndex < 0 )
			{
				_localIndex = tIndex;
				if ( _view != null && tempOld >= 0 )
				{
					_view.removeSlotIndex( (uint)tempOld );
				}
				
				return true;
			}
			// Add to View
			else if ( tGame != null )
			{
				Controller tempController;
				if ( tGame.getLocalController( tIndex, out tempController ) && tempController == this )
				{
					_localIndex = tIndex;
					if ( _view != null )
					{
						if ( tempOld >= 0 )
						{
							_view.removeSlotIndex( (uint)tempOld );
						}
						_view.addSlotIndex( (uint)_localIndex );
					}
					
					return true;
				}
			}
			
			return false;
		}
		
		//=======================
		// Pawns
		//=======================
		public virtual List<Pawn> pawns
		{
			get
			{
				return _pawns == null ? null : new List<Pawn>( _pawns );
			}
		}
		
		public virtual bool getPawn( int tIndex, out Pawn tPawn )
		{
			if ( tIndex >= 0 && _pawns != null && tIndex < _pawns.Count )
			{
				tPawn = _pawns[ tIndex ];
				return true;
			}
			
			tPawn = null;
			return false;
		}
		
		public virtual bool addPawn( Pawn tPawn, bool tIsPawnNotified = true )
		{
			if ( tPawn != null )
			{
				if ( _pawns == null )
				{
					_pawns = new List<Pawn>();
					_pawns.Add( tPawn );
					effectsAddPawn( tPawn, 0, tIsPawnNotified );
					
					return true;
				}
				else if ( !_pawns.Contains( tPawn ) ) // add only if doesn't exist
				{
					_pawns.Add( tPawn );
					effectsAddPawn( tPawn, ( _pawns.Count - 1 ), tIsPawnNotified );
				
					return true;
				}
			}
		
			return false;
		}
		
		protected virtual void effectsAddPawn( Pawn tPawn, int tIndex, bool tIsPawnNotified = true )
		{
			// Notify Pawn
			if ( tIsPawnNotified )
			{
				tPawn.addController( this, false );
			}
		}
		
		public virtual bool removePawn( Pawn tPawn, bool tIsPawnNotified = true )
		{
			if ( _pawns != null )
			{
				for ( int i = ( _pawns.Count - 1 ); i >= 0; --i )
				{
					if ( _pawns[i] == tPawn )
					{
						_pawns.RemoveAt( i );
						if ( _pawns.Count == 0 )
						{
							_pawns = null;
						}
						
						effectsRemovePawn( tPawn, i, tIsPawnNotified );
						
						return true;
					}
				}
			}
			
			return false;
		}
		
		public virtual bool removePawn( int tIndex, bool tIsPawnNotified = true )
		{
			if ( tIndex >= 0 && _pawns != null && tIndex < _pawns.Count )
			{
				Pawn tempPawn = _pawns[ tIndex ];
				_pawns.RemoveAt( tIndex );
				if ( _pawns.Count == 0 )
				{
					_pawns = null;
				}
				
				effectsRemovePawn( tempPawn, tIndex, tIsPawnNotified );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemovePawn( Pawn tPawn, int tIndex, bool tIsPawnNotified = true )
		{
			// Notify Pawn
			if ( tIsPawnNotified && tPawn != null )
			{
				tPawn.removeController( this, false );
			}
		}
	}
}