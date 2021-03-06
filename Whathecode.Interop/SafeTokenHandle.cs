﻿using System;
using Microsoft.Win32.SafeHandles;


namespace Whathecode.Interop
{
	/// <summary>
	///   A wrapper class for a token handle.
	/// </summary>
	public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public SafeTokenHandle()
			: base( true ) {}

		public SafeTokenHandle( IntPtr handle )
			: base( true )
		{
			SetHandle( handle );
		}


		protected override bool ReleaseHandle()
		{
			return Kernel32.CloseHandle( handle );
		}
	}
}
