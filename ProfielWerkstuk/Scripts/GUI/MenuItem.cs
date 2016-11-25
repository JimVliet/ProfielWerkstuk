using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class MenuItem
	{
		public IMenuItem Data;
		public float UpperMargin = 0;
		public float LowerMargin = 0;

		public MenuItem(IMenuItem data)
		{
			Data = data;
		}
	}

	public interface IMenuItem
	{
		ClickEvent ClickedEvent { get; set; }
		bool IsBeingHovered { get; set; }
		Vector2 Size { get; set; }

		void Draw(SpriteBatch spriteBatch, Vector2 position);
		Vector2 GetSize();
		Vector2 GetTopLeft(Vector2 pos);
		Vector2 GetLowerRight(Vector2 pos);
	}
}
