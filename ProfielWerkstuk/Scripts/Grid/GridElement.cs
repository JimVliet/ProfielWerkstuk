namespace ProfielWerkstuk.Scripts.Grid
{
	public class GridElement
	{
		public int X;
		public int Y;
		public GridElementType Type = GridElementType.Empty;

		public GridElement(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public enum GridElementType
	{
		Empty, Solid, Start, End
	}
}
