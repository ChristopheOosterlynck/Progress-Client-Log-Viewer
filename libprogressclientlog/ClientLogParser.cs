using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace libprogressclientlog
{
	public class ClientLogParser
	{

		public static Dictionary<int, StackElement> Parse (string filename)
		{
			Stack<StackElement> Stack = new Stack<StackElement>();
			Stack.Push(new StackElement(0, "gp_prun"));

			string[] lines = System.IO.File.ReadAllLines(filename);
		
			Dictionary<int, StackElement> calleeMap = new Dictionary<int, StackElement> ();

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
					qariRegex = new Regex (".* Return from (?<procedure>(Main Block|[a-zA-Z0-9_]+)?).*\\[(?<file>[a-zA-Z0-9_]+?)\\]");
					mc = qariRegex.Matches (line);
					if (mc.Count > 0) {
						foreach (Match m in mc) {
							string procedure = m.Groups ["procedure"].ToString ();
							string file = m.Groups ["file"].ToString ();
							if ((procedure == "Main Block" && file == Stack.Peek ().Name)
							    || procedure== Stack.Peek ().Name ) {
								Stack.Pop ();
							}
						}
					}
				}
				lineNumber++;
			}

			return calleeMap;
		}
	}
}
