using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.States;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class UserInterfaceManager
	{
		public SpriteFont Font28;
		public SpriteFont Font24;
		public SpriteFont Font16;
		public Game1 Game;
		private BaseUserInterfaceState _uiState;
		private readonly GridMapState _gridMapState;
		private readonly MainMenuState _mainMenuState;

		public UserInterfaceManager(Game1 game)
		{
			Game = game;
			_gridMapState = new GridMapState(game);
			_mainMenuState = new MainMenuState(game);
			SwitchToState(UserInterfaceStates.GridMap);
		}

		public void Setup()
		{
			_mainMenuState.Setup();
			_gridMapState.Setup();
		}

		/// <summary>
		/// This method needs to be used when the user has clicked somewhere
		/// </summary>
		/// <returns>Returns true if the click location is outside any menus</returns>
		public bool ClickEvent(Vector2 clickLocation)
		{
			foreach (Menu menu in _uiState.MenuList)
			{
				if (!Utilities.IsPointWithin(clickLocation, menu.GetTopLeft(), menu.GetLowerRight()))
					continue;
				
				List<MenuItem> itemList = menu.GetMenuItems();
				List<Vector2> buttonPositions = menu.GetButtonPositions();
				for (int i = 0; i < itemList.Count; i++)
				{
					IMenuItem item = itemList[i].Data;
					if (!Utilities.IsPointWithin(clickLocation, item.GetTopLeft(buttonPositions[i]), item.GetLowerRight(buttonPositions[i])))
						continue;

					item.ClickedEvent?.Invoke(item, clickLocation);
					return false;
				}
			}

			return AllowClicking();
		}

		public bool OutsideMenus(Vector2 clickLocation)
		{
			return !_uiState.MenuList.All(menu => Utilities.IsPointWithin(clickLocation, menu.GetTopLeft(), menu.GetLowerRight()));
		}

		public void CheckMouseHover(Vector2 mouseLocation)
		{
			List<Menu> menuList = _uiState.MenuList;

			foreach (Menu menu in menuList)
			{
				bool enabled = Utilities.IsPointWithin(mouseLocation, menu.GetTopLeft(), menu.GetLowerRight());

				List<MenuItem> itemList = menu.GetMenuItems();
				List<Vector2> buttonPositions = menu.GetButtonPositions();
				for (int i = 0; i < itemList.Count; i++)
				{
					IMenuItem item = itemList[i].Data;
					bool pointWithin = Utilities.IsPointWithin(mouseLocation, item.GetTopLeft(buttonPositions[i]),
						item.GetLowerRight(buttonPositions[i]));
					item.Hover(pointWithin && enabled, buttonPositions[i], mouseLocation);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			_uiState.Draw(spriteBatch, gameTime);
		}

		public void SwitchToState(UserInterfaceStates state)
		{
			switch (state)
			{
				case UserInterfaceStates.GridMap:
					_uiState = _gridMapState;
					return;
				case UserInterfaceStates.MainMenu:
					_uiState = _mainMenuState;
					return;
			}
		}

		public void SwitchState()
		{
			switch (_uiState.StateType)
			{
				case UserInterfaceStates.GridMap:
					_uiState = _mainMenuState;
					return;
				case UserInterfaceStates.MainMenu:
					_uiState = _gridMapState;
					return;
			}
		}

		public bool AllowClicking()
		{
			return _uiState.AllowClicking;
		}
	}
}
