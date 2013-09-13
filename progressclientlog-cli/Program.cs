using System;
using System.Collections.Generic;
using libprogressclientlog;

namespace progressclientlogcli
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			List<int> stack = ClientLogParser.GetStackForLine (@"c:\\client.log", 25);
			foreach (int line in stack)
			{
				Console.WriteLine(line);
			}
		}
	}
}
