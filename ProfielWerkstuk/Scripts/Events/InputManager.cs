using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI;

namespace ProfielWerkstuk.Scripts.Events
{
	public class InputManager
	{
		public Vector2 LastLeftClick;
		public Game1 Game;
		public GridElement DragElement;
		public MouseState MouseState;
		public KeyboardState KeyboardState;
		public MouseState OldMouseState;
		public KeyboardState OldKeyboardState;
		public List<Menu> EscapeTriggerList = new List<Menu>();
		private bool _outsideMenuClick;

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
			Game.UserInterface.CheckMouseHover(clickLocation);
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
			if(Game.UserInterface.OutsideMenus(clickLocation) && Game.UserInterface.AllowClicking())
				Game.Grid.GridHoldClick(clickLocation);
		}

		public void RightMouseClick(Vector2 clickLocation)
		{
			
		}

		public void EscapePushed()
		{
			if (!(KeyboardState.IsKeyDown(Keys.Escape) && !OldKeyboardState.IsKeyDown(Keys.Escape)))
				return;
			Game.UserInterface.SwitchState();
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
				else if (Game.UserInterface.OutsideMenus(clickLocation))
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
			if(Game.UserInterface.AllowClicking())
				Game.CameraManager.CheckScroll(deltaScroll);
		}

		public void LeftClickEvent(Vector2 clickLocation)
		{
			_outsideMenuClick = Game.UserInterface.ClickEvent(clickLocation);

			if (_outsideMenuClick && Game.UserInterface.AllowClicking())
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
			if(!_outsideMenuClick || !Game.UserInterface.AllowClicking())
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
			if (DragElement == null && Game.UserInterface.AllowClicking())
				Game.CameraManager.CheckPanning(MouseState, OldMouseState);
		}
	}
}
