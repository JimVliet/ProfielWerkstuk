using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI.BaseClasses
{
	public abstract class BaseMenuElement
	{
		protected readonly MenuContainer ParentContainer;
		protected bool MatchToContainer;

		private Vector2 _inPreferedSize;
		public Vector2 PreferedSize
		{
			get { return _inPreferedSize; }
			set
			{
				_inPreferedSize = value;
				UpdateSize();
			}
		}

		private Vector2 _inOffset;
		public Vector2 Offset
		{
			get { return _inOffset; }
			set
			{
				_inOffset = value;
				UpdateSize();
			}
		}

		private Vector2 _inSize;
		public Vector2 Size
		{
			get { return _inSize; }
			set
			{
				_inSize = value;
				UpdateSize();
			}
		}

		protected Vector2 InPadding;
		public Vector2 Padding
		{
			get { return InPadding; }
			set
			{
				InPadding = value;
				UpdateSize();
			}
		}

		protected BaseMenuElement(MenuContainer parentContainer)
		{
			ParentContainer = parentContainer;
		}

		public void AddToContainer()
		{
			ParentContainer.AddMenuElement(this);
		}

		protected abstract Vector2 GetMinimalSize();
		public abstract void Draw(SpriteBatch spriteBatch);

		protected void UpdateSize()
		{
			Vector2 minSize = GetMinimalSize();
			_inSize = new Vector2(Math.Max(minSize.X, PreferedSize.X), Math.Max(minSize.Y, PreferedSize.Y));
			ParentContainer.UpdateSize();
		}

		public virtual Vector2 GetTopLeft()
		{
			return Offset - Size/2;
		}

		public virtual Vector2 GetLowerRight()
		{
			return Offset + Size / 2;
		}

		public virtual bool IsPointWithin(Vector2 point)
		{
			Vector2 pos = GetPosition();

			return Utilities.IsPointWithin(point, GetTopLeft() + pos - Offset, GetLowerRight() + pos - Offset);
		}

		protected virtual Vector2 GetPosition()
		{
			return ParentContainer.GetPosition() + Offset;
		}

		/// <summary>
		/// Returns true if the menuElement has changed size
		/// </summary>
		/// <param name="maxWidth"></param>
		/// <returns></returns>
		public virtual bool MatchToContainerWidth(float maxWidth)
		{
			if(!MatchToContainer)
				return false;
			_inSize.X = maxWidth;
			return true;
		}

		public virtual void Hover()
		{
			
		}

		public virtual void UnHover()
		{

		}

		public virtual void LeftClickEvent()
		{
			
		}

		protected EventHandlers GetEventHandlers()
		{
			return ParentContainer.GetEventHandlers();
		}

		public virtual void LeftReleaseEvent()
		{
			
		}
	}
}
