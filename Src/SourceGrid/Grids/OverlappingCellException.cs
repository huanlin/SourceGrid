using System;

namespace SourceGrid
{
	public class OverlappingCellException : SourceGridException
	{
		public OverlappingCellException(string message): base(message)
		{
		}
	}
}
