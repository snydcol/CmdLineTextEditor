using System;
using System.Diagnostics;
using System.Media;
using System.Runtime.CompilerServices;

namespace TextEditor
{
	public class GapBuffer
	{

		public GapBuffer(string text)
		{
			Size = text.Length +10;
			Capacity = Size;
			Buffer = new char[Capacity];
			text.ToCharArray().CopyTo(Buffer, 10);
		}

		public GapBuffer(int bufferSize)
		{
			Capacity = bufferSize;
			Buffer = new char[bufferSize];
		}

		public char[] Buffer;

		/// <summary>
		/// Default GapSize
		/// </summary>
		public int GapSize => 10;
		
		public int GapStartPos { get; set; } = 0;
		public int GapsEndPos { get; set; } = 9;
		public int Capacity { get; set; }
		/// <summary>
		/// Size of text including gap
		/// </summary>
		public int Size { get; set; } = 10;

		public int GetNumLines()
		{
			var numLines = 0;
			for (int i = 0; i < Size; i++)
			{
				if(Buffer[i] == '\n')
					numLines++;
			}
			return numLines;
		}
		public int GetNumColumnsInRow(int row)
		{
			//TODO Better solution
			var startPos = CalculateBufferPos(row, 0);

			var numChars = 0;
			for (int i = startPos; i < GetSizeExcludingGap(); i++)
			{
				if (Buffer[i] == '\n')
					break;
				if (Buffer[i] != '\0')
					numChars++;
			}

			return numChars;
		}

		public int GetSizeExcludingGap()
		{
			return Size - (GapsEndPos - GapStartPos);
		}
		public int CalculateBufferPos(int row, int column)
		{

			var pos = 0;
			for (int i = 0; i < row; i++)
			{
				while (Buffer[pos] != '\n')
				{
					pos++;
				}
				pos++;
			}
			return pos + column;
		}

		public new string ToString()
		{
			var sz = GetSizeExcludingGap()-1;
			var c = new char[sz];
			Array.Copy(Buffer, c, GapStartPos);
			Array.Copy(Buffer, GapsEndPos+1, c, GapStartPos, sz);

			return new string(c);
		}

		//TODO Has issues with backspace since we keep charecters in buffer
		public void Render()
		{
			Console.WriteLine(Buffer);
		}

		//In hindsight could have just used copy too
		public void Resize(int pos)
		{
			var newBufferSize = Capacity + (GapSize *3);
			var newBuffer = new char[newBufferSize];
			//Copy Start of Buffer to Gap into the newBuffer
			for (int i = 0; i < pos; i++)
			{
				newBuffer[i] = Buffer[i];
			}
			//Create Gap in new buffer
			for (int i = pos; i < GapSize; i++)
			{
				newBuffer[i] = new char();
			}
			//Copy end of Gap to end of buffer into new buffer
			for (int i = pos; i < Size; i++)
			{
				newBuffer[i+GapSize] = Buffer[i];
			}

			Buffer = newBuffer;
			Capacity = newBufferSize;
			Size += GapSize;
			GapsEndPos += GapSize;
		}

		public void GrowGap(int newGapSize, int pos)
		{
			//If current Size of text is greater than the buffer -- grow buffer
			//Extend Buffer if the new Gap will be bigger than buffer

			if (Size + newGapSize > Capacity)
			{
				Resize(pos);
				return;
			}

			var copySize = Size - pos;
			//Create new array and copy buff from pos to end of text
			char[] a = new char[copySize];

			for (int i = 0; i < Size - pos; i++)
			{
				a[i] = Buffer[i + pos];
			}

			for (int i = pos; i < newGapSize+pos; i++)
			{
				Buffer[i] = new char();
			}

			for (int i = 0; i < copySize; i++)
			{
				Buffer[i + pos+ newGapSize] = a[i];
			}


			Size += newGapSize;
			GapsEndPos += newGapSize;
		}

		public void MoveCursor(int pos)
		{
			//Will call move left or right depending on POs
			if(pos < GapStartPos)
				MoveLeft(pos);
			else 
				MoveRight(pos);
		}
		private void MoveRight(int pos)
		{
			//Move Gap right
			while (pos > GapStartPos)
			{
				GapStartPos++;
				GapsEndPos++;

				Buffer[GapStartPos - 1] = Buffer[GapsEndPos];
				Buffer[GapsEndPos] = new char();
			}
			
		}
		private void MoveLeft(int pos)
		{
			while (pos < GapStartPos )
			{
				//Move Gap left
				GapStartPos--;
				GapsEndPos--;

				Buffer[GapsEndPos + 1] = Buffer[GapStartPos];
				Buffer[GapStartPos] = new char();
			}
			
		}

		public void Insert(String s, int pos)
		{
			//If pos is not start of gap move cursor
			if (pos != GapStartPos)
				MoveCursor(pos);
			

			foreach (var c in s)
			{
				if (GapStartPos == GapsEndPos)
				{
					//Grow Gap
					GrowGap(GapSize, pos);
				}

				Buffer[pos] = c;
				GapStartPos += 1;
				pos++;
			}
		}
		public void Remove(int pos)
		{

			if (pos == 0) return;

			if (pos != GapStartPos)
				MoveCursor(pos);
			GapStartPos--;

		}
	}
}