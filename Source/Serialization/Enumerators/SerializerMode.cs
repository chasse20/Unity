using System;

namespace PeenRetain
{
	//##########################
	// Enum Declaration
	//##########################
	[Flags]
	public enum SerializerMode
	{
		Readable = 1,
		ReadableEditor = 2,
		ReadableEditorPlay = 4,
		ReadOnAwake = 8,
		DestroyOnAwake = 16, // will destroy in editor only if playing
		Writable = 32,
		WritableEditor = 64,
		WritableEditorPlay = 128,
		WriteOnDestroy = 256
	}
}