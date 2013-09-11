using System;
using libprogressclientlog;

namespace progressclientlogcli
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ClientLogParser.Parse (@"c:\\client.log");
		}
	}
}
