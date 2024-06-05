using System;

namespace TextEditor
{
	public class Cursor
	{
		public int Row { get; private set; }
		public int Column { get; private set; }
		public int BufferPos { get; private set; }

		//TODO Probaly should move this to GapBuffer -- cursor should only care about visual position
		//BufferPos should only be recalculated on line change otherwise can be incremented with column
		public void	SetBufferPos(GapBuffer b)
		{
			BufferPos = b.CalculateBufferPos(Row, Column);
		}

		public Cursor(int row = 0, int column = 0)
		{
			Console.CursorVisible = true;
			Row = row;
			Column = column;
			//TODO Actually calculate
			BufferPos = 0;
		}

		//TODO check to see if changed before dont want an out of bounds move recalculating it
		public void MoveUp(GapBuffer b)
		{
			Row -= 1;
			BoundsCheck(b);
			SetBufferPos(b);
		}
		//TODO check to see if changed before dont want an out of bounds move recalculating it
		public void MoveDown(GapBuffer b)
		{
			Row += 1;
			BoundsCheck(b);
			SetBufferPos(b);
		}

		public void MoveRight(GapBuffer b, int numColumns)
		{
			Column += numColumns;
			BufferPos += numColumns;
			BoundsCheck(b);
		}

		public void MoveRight(GapBuffer b)
		{
			Column += 1;
			BufferPos += 1;
			BoundsCheck(b);
		}

		public void MoveLeft(GapBuffer b)
		{
			Column -= 1;
			BufferPos -= 1;
			BoundsCheck(b);
		}

		public void MoveBackSpace(GapBuffer b)
		{
			if (Column == 0)
			{
				Row -= 1;
				//Force bounds check to pick last column in row
				Column = Int32.MaxValue;
				BoundsCheck(b);
				SetBufferPos(b);
			}
			else
			{
				MoveLeft(b);
			}
		}

		public void MoveNewLine(GapBuffer b)
		{
			Column = 0;
			Row += 1;
			SetBufferPos(b);
			BoundsCheck(b);
		}

		public void BoundsCheck(GapBuffer b)
		{
			//Max of 0 and pos to prevent negatives
			//Then Min of that and num to prevent positive out of bounds
			Row = Math.Min(b.GetNumLines(), Math.Max(Row, 0));
			Column = Math.Min(b.GetNumColumnsInRow(Row), Math.Max(Column, 0));

		}
	}
}