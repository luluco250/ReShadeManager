using System;
using System.Runtime.InteropServices;

namespace ReShadeManager.Core.Utils
{
	static class NativeMethods
	{
		public const int SYMBOLIC_LINK_FLAG_DIRECTORY = 1;
		public const int SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 2;

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool CreateSymbolicLinkW(
			string lpSymlinkFileName,
			string lpTargetFileName,
			int dwFlags);
	}
}
