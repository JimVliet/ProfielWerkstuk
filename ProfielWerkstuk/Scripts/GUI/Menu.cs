using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class Menu
	{
		public Vector2 Position;
		private Vector2 _size;
		public Color MenuColor = new Color(34, 34, 34);
		private readonly List<MenuItem> _buttonList = new List<MenuItem>();
		public Vector2 Margin = new Vector2(25f, 25f);
		public float BaseButtonDistance = 25f;
		public bool IsActive = true;
		public bool IsBeingDisabled;
		public bool AllowClicking = true;
		public MenuActivated MenuActivated;

		public Menu(Vector2 pos)
		{
			Position = pos;
			_size = GetMenuSize();
		}

		public Vector2 GetMenuSize()
		{
			float horizontalSize = 0;
			float verticalSize = (_buttonList.Count-1) * BaseButtonDistance;

			foreach (MenuItem item in _buttonList)
			{
				Vector2 buttonSize = item.Data.GetSize();
				horizontalSize = Math.Max(buttonSize.X, horizontalSize);
				verticalSize += buttonSize.Y + item.LowerMargin + item.UpperMargin;
			}

			return new Vector2(Math.Max(20f, horizontalSize), Math.Max(20f, verticalSize)) + 2 * Margin;
		}

		public void ResetButtonWidth()
		{
			if (_buttonList.Count == 0)
				return;

			float maxWidth = 0;

			foreach (MenuItem item in _buttonList)
			{
				maxWidth = Math.Max(item.Data.GetSize().X, maxWidth);
			}

			foreach (MenuItem item in _buttonList)
			{
				item.Data.ButtonSize.X = maxWidth;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			MonoGame.Extended.Shapes.SpriteBatchExtensions.FillRectangle(spriteBatch, Position - (_size/2), _size, MenuColor);

			if(_buttonList.Count  == 0)
				return;
			Vector2 drawPosition = Position - (_size/2) + Margin;
			foreach (MenuItem item in _buttonList)
			{
				Vector2 itemSize = item.Data.GetSize();
				item.Data.Draw(spriteBatch, new Vector2(Position.X, drawPosition.Y + item.UpperMargin + itemSize.Y/2));
				drawPosition.Y += itemSize.Y + item.LowerMargin + item.UpperMargin + BaseButtonDistance;
			}
		}

		public void AddButton(Button button)
		{
			_buttonList.Add(new MenuItem(button));
			_size = GetMenuSize();
			ResetButtonWidth();
		}

		public Button GetButton(int pos)
		{
			return _buttonList.Count > pos ? _buttonList[pos].Data : null;
		}

		public bool RemoveButton(int index)
		{
			if(_buttonList.Count <= index)
				return false;
			_buttonList.RemoveAt(index);
			_size = GetMenuSize();
			ResetButtonWidth();
			return true;
		}

		public Vector2 GetTopLeft()
		{
			return Position - (_size/2);
		}

		public Vector2 GetLowerRight()
		{
			return Position + (_size / 2);
		}

		public List<MenuItem> GetMenuItems()
		{
			return _buttonList;
		}

		public List<Vector2> GetButtonPositions()
		{
			List<Vector2> vectorList = new List<Vector2>();

			Vector2 drawPosition = Position - (_size / 2) + Margin;
			foreach (MenuItem item in _buttonList)
			{
				Vector2 itemSize = item.Data.GetSize();
				vectorList.Add(new Vector2(Position.X, drawPosition.Y + item.UpperMargin + itemSize.Y / 2));
				drawPosition.Y += itemSize.Y + item.LowerMargin + item.UpperMargin + BaseButtonDistance;
			}

			return vectorList;
		}
	}

	public delegate void MenuActivated(Menu menu);
}
