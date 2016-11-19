using Microsoft.Xna.Framework;

namespace ProfielWerkstuk.Scripts.Utility
{
	public static class Utilities
	{
		public static bool IsPointWithin(Vector2 point, Vector2 topLeft, Vector2 bottomRight)
		{
			return point.X >= topLeft.X && point.X <= bottomRight.X
				   && point.Y >= topLeft.Y && point.Y <= bottomRight.Y;
		}
	}
}
