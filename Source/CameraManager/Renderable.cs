using System;

namespace PeenScreen
{	
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Immutable container for <see cref="View"/> and its rendered slot index, used by <see cref="ScreenManager"/></summary>
	public class Renderable
	{
		//=======================
		// Variables
		//=======================
		/// <summary>Immutable <see cref="View"/> reference</summary>
		protected View _view;
		/// <summary>Immutable slot index that is intended to be rendered</summary>
		protected uint _slotIndex;
		
		//=======================
		// Constructor
		//=======================
		public Renderable( View tView, uint tSlotIndex )
		{
			_view = tView;
			_slotIndex = tSlotIndex;
		}
		
		//=======================
		// Accessors
		//=======================
		public virtual View view
		{
			get
			{
				return _view;
			}
		}
		
		public virtual uint slotIndex
		{
			get
			{
				return _slotIndex;
			}
		}
	}
}