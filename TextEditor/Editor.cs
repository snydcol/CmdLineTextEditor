using System;
using System.Data.Common;
using System.IO;
using System.Windows;

namespace TextEditor
{
	public class Editor
	{
		private Cursor _cursor;
		private GapBuffer _buffer;

		public Editor()
		{
			var text = Terminal.LoadFile("LoadFile.txt");
			_cursor = new Cursor();
			_buffer = new GapBuffer(text);
		}

		public void Render()
		{
			//Clear Screen
			Console.Clear();
			//Move Cursor to start
			Terminal.MoveCursor(0, 0);
			//Render the stored text buffer
			_buffer.Render();

			//Move cursor to last postion
			Terminal.MoveCursor(_cursor.Row, _cursor.Column);
		}

		//TODO Undo
		public void HandleInput()
		{
			var text = Console.ReadKey();
			if (text.Modifiers == ConsoleModifiers.Control && text.Key == ConsoleKey.Q)
			{
				Console.WriteLine("Quitting");
				Environment.Exit(0);
			}
			//CTRL + L  -- Clear and reset --  testing only
			else if(text.Modifiers == ConsoleModifiers.Control && text.Key == ConsoleKey.L)
			{

				_buffer = new GapBuffer(50);
				_cursor = new Cursor();

			}
			//TODO Breaks
			//Save
			else if (text.Modifiers == ConsoleModifiers.Control && text.Key == ConsoleKey.S)
			{

				Terminal.WriteFile("NEW.txt", _buffer);
			}

			//Copy and paste are implented by windows
			else if (text.Key == ConsoleKey.Backspace)
			{
				_buffer.Remove(_cursor.BufferPos);
				_cursor.MoveBackSpace(_buffer);
			}

			else if (text.Key == ConsoleKey.Enter)
			{
				_buffer.Insert('\n'.ToString(), _cursor.BufferPos);
				_cursor.MoveNewLine(_buffer);
			}

			//Cursor Movement
			else if(text.Key == ConsoleKey.LeftArrow)
			{
				_cursor.MoveLeft(_buffer);
			}
			else if (text.Key == ConsoleKey.RightArrow)
			{
				_cursor.MoveRight(_buffer);
			}
			else if (text.Key == ConsoleKey.UpArrow)
			{
				_cursor.MoveUp(_buffer);
			}
			else if (text.Key == ConsoleKey.DownArrow)
			{
				_cursor.MoveDown(_buffer);
			}

			else
			{
				_buffer.Insert(text.KeyChar.ToString(), _cursor.BufferPos);
				_cursor.MoveRight(_buffer);
			}
		}

		public void Run()
		{
			while (true)
			{
				Render();
				HandleInput();
			}
		}
	}
}