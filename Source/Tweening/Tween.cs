using System;
using UnityEngine;

namespace PeenTween
{	
	//##########################
	// Class Declaration
	//##########################
	public class Tween : ITween
	{
		//=======================
		// Variables
		//=======================
		[SerializeField]
		protected TimeMode _timeMode = TimeMode.Scaled;
		public float elapsed;
		public float duration;
		public int loops = 1; // 0 or under is unlimited
		protected Action<Tween> _onCompleted;
		protected Action<Tween> _onTicked;
		[SerializeField]
		protected Component _owner;
		
		//=======================
		// Constructor
		//=======================
		public Tween( float tDuration )
		{
			duration = tDuration;
		}
		
		//=======================
		// Destructor
		//=======================
		~Tween()
		{
			_onTicked = null;
			_onCompleted = null;
		}
		
		//=======================
		// Tween
		//=======================
		public virtual TimeMode timeMode
		{
			get
			{
				return _timeMode;
			}
			set
			{
				_timeMode = value;
			}
		}
		
		public virtual TweenTick tick( float tDeltaTime )
		{
			elapsed += tDeltaTime;
			if ( elapsed >= duration )
			{
				if ( loops == 1 )
				{
					if ( _onTicked != null )
					{
						_onTicked( this );
					}
			
					if ( _onCompleted != null )
					{
						_onCompleted( this );
					}
					
					return TweenTick.Completed;
				}
				else
				{
					--loops;
					elapsed -= duration; // ensure remainder is accurately tacked on
				}
			}
			
			if ( _onTicked != null )
			{
				_onTicked( this );
			}
			return TweenTick.Ticked;
		}
		
		public virtual void reverse()
		{
			elapsed = duration - elapsed;
		}
		
		//=======================
		// Events
		//=======================
		public virtual event Action<Tween> onCompleted
		{
			add
			{
				_onCompleted += value;
			}
			remove
			{
				_onCompleted -= value;
			}
		}
		
		public virtual event Action<Tween> onTicked
		{
			add
			{
				_onTicked += value;
			}
			remove
			{
				_onTicked -= value;
			}
		}
		
		//=======================
		// Owner
		//=======================
		public virtual Component owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}
	}
	
	//##########################
	// Class Declaration
	//##########################
	public class Tween<T> : ITween
	{
		//=======================
		// Variables
		//=======================
		[SerializeField]
		protected TimeMode _timeMode = TimeMode.Scaled;
		public float elapsed;
		public float duration;
		public int loops = 1; // 0 or under is unlimited
		public bool isPingPong;
		protected Action<Tween<T>> _onCompleted;
		public float interval;
		public float counter;
		protected Action<Tween<T>> _onTicked;
		public T from;
		public T to;
		public EasingType easing;
		[SerializeField]
		protected Component _owner;
		
		//=======================
		// Constructor
		//=======================
		public Tween( float tDuration, T tFrom, T tTo )
		{
			duration = tDuration;
			from = tFrom;
			to = tTo;
		}
		
		//=======================
		// Destructor
		//=======================
		~Tween()
		{
			_onTicked = null;
			_onCompleted = null;
		}
		
		//=======================
		// Tween
		//=======================
		public virtual TimeMode timeMode
		{
			get
			{
				return _timeMode;
			}
			set
			{
				_timeMode = value;
			}
		}
		
		public virtual TweenTick tick( float tDeltaTime )
		{
			if ( duration > 0 )
			{
				elapsed += tDeltaTime;
				if ( elapsed >= duration )
				{
					if ( loops == 1 )
					{
						if ( _onTicked != null )
						{
							_onTicked( this );
						}
						
						if ( _onCompleted != null )
						{
							_onCompleted( this );
						}
						
						return TweenTick.Completed;
					}
					else
					{
						--loops;
						if ( isPingPong )
						{
							float tempRemainder = elapsed - duration;
							reverse();
							elapsed += tempRemainder;
						}
						else
						{
							elapsed -= duration; // ensure remainder is accurately tacked on
						}
					}
				}
			}
			
			if ( interval > 0 )
			{
				counter -= tDeltaTime;
				if ( counter <= 0 )
				{
					counter += interval;
				}
				else
				{
					return TweenTick.Ticking;
				}
			}
			
			if ( _onTicked != null )
			{
				_onTicked( this );
			}
			return TweenTick.Ticked;
		}
		
		public virtual void reverse()
		{
			// Swap
			T tempFrom = from;
			from = to;
			to = tempFrom;
		}
		
		public virtual T result
		{
			get
			{
				return from;
			}
		}
		
		//=======================
		// Events
		//=======================
		public virtual event Action<Tween<T>> onCompleted
		{
			add
			{
				_onCompleted += value;
			}
			remove
			{
				_onCompleted -= value;
			}
		}
		
		public virtual event Action<Tween<T>> onTicked
		{
			add
			{
				_onTicked += value;
			}
			remove
			{
				_onTicked -= value;
			}
		}
		
		//=======================
		// Owner
		//=======================
		public virtual Component owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}
	}
}