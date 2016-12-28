using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI.BaseClasses
{
	public class MenuContainer
	{
		private readonly UserInterfaceMenu _parent;
		private readonly List<BaseMenuElement> _menuElements = new List<BaseMenuElement>();

		public Vector2 PreferedSize = new Vector2();
		public Vector2 Offset;

		protected Vector2 InSize;
		public Vector2 Size
		{
			get { return InSize; }
			set
			{
				UpdateSize();
				InSize = value;
			}
		}

		public MenuContainer(UserInterfaceMenu parent)
		{
			_parent = parent;
		}

		public void AddToMenu()
		{
			_parent.AddMenuContainer(this);
		}

		public void AddMenuElement(BaseMenuElement element)
		{
			_menuElements.Add(element);
			UpdateSize();
		}

		private Vector2 GetMinimalSize()
		{
			float minHalfWidth = 0;
			float minHalfHeight = 0;

			foreach (var element in _menuElements)
			{
				Vector2 cornerOne = element.GetTopLeft();
				Vector2 cornerTwo = element.GetLowerRight();

				minHalfWidth = Math.Max(Math.Abs(cornerOne.X), minHalfWidth);
				minHalfHeight = Math.Max(Math.Abs(cornerOne.Y), minHalfHeight);

				minHalfWidth = Math.Max(Math.Abs(cornerTwo.X), minHalfWidth);
				minHalfHeight = Math.Max(Math.Abs(cornerTwo.Y), minHalfHeight);
			}

			return new Vector2(2 * minHalfWidth, 2 * minHalfHeight);
		}

		public void UpdateSize()
		{
			Vector2 minSize = GetMinimalSize();
			InSize = new Vector2(Math.Max(minSize.X, PreferedSize.X), Math.Max(minSize.Y, PreferedSize.Y));
			_parent.UpdateSize();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var element in _menuElements)
			{
				element.Draw(spriteBatch);
			}
		}

		protected Vector2 GetTopLeft()
		{
			return Offset - Size / 2;
		}

		protected Vector2 GetLowerRight()
		{
			return Offset + Size / 2;
		}

		public Vector2 GetPosition()
		{
			return _parent.Position + Offset;
		}

		public void MatchToContainerWidth(float maxWidth)
		{
			foreach (var element in _menuElements)
			{
				if(element.MatchToContainerWidth(maxWidth))
					InSize.X = maxWidth;
			}
		}

		public BaseMenuElement CheckHover(Vector2 mouseLocation)
		{
			foreach (var element in _menuElements)
			{
				if (element.IsPointWithin(mouseLocation))
					return element;
			}

			return null;
		}

		public bool IsPointWithin(Vector2 point)
		{
			Vector2 pos = GetPosition();

			return Utilities.IsPointWithin(point, pos - Size / 2, pos + Size / 2);
		}

		public void CheckLeftClick(Vector2 mouseLocation)
		{
			foreach (var element in _menuElements)
			{
				if (!element.IsPointWithin(mouseLocation))
					continue;

				element.LeftClickEvent();
				break;
			}
		}

		public EventHandlers GetEventHandlers()
		{
			return _parent.GetEventHandlers();
		}

		public void LeftReleaseEvent(Vector2 mouseLocation)
		{
			_menuElements.FirstOrDefault(element => element.IsPointWithin(mouseLocation))?.LeftReleaseEvent();
		}
	}
}
