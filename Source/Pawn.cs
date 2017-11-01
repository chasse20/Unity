using UnityEngine;
using System;
using System.Collections.Generic;

namespace WaltsGame
{
	//##########################
	// Class Declaration
	//##########################
	public class Pawn : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		protected HashSet<Controller> _controllers;
		
		//=======================
		// Destruction
		//=======================
		protected virtual void OnDestroy()
		{
			// Clear Controllers
			if ( _controllers != null )
			{
				List<Controller> tempList = new List<Controller>( _controllers );
				for ( int i = ( tempList.Count - 1 ); i >= 0; --i )
				{
					removeController( tempList[i] );
				}
				
				if ( _controllers != null )
				{
					_controllers.Clear();
					_controllers = null;
				}
			}
		}
		
		//=======================
		// Controllers
		//=======================
		public virtual List<Controller> controllers
		{
			get
			{
				return _controllers == null ? null : new List<Controller>( _controllers );
			}
		}
		
		public virtual bool hasController( Controller tController )
		{
			return _controllers != null && _controllers.Contains( tController );
		}
		
		public virtual bool addController( Controller tController, bool tIsControllerNotified = true )
		{
			if ( tController != null )
			{
				if ( _controllers == null )
				{
					_controllers = new HashSet<Controller>();
				}
				
				if ( _controllers.Add( tController ) )
				{
					effectsAddController( tController, tIsControllerNotified );
					
					return true;
				}
			}
			
			return false;
		}
		
		protected virtual void effectsAddController( Controller tController, bool tIsControllerNotified = true )
		{
			// Notify Controller
			if ( tIsControllerNotified )
			{
				tController.addPawn( this, false );
			}
		}
		
		public virtual bool removeController( Controller tController, bool tIsControllerNotified = true )
		{
			if ( _controllers != null && _controllers.Remove( tController ) )
			{			
				if ( _controllers.Count == 0 )
				{
					_controllers = null;
				}
				
				effectsRemoveController( tController, tIsControllerNotified );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsRemoveController( Controller tController, bool tIsControllerNotified = true )
		{
			// Notify Controller
			if ( tIsControllerNotified && tController != null )
			{
				tController.removePawn( this, false );
			}
		}
	}
}