using System;
namespace AssemblyCSharp
{
	public static class StringFormatter
	{
		public static string reduceWhitespaces(string text)
		{
			System.Text.RegularExpressions.RegexOptions options = System.Text.RegularExpressions.RegexOptions.None;
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex (@"[ ]{2,}", options);
			return regex.Replace (text, @" ");
		}
	}
}

