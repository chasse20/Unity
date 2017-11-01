using UnityEngine;
using System;
using System.IO;
using System.Collections;
using PeenToys; // cross contamination

namespace PeenRetain
{	
	//##########################
	// Class Declaration
	//##########################
	public abstract class Serializer : MonoBehaviour
	{
		//=======================
		// Variables
		//=======================
		public Streamer streamer;
		[AttributeMask( typeof( SerializerMode ) )]
		public SerializerMode mode;
		public float version;
		
		//=======================
		// Initialization
		//=======================
		protected virtual void Awake()
		{
			// Read if auto on, otherwise destroy if in play
			if ( ( mode & SerializerMode.ReadOnAwake ) == SerializerMode.ReadOnAwake )
			{
				read();
			}
			
			#if UNITY_EDITOR
				if ( ( mode & SerializerMode.DestroyOnAwake ) == SerializerMode.DestroyOnAwake && Application.isPlaying )
				{
					Destroy( this );
				}
			#else
				if ( ( mode & SerializerMode.DestroyOnAwake ) == SerializerMode.DestroyOnAwake )
				{
					Destroy( this );
				}
			#endif
		}
		
		//=======================
		// Deconstruction
		//=======================
		protected virtual void OnDestroy()
		{
			// Serialize if auto
			if ( ( mode & SerializerMode.WriteOnDestroy ) == SerializerMode.WriteOnDestroy )
			{
				write();
			}
		}
		
		//=======================
		// Read
		//=======================
		public virtual bool read()
		{
			// Read only if readable and playing, or not playing and readable in editor
			#if UNITY_EDITOR
				if ( ( mode & SerializerMode.Readable ) == SerializerMode.Readable && ( ( Application.isPlaying && ( mode & SerializerMode.ReadableEditorPlay ) == SerializerMode.ReadableEditorPlay ) || ( !Application.isPlaying && ( mode & SerializerMode.ReadableEditor ) == SerializerMode.ReadableEditor ) ) && streamer != null )
			#else
				if ( ( mode & SerializerMode.Readable ) == SerializerMode.Readable && streamer != null )
			#endif
			{
				return streamer.read( read );
			}
			
			return false;
		}
		
		public abstract void read( Stream tStream );
		
		//=======================
		// Write
		//=======================
		public virtual bool write()
		{
			// Write only if writeable and playing, or not playing and writeable in editor
			#if UNITY_EDITOR
				if ( ( mode & SerializerMode.Writable ) == SerializerMode.Writable && ( ( Application.isPlaying && ( mode & SerializerMode.WritableEditorPlay ) == SerializerMode.WritableEditorPlay ) || ( !Application.isPlaying && ( mode & SerializerMode.WritableEditorPlay ) == SerializerMode.WritableEditorPlay ) ) && streamer != null )
			#else
				if ( ( mode & SerializerMode.Writable ) == SerializerMode.Writable && streamer != null )
			#endif
			{
				return streamer.write( write );
			}
			
			return false;
		}
		
		public abstract void write( Stream tStream );
	}

	//##########################
	// Class Declaration
	//##########################
	public abstract class Serializer<T,L> : Serializer where L : UnityListRef<T> where T : class
	{
		//=======================
		// Variables
		//=======================
		[SerializeField]
		protected L _serializables;
		
		//=======================
		// Serializables
		//=======================
		public virtual L serializables
		{
			get
			{
				return _serializables;
			}
		}
	}
}