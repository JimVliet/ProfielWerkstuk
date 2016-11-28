using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.Events
{
	public class InputManager
	{
		public Vector2 LastLeftClick;
		public Game1 Game;
		private bool _validLeftClick;
		public GridElement DragElement;
		public MouseState MouseState;
		public KeyboardState KeyboardState;
		public MouseState OldMouseState;
		public KeyboardState OldKeyboardState;
		public List<Menu> EscapeTriggerList = new List<Menu>();
		public bool AllowDragging = true;

		public InputManager(Game1 game)
		{
			Game = game;
		}

		public void Update()
		{
			KeyboardState = Keyboard.GetState();
			MouseState = Mouse.GetState();
			Vector2 clickLocation = new Vector2(MouseState.X, MouseState.Y);

			CheckClickEvent(clickLocation);
			CheckRightClickEvent(clickLocation);
			CheckMouseHover(clickLocation);
			CheckScroll();
			EscapePushed();

			OldMouseState = MouseState;
			OldKeyboardState = KeyboardState;
		}

		public void CheckRightClickEvent(Vector2 clickLocation)
		{
			if (MouseState.RightButton == ButtonState.Pressed)
			{
				if (OldMouseState.RightButton == ButtonState.Pressed)
					RightMouseHold(clickLocation);
				else
					RightMouseClick(clickLocation);
			}
			else if (OldMouseState.RightButton == ButtonState.Pressed)
				RightMouseRelease(clickLocation);
		}

		public void RightMouseRelease(Vector2 clickLocation)
		{
			Game.Grid.GridHoldType = GridElementType.Null;
		}

		public void RightMouseHold(Vector2 clickLocation)
		{
			Game.Grid.GridHoldClick(clickLocation);
		}

		public void RightMouseClick(Vector2 clickLocation)
		{
			
		}

		public void CheckMouseHover(Vector2 mouseLocation)
		{
			List<Menu> menuList = Game.UserInterface.MenuList;

			foreach (Menu menu in menuList)
			{
				bool disabled = menu.IsBeingDisabled || !menu.IsActive ||
				                !Utilities.IsPointWithin(mouseLocation, menu.GetTopLeft(), menu.GetLowerRight());

				List<MenuItem> itemList = menu.GetMenuItems();
				List<Vector2> buttonPositions = menu.GetButtonPositions();
				for (int i = 0; i < itemList.Count; i++)
				{
					IMenuItem item = itemList[i].Data;
					bool pointWithin = Utilities.IsPointWithin(mouseLocation, item.GetTopLeft(buttonPositions[i]),
						item.GetLowerRight(buttonPositions[i]));
					item.Hover(pointWithin && !disabled, buttonPositions[i], mouseLocation);
				}
			}
		}

		public void EscapePushed()
		{
			if (!(KeyboardState.IsKeyDown(Keys.Escape) && !OldKeyboardState.IsKeyDown(Keys.Escape)))
				return;

			foreach (Menu menu in EscapeTriggerList)
			{
				menu.MenuActivated?.Invoke(menu);
			}
		}

		public void CheckClickEvent(Vector2 clickLocation)
		{
			if(OldMouseState.LeftButton == ButtonState.Released &&
				MouseState.LeftButton == ButtonState.Pressed)
				LeftClickEvent(clickLocation);
			if (OldMouseState.LeftButton == ButtonState.Pressed)
			{
				if(MouseState.LeftButton == ButtonState.Released)
					LeftReleaseEvent(clickLocation);
				else if (_validLeftClick)
				{
					LeftDragEvent();
				}
			}
		}

		public void CheckScroll()
		{
			int deltaScroll = MouseState.ScrollWheelValue - OldMouseState.ScrollWheelValue;
			if (deltaScroll != 0)
				ScrollEvent(deltaScroll);
		}

		public void ScrollEvent(int deltaScroll)
		{
			Game.CameraManager.CheckScroll(deltaScroll);
		}

		public void LeftClickEvent(Vector2 clickLocation)
		{
			_validLeftClick = Game.UserInterface.ClickEvent(clickLocation);

			if (_validLeftClick && AllowDragging)
			{
				GridElement element = Game.Grid.GetGridElement(clickLocation);
				if (element != null && (element.Type == GridElementType.Start || element.Type == GridElementType.End))
				{
					DragElement = element;
				}
			}

			//Store the new lastLeftClick location
			LastLeftClick = clickLocation;
		}

		public void LeftReleaseEvent(Vector2 clickLocation)
		{
			if(!_validLeftClick || !AllowDragging)
				return;

			if (DragElement != null)
			{
				GridElement element = Game.Grid.GetGridElement(clickLocation);
				if (element != null && element.Type != GridElementType.End && element.Type != GridElementType.Start)
				{
					if(DragElement.Type == GridElementType.Start)
						Game.Grid.ChangeStartElement(element);
					else
						Game.Grid.ChangeEndElement(element);
				}

				DragElement = null;
			}
			else
			{
				Vector2 deltaClick = clickLocation - LastLeftClick;

				if(deltaClick.Length() > 2)
					return;

				Game.Grid.GridClicked(clickLocation);
			}

			
		}

		public void LeftDragEvent()
		{
			if (DragElement == null && AllowDragging)
				Game.CameraManager.CheckPanning(MouseState, OldMouseState);
		}
	}
}
