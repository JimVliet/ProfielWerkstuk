﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace ProfielWerkstuk.Scripts.GUI.BaseClasses
{
	public class ControlMenuElement : BaseMenuElement
	{
		protected Texture2D Texture;
		private bool _isBeingHovered;

		protected ControlMenuElement(MenuContainer parentContainer, Texture2D texture) : base(parentContainer)
		{
			Texture = texture;
			UpdateSize();
		}

		protected override Vector2 GetMinimalSize()
		{
			return new Vector2(Texture.Width + 2 * Padding.X, Texture.Height + 2 * Padding.Y);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 drawLocation = GetPosition() - Size / 2;
			drawLocation.X = (int)drawLocation.X;
			drawLocation.Y = (int)drawLocation.Y;

			if (_isBeingHovered)
				spriteBatch.FillRectangle(drawLocation - Padding, Size, Color.White * 0.05f);

			spriteBatch.Draw(Texture, drawLocation);
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
