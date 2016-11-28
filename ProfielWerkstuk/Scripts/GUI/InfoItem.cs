using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.Events;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class InfoItem : IMenuItem
	{
		private readonly List<InfoTextElement> _infoElements = new List<InfoTextElement>();

		public ClickEvent ClickedEvent { get; set; }

		public Vector2 Size { get; set; }

		/// <summary>
		/// This class is a container item. It is used to store multiple textelements in it's area.
		/// It is used as a menuitem, so it is part of a menu normally.
		/// </summary>
		/// <param name="size">Used to give the InfoItem a minimal size. 
		/// WARNING: it might be automatically made larger if the minimum size exceeds the current size</param>
		public InfoItem(Vector2 size)
		{
			Size = size;
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 pos)
		{
			foreach (InfoTextElement element in _infoElements)
			{
				element.Draw(spriteBatch, pos + element.Position);
			}
		}

		public void Hover(bool hovered, Vector2 position, Vector2 hoverLocation)
		{
			
		}

		public void AddInfoText(InfoTextElement element)
		{
			_infoElements.Add(element);
			UpdateSize();
		}

		public void UpdateSize()
		{
			Vector2 minSize = GetMinSize();
			Size = new Vector2(Math.Max(minSize.X, Size.X), Math.Max(minSize.Y, Size.Y));
		}

		public Vector2 GetSize()
		{
			return Size;
		}

		public Vector2 GetMinSize()
		{
			float minHalfWidth = Size.X/2;
			float minHalfHeight = Size.Y / 2;

			foreach (InfoTextElement element in _infoElements)
			{
				Vector2 cornerOne = element.Position + element.Size/2 + element.Padding;
				Vector2 cornerTwo = element.Position - element.Size/2 - element.Padding;

				minHalfWidth = Math.Max(Math.Abs(cornerOne.X), minHalfWidth);
				minHalfHeight = Math.Max(Math.Abs(cornerOne.Y), minHalfHeight);

				minHalfWidth = Math.Max(Math.Abs(cornerTwo.X), minHalfWidth);
				minHalfHeight = Math.Max(Math.Abs(cornerTwo.Y), minHalfHeight);
			}

			return new Vector2(2*minHalfWidth, 2*minHalfHeight);
		}

		public Vector2 GetTopLeft(Vector2 pos)
		{
			return pos - (Size / 2);
		}

		public Vector2 GetLowerRight(Vector2 pos)
		{
			return pos + (Size / 2);
		}
	}

	public class InfoTextElement
	{
		public SpriteFont Font;
		public string Text;
		public Color TextColor = Color.White;
		public Vector2 Padding = new Vector2(10f, 10f);
		public SizeUpdate SizeUpdated;
		public ClickEvent ClickedEvent { get; set; }
		public bool IsBeingHovered;

		private Vector2 _position;
		public Vector2 Position
		{
			get { return _position; }
			set
			{
				SizeUpdated?.Invoke(this);
				_position = value;
			}
		}

		private Vector2 _size;
		public Vector2 Size
		{
			get { return _size; }
			set
			{
				SizeUpdated?.Invoke(this);
				_size = value;
			}
		}

		public InfoTextElement(Vector2 pos, Vector2 size, string text, SpriteFont font)
		{
			_position = pos;
			Text = text;
			Font = font;
			_size = size;
			UpdateSize();
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 pos)
		{
			Vector2 textVector2 = pos - Font.MeasureString(Text) / 2;
			//This prevents some nasty anti-aliasing making the letters clearer and less smudged
			textVector2.X = (int)textVector2.X;
			textVector2.Y = (int)textVector2.Y;

			spriteBatch.DrawString(Font, Text, textVector2, TextColor);
		}

		public Vector2 GetMinimalSize()
		{
			return Font.MeasureString(Text) + (2 * Padding);
		}

		public void UpdateSize()
		{
			Vector2 minSize = GetMinimalSize();
			Size = new Vector2(Math.Max(minSize.X, Size.X), Math.Max(minSize.Y, Size.Y));
		}
	}
}
