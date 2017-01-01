using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace ProfielWerkstuk.Scripts.GUI.BaseClasses
{
	public class ColoredMenuElement : BaseMenuElement
	{
		private readonly Vector2 _previewSize;
		private bool _isBeingHovered;

		protected ColoredMenuElement(MenuContainer parentContainer, Vector2 preview) : base(parentContainer)
		{
			_previewSize = preview;
		}

		protected override Vector2 GetMinimalSize()
		{
			return _previewSize + 2*Padding;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 drawLocation = GetPosition() - Size / 2 + Padding;
			drawLocation.X = (int)drawLocation.X;
			drawLocation.Y = (int)drawLocation.Y;

			if (_isBeingHovered)
				spriteBatch.FillRectangle(drawLocation - Padding, Size, Color.White * 0.05f);

			spriteBatch.FillRectangle(drawLocation, _previewSize, GetPreviewColor());
		}

		protected virtual Color GetPreviewColor()
		{
			return Color.Black;
		}

		public override void Hover()
		{
			_isBeingHovered = true;
		}

		public override void UnHover()
		{
			_isBeingHovered = false;
		}
	}
}
