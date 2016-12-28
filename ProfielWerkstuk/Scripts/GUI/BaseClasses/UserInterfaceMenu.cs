using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.States;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI.BaseClasses
{
	public class UserInterfaceMenu
	{
		protected readonly BaseUserInterfaceState State;
		public Vector2 Position;
		public Color MenuColor = new Color(34, 34, 34);
		public Vector2 Margin = new Vector2(10, 10);
		public float BaseButtonDistance = 10f;
		public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.None;
		public VerticalAlignment VerticalAlignment = VerticalAlignment.None;

		private readonly List<MenuContainer> _menuContainerList = new List<MenuContainer>();

		private Vector2 _size;
		private Vector2 Size
		{
			get { return _size; }
			set
			{
				_size = value;
				SizeChanged();
			}
		}

		protected UserInterfaceMenu(BaseUserInterfaceState state, Vector2 pos)
		{
			Position = pos;
			State = state;
			_size = GetMenuSize();
		}

		protected UserInterfaceMenu(BaseUserInterfaceState state, HorizontalAlignment horizontal, VerticalAlignment vertical)
		{
			HorizontalAlignment = horizontal;
			VerticalAlignment = vertical;
			State = state;
			Size = GetMenuSize();

			state.AddUserInterfaceMenu(this);
		}

		public void AddToState()
		{
			State.AddUserInterfaceMenu(this);
		}

		private void SizeChanged()
		{
			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					Position.X = Size.X/2;
					break;
				case HorizontalAlignment.Center:
					Position.X = Utilities.GetWindowWidth(State.Game)/2f;
					break;
				case HorizontalAlignment.Right:
					Position.X = Utilities.GetWindowWidth(State.Game) - Size.X / 2;
					break;
			}

			switch (VerticalAlignment)
			{
				case VerticalAlignment.Top:
					Position.Y = Size.Y / 2;
					break;
				case VerticalAlignment.Center:
					Position.Y = Utilities.GetWindowHeight(State.Game) / 2f;
					break;
				case VerticalAlignment.Bottom:
					Position.Y = Utilities.GetWindowHeight(State.Game) - Size.Y / 2;
					break;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 drawPos = Position - _size/2;
			//Prevents weird anti-aliasing
			drawPos.X = (int) drawPos.X;
			drawPos.Y = (int) drawPos.Y;

			MonoGame.Extended.Shapes.SpriteBatchExtensions.FillRectangle(spriteBatch, drawPos, _size, MenuColor);

			if (_menuContainerList.Count == 0)
				return;
			foreach (var container in _menuContainerList)
			{
				container.Draw(spriteBatch);
			}
		}

		private Vector2 GetMenuSize()
		{
			float horizontalSize = 0;
			float verticalSize = (_menuContainerList.Count - 1) * BaseButtonDistance;

			foreach (var item in _menuContainerList)
			{
				Vector2 buttonSize = item.Size;
				horizontalSize = Math.Max(buttonSize.X, horizontalSize);
				verticalSize += buttonSize.Y;
			}

			return new Vector2(Math.Max(20f, horizontalSize), Math.Max(20f, verticalSize)) + 2 * Margin;
		}

		public void UpdateSize()
		{
			Size = GetMenuSize();
			ResetContainerWidth();
		}

		private void UpdateOffsets()
		{
			float height = -Size.Y/2 + Margin.Y;

			foreach (var container in _menuContainerList)
			{
				container.Offset.Y = height + container.Size.Y/2;

				height += BaseButtonDistance + container.Size.Y;
			}
		}

		public void AddMenuContainer(MenuContainer container)
		{
			_menuContainerList.Add(container);
			UpdateSize();
			UpdateOffsets();
			ResetContainerWidth();
		}

		private void ResetContainerWidth()
		{
			if (_menuContainerList.Count == 0)
				return;

			float maxWidth = _menuContainerList.Select(item => item.Size.X).Concat(new float[] {0}).Max();

			foreach (var item in _menuContainerList)
			{
				item.MatchToContainerWidth(maxWidth);
			}
		}

		public BaseMenuElement CheckHover(Vector2 mouseLocation)
		{
			return (from container in _menuContainerList where container.IsPointWithin(mouseLocation)
					select container.CheckHover(mouseLocation)).FirstOrDefault(element => element != null);
		}

		public void CheckLeftClick(Vector2 mouseLocation)
		{
			foreach (var container in _menuContainerList)
			{
				if (!container.IsPointWithin(mouseLocation))
					continue;
				container.CheckLeftClick(mouseLocation);
				break;
			}
		}

		public bool IsPointWithin(Vector2 point)
		{
			return Utilities.IsPointWithin(point, Position - Size / 2, Position + Size / 2);
		}

		public EventHandlers GetEventHandlers()
		{
			return State.GetEventHandlers();
		}

		public void LeftReleaseEvent(Vector2 mouseLocation)
		{
			_menuContainerList.FirstOrDefault(element => element.IsPointWithin(mouseLocation))?.LeftReleaseEvent(mouseLocation);
		}
	}

	public enum HorizontalAlignment
	{
		Left, Center, Right, None
	}

	public enum VerticalAlignment
	{
		Top, Center, Bottom, None
	}
}
