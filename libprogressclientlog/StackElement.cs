using System;

namespace libprogressclientlog
{
	public class StackElement
	{
		private string myName = "N/A";
		public string Name
		{
			get 
			{
				return myName; 
			}
			set 
			{
				myName = value; 
			}
		}

		public int parentLine = 0;

		public StackElement (int lineNumber, string name)
		{
			this.parentLine = lineNumber;
			this.Name = name;
		}
	}
}

