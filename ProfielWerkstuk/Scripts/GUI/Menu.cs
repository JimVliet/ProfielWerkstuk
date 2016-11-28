using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.Events;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class Menu
	{
		public Vector2 Position;
		private Vector2 _size;
		public Color MenuColor = new Color(34, 34, 34);
		private readonly List<MenuItem> _menuItemList = new List<MenuItem>();
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
			float verticalSize = (_menuItemList.Count-1) * BaseButtonDistance;

			foreach (MenuItem item in _menuItemList)
			{
				Vector2 buttonSize = item.Data.GetSize();
				horizontalSize = Math.Max(buttonSize.X, horizontalSize);
				verticalSize += buttonSize.Y + item.LowerMargin + item.UpperMargin;
			}

			return new Vector2(Math.Max(20f, horizontalSize), Math.Max(20f, verticalSize)) + 2 * Margin;
		}

		public void ResetButtonWidth()
		{
			if (_menuItemList.Count == 0)
				return;

			float maxWidth = 0;

			foreach (MenuItem item in _menuItemList)
			{
				maxWidth = Math.Max(item.Data.GetSize().X, maxWidth);
			}

			foreach (MenuItem item in _menuItemList)
			{
				item.Data.Size = new Vector2(maxWidth, item.Data.Size.Y);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 drawPos = Position - _size/2;
			//Prevents weird anti-aliasing
			drawPos.X = (int)drawPos.X;
			drawPos.Y = (int)drawPos.Y;

			MonoGame.Extended.Shapes.SpriteBatchExtensions.FillRectangle(spriteBatch, drawPos, _size, MenuColor);

			if(_menuItemList.Count  == 0)
				return;
			Vector2 drawPosition = Position - (_size/2) + Margin;
			foreach (MenuItem item in _menuItemList)
			{
				Vector2 itemSize = item.Data.GetSize();
				item.Data.Draw(spriteBatch, new Vector2(Position.X, drawPosition.Y + item.UpperMargin + itemSize.Y/2));
				drawPosition.Y += itemSize.Y + item.LowerMargin + item.UpperMargin + BaseButtonDistance;
			}
		}

		public void AddMenuItem(IMenuItem item)
		{
			_menuItemList.Add(new MenuItem(item));
			_size = GetMenuSize();
			ResetButtonWidth();
		}

		public void Hover(Vector2 mouseLocation)
		{
			List<Vector2> buttonPositions = GetButtonPositions();
			for (int i = 0; i < _menuItemList.Count; i++)
			{
				IMenuItem menuItem = _menuItemList[i].Data;
				bool pointWithin = Utilities.IsPointWithin(mouseLocation, menuItem.GetTopLeft(buttonPositions[i]), menuItem.GetLowerRight(buttonPositions[i]));
				menuItem.Hover(pointWithin, buttonPositions[i], mouseLocation);
			}
		}

		public Button GetButton(int pos)
		{
			return _menuItemList.Count > pos ? (Button)_menuItemList[pos].Data : null;
		}

		public bool RemoveButton(int index)
		{
			if(_menuItemList.Count <= index)
				return false;
			_menuItemList.RemoveAt(index);
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
			return _menuItemList;
		}

		public List<Vector2> GetButtonPositions()
		{
			List<Vector2> vectorList = new List<Vector2>();

			Vector2 drawPosition = Position - (_size / 2) + Margin;
			foreach (MenuItem item in _menuItemList)
			{
				Vector2 itemSize = item.Data.GetSize();
				vectorList.Add(new Vector2(Position.X, drawPosition.Y + item.UpperMargin + itemSize.Y / 2));
				drawPosition.Y += itemSize.Y + item.LowerMargin + item.UpperMargin + BaseButtonDistance;
			}

			return vectorList;
		}
	}
}
