using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace libprogressclientlog
{
	public class ClientLogParser
	{
		public static List<int> GetStackForLine (string filename, int lineNumber) {
			Dictionary<int, int> parsed = Parse (filename);

			if (!parsed.ContainsKey (lineNumber))
				return new List<int> ();

			List<int> stack = new List<int>();

			int parentLine = lineNumber;
			while (parentLine != 0) {
				stack.Add (parentLine);
				parentLine = parsed [parentLine];
			}
			stack.Reverse ();
			return stack;
		}

		public static Dictionary<int, int> Parse (string filename)
		{
			Stack<StackElement> Stack = new Stack<StackElement>();
			Stack.Push(new StackElement(0, "gp_prun"));

			string[] lines = System.IO.File.ReadAllLines(filename);
		
			Dictionary<int, int> calleeMap = new Dictionary<int, int> ();

			int lineNumber = 1;
			foreach (string line in lines)
			{
				// Use a tab to indent each line of the file.
				Regex qariRegex = new Regex(".* Run (?<name>[a-zA-Z0-9_]+?) ");
				MatchCollection mc = qariRegex.Matches(line);
				if (mc.Count > 0) {
					foreach (Match m in mc) {
						string name = m.Groups ["name"].ToString ();
						int parentLine = Stack.Peek ().parentLine;
						StackElement stackElement = new StackElement (lineNumber, name);
						Stack.Push (stackElement);
						calleeMap [lineNumber] = parentLine;
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
