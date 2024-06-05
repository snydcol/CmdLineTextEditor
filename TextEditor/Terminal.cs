using System;
using System.IO;

namespace TextEditor
{
	public static class Terminal
	{

		public static void ClearScreen()
		{
			Console.Clear();
		}

		public static void MoveCursor(int row, int column)
		{
			Console.CursorTop = row;
			Console.CursorLeft = column;
		}

		public static void WriteFile(string fileName, GapBuffer buffer)
		{
			File.WriteAllText(fileName, buffer.ToString());
		}
		public static string LoadFile(string fileName)
		{
			return File.ReadAllText(fileName);
		}
	}
}