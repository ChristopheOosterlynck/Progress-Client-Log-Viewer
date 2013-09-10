using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Stack<StackElement> Stack = new Stack<StackElement>();
			Stack.Push(new StackElement(0, "gp_prun"));

			string[] lines = System.IO.File.ReadAllLines(@"C:\client.log");
		
			Dictionary<int, StackElement> calleeMap = new Dictionary<int, StackElement> ();

			// Display the file contents by using a foreach loop.
			System.Console.WriteLine("Contents of client.txt = ");
			int lineNumber = 0;
			foreach (string line in lines)
			{
				// Use a tab to indent each line of the file.
				Regex qariRegex = new Regex(".* Run (?<name>[a-zA-Z0-9_]+?) ");
				MatchCollection mc = qariRegex.Matches(line);
				if (mc.Count > 0) {
					foreach (Match m in mc) {
						string name = m.Groups ["name"].ToString ();
						StackElement stackElement = new StackElement (lineNumber, name);
						Stack.Push (stackElement);
						calleeMap [lineNumber] = stackElement;
					}
				} else {

					qariRegex = new Regex (".* Return from Main Block.*\\[(?<name>[a-zA-Z0-9_]+?)\\]");
					mc = qariRegex.Matches (line);
					if (mc.Count > 0) {
						foreach (Match m in mc) {
							string name = m.Groups ["name"].ToString ();
							if (name == Stack.Peek ().Name) {
								Stack.Pop ();
							}
						}
					} else {
						qariRegex = new Regex (".* Return from (?<name>[a-zA-Z0-9_]+?) ");
						mc = qariRegex.Matches (line);
						foreach (Match m in mc) {
							string name = m.Groups ["name"].ToString ();
							if (name == Stack.Peek ().Name) {
								Stack.Pop ();
							}
						}
					}
				}
				/*if (mc.Count == 1) {
					CaptureCollection cc = mc [0].Captures;
					Console.WriteLine("\t" + cc[0].ToString());
				}*/

				/*Console.WriteLine ("\t" + lineNumber);
				foreach (StackElement stackElement in Stack) {
					Console.WriteLine("\t" + stackElement.Name);
				}*/

						lineNumber++;
			}
		}
	}
}
