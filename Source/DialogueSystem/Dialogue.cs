using UnityEngine;
using System;
using System.Collections.Generic;

namespace PeenTalk
{
	//##########################
	// Class Declaration
	//##########################
	public class Dialogue : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================		
		[SerializeField]
		protected string _bookmark;
		protected Dictionary<string,Item> _bookmarks; // key is bookmark
		protected Item _item;
		[SerializeField]
		protected EventDialogueItemOpen _onItemOpen;
		protected float _duration;
		[SerializeField]
		protected EventDialogueItemClose _onItemClose;
		
		//=======================
		// Initialization
		//=======================
		protected virtual void Start()
		{
			// Check to see if ticked
			enable();
			
			// Effects
			effectsBookmark( _bookmark );
		}
		
		//=======================
		// Deconstruction
		//=======================
		protected virtual void OnDestroy()
		{
			// Clear event
			_onItemOpen.RemoveAllListeners();
			_onItemClose.RemoveAllListeners();
		
			// Remove Bookmarks
			if ( _bookmarks != null )
			{
				List<string> tempKeys = new List<string>( _bookmarks.Keys );
				for ( int i = ( tempKeys.Count - 1 ); i >= 0; --i )
				{
					removeBookmark( tempKeys[i], _bookmarks[ tempKeys[i] ] );
				}
				
				if ( _bookmarks != null )
				{
					_bookmarks.Clear();
					_bookmarks = null;
				}
			}
		}
		
		//=======================
		// Tick
		//=======================
		public virtual bool enable()
		{
			enabled = _item != null && _duration > 0;
			return enabled;
		}
		
		protected virtual void Update()
		{
			if ( _item == null )
			{
				enabled = false;
			}
			else
			{
				// Count down
				_duration -= Time.deltaTime;
				if ( _duration <= 0 )
				{
					setDuration( 0 );
				}
			}
		}
		//=======================
		// Bookmarks
		//=======================
		public virtual List<KeyValuePair<string,Item>> bookmarks
		{
			get
			{
				if ( _bookmarks != null )
				{
					List<KeyValuePair<string,Item>> tempBookmarks = new List<KeyValuePair<string,Item>>();
					foreach ( KeyValuePair<string,Item> tempPair in _bookmarks )
					{
						tempBookmarks.Add( tempPair );
					}
					
					return tempBookmarks;
				}
				
				return null;
			}
		}
		
		public virtual bool getBookmark( string tName, out Item tItem )
		{
			if ( tName != null && _bookmarks != null && _bookmarks.TryGetValue( tName, out tItem ) )
			{
				return true;
			}
			
			tItem = null;
			return false;
		}
		
		public virtual bool addBookmark( string tName, Item tItem )
		{
			if ( _bookmarks == null  )
			{
				_bookmarks = new Dictionary<string,Item>();
			}
			_bookmarks[ tName ] = tItem;
			
			return true;
		}
		
		public virtual bool removeBookmark( string tName, Item tItem )
		{
			if ( _bookmarks != null && _bookmarks.Remove( tName ) )
			{
				if ( _bookmarks.Count == 0 )
				{
					_bookmarks = null;
				}

				return true;
			}
			
			return false;
		}
		
		public virtual string bookmark
		{
			get
			{
				return _bookmark;
			}
			set
			{
				setBookmark( value );
			}
		}
		
		public virtual void setBookmark( string tBookmark )
		{
			string tempOld = _bookmark;
			_bookmark = tBookmark;
			effectsBookmark( tempOld );
		}
		
		protected virtual void effectsBookmark( string tOld )
		{
			// Goto Item
			if ( _bookmark != null )
			{
				Item tempItem;
				if ( _bookmarks != null && _bookmarks.TryGetValue( _bookmark, out tempItem ) )
				{
					setItem( tempItem );
				}
			}
		}
		
		//=======================
		// Item
		//=======================
		public virtual Item item
		{
			get
			{
				return _item;
			}
			set
			{
				setItem( value );
			}
		}
		
		public virtual bool setItem( Item tItem )
		{
			if ( tItem == null || tItem.validate( this ) )
			{
				Item tempOld = _item;
				_item = tItem;
				effectsItem( tempOld );
				
				return true;
			}
			
			return false;
		}
		
		protected virtual void effectsItem( Item tOld )
		{
			// Reset counter
			setDuration( -1 );
			
			// Open
			Item tempItem = _item;
			if ( _item == null )
			{
				_bookmark = null;
			}
			else
			{
				_item.open( this );
			}
			
			// Set duration and proceed with event ONLY if the Item hasn't changed
			if ( tempItem == _item )
			{				
				openItem( tOld );
				if ( tempItem == _item && _item != null )
				{
					setDuration( _item.duration );
				}
			}
		}
		
		protected virtual void openItem( Item tOld )
		{
			// Event
			_onItemOpen.Invoke( this, tOld );
		}
		
		protected virtual void closeItem()
		{
			// Event
			_onItemClose.Invoke( this );
		}
		
		public virtual void nextItem()
		{
			if ( _item != null )
			{
				setItem( _item.nextItem );
			}
		}
		
		public virtual void choose( int tChoice )
		{
			ItemChoices tempChoices = _item as ItemChoices;
			if ( tempChoices != null )
			{
				tempChoices.choose( this, tChoice );
			}
		}
		
		public virtual void choose( Choice tChoice )
		{
			ItemChoices tempChoices = _item as ItemChoices;
			if ( tempChoices != null )
			{
				tempChoices.choose( this, tChoice );
			}
		}
		
		//=======================
		// Duration
		//=======================
		public virtual float duration
		{
			get
			{
				return _duration;
			}
			set
			{
				setDuration( value );
			}
		}
		
		public virtual bool setDuration( float tDuration )
		{
			float tempOld = _duration;
			_duration = tDuration;
			effectsDuration( tempOld );
			
			return true;
		}
		
		protected virtual void effectsDuration( float tOld )
		{
			// Toggle tick
			enable();
			
			// Close Item if at 0 and proceed with event ONLY if the Item hasn't changed
			if ( _duration == 0 )
			{
				Item tempItem = _item;
				if ( _item != null )
				{
					_item.close( this );
				}
				
				if ( tempItem == _item )
				{
					closeItem();
				}
			}
		}
		
		//=======================
		// Events
		//=======================
		public virtual EventDialogueItemOpen onItemOpen
		{
			get
			{
				return _onItemOpen;
			}
		}
		
		public virtual EventDialogueItemClose onItemClose
		{
			get
			{
				return _onItemClose;
			}
		}
	}
}