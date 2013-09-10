using System;

namespace test
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

		public int lineNumber = 0;

		public StackElement (int lineNumber, string name)
		{
			this.lineNumber = lineNumber;
			this.Name = name;
		}
	}
}

