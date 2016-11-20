using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProfielWerkstuk.Scripts.Grid;
using ProfielWerkstuk.Scripts.GUI;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.Events
{
	public class InputManager
	{
		public Vector2 LastLeftClick;
		public Game1 Game;
		private bool _validLeftClick;
		public MouseState MouseState;
		public KeyboardState KeyboardState;
		public MouseState OldMouseState;
		public KeyboardState OldKeyboardState;
		public List<Menu> EscapeTriggerList = new List<Menu>();

		public InputManager(Game1 game)
		{
			Game = game;
		}

		public void Update()
		{
			KeyboardState = Keyboard.GetState();
			MouseState = Mouse.GetState();

			CheckClickEvent();
			CheckScroll();
			EscapePushed();
			CheckMouseHover();

			OldMouseState = MouseState;
			OldKeyboardState = KeyboardState;
		}

		public void CheckMouseHover()
		{
			Vector2 mouseLocation = new Vector2(MouseState.X, MouseState.Y);
			List<Menu> menuList = Game.UserInterface.MenuList;

			//Disable all old hovers
			foreach (Menu menu in menuList)
			{
				List<MenuItem> itemList = menu.GetMenuItems();
				foreach (MenuItem item in itemList)
				{
					Button button = item.Data;
					button.IsBeingHovered = false;
				}
			}

			foreach (Menu menu in menuList)
			{
				if (menu.IsBeingDisabled || !menu.IsActive || !Utilities.IsPointWithin(mouseLocation, menu.GetTopLeft(), menu.GetLowerRight()))
					continue;

				List<MenuItem> itemList = menu.GetMenuItems();
				List<Vector2> buttonPositions = menu.GetButtonPositions();
				for (int i = 0; i < itemList.Count; i++)
				{
					Button button = itemList[i].Data;
					if (Utilities.IsPointWithin(mouseLocation, button.GetTopLeft(buttonPositions[i]),
						button.GetLowerRight(buttonPositions[i])))
						button.IsBeingHovered = true;
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

		public void CheckClickEvent()
		{
			if(OldMouseState.LeftButton == ButtonState.Released &&
				MouseState.LeftButton == ButtonState.Pressed)
				LeftClickEvent();
			if (OldMouseState.LeftButton == ButtonState.Pressed)
			{
				if(MouseState.LeftButton == ButtonState.Released)
					LeftReleaseEvent();
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

		public void LeftClickEvent()
		{
			Vector2 clickLocation = new Vector2(MouseState.X, MouseState.Y);

			_validLeftClick = Game.UserInterface.ClickEvent(clickLocation);
			//Store the new lastLeftClick location
			LastLeftClick = clickLocation;
		}

		public void LeftReleaseEvent()
		{
			if(!_validLeftClick)
				return;

			Vector2 clickLocation = new Vector2(MouseState.X, MouseState.Y);
			Vector2 deltaClick = clickLocation - LastLeftClick;

			if(deltaClick.Length() > 2)
				return;

			GridElement element = Game.Grid.GetGridElement(clickLocation);
			if (element != null)
			{
				switch (element.Type)
				{
					case GridElementType.Empty:
						element.Type = GridElementType.Solid;
						break;
					case GridElementType.Solid:
						element.Type = GridElementType.Empty;
						break;
				}
			}
		}

		public void LeftDragEvent()
		{
			Game.CameraManager.CheckPanning(MouseState, OldMouseState);
		}
	}
}
