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

		public static bool IsCollinear(int xOne, int yOne, int xTwo, int yTwo, int xThree, int yThree)
		{
			//Check if the area of the triangle is equal to 0
			return xOne * (yTwo - yThree) + xTwo * (yThree - yOne) + xThree * (yOne - yTwo) == 0;
		}

		public static int GetWindowWidth(Game1 game)
		{
			return game.Graphics.PreferredBackBufferWidth;
		}

		public static int GetWindowHeight(Game1 game)
		{
			return game.Graphics.PreferredBackBufferHeight;
		}
	}
}
