using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class UserInterfaceManager
	{
		public SpriteFont Font24;
		public SpriteFont Font16;
		public SpriteFont Font14;
		public SpriteFont BoldFont16;
		public readonly ProfielWerkstuk Game;
		private BaseUserInterfaceState _uiState;

		private readonly GridMapState _gridMapState;
		private readonly MainMenuState _mainMenuState;
		private readonly KeybindingsState _keybindingsState;
		private BaseMenuElement _hoverElement;

		public UserInterfaceManager(ProfielWerkstuk game)
		{
			Game = game;
			_gridMapState = new GridMapState(game, this);
			_mainMenuState = new MainMenuState(game, this);
			_keybindingsState = new KeybindingsState(game, this);
			SwitchToState(UserInterfaceStates.GridMap);
		}

		public void Setup()
		{
			_mainMenuState.Setup();
			_gridMapState.Setup();
			_keybindingsState.Setup();
		}

		public void Update(GameTime gameTime)
		{
			_uiState?.Update(gameTime);
		}

		public UserInterfaceMenu GetMenu(Vector2 mouseLocation)
		{
			return _uiState.UserInterfaceMenuList.FirstOrDefault(menu => menu.IsPointWithin(mouseLocation));
		}

		public void CheckMouseHover(Vector2 mouseLocation)
		{
			foreach (var menu in _uiState.UserInterfaceMenuList)
			{
				if (!menu.IsPointWithin(mouseLocation))
					continue;

				BaseMenuElement element = menu.CheckHover(mouseLocation);

				if (element == null && _hoverElement != null)
				{
					_hoverElement.UnHover();
					_hoverElement = null;
				}

				if (element == null || element.Equals(_hoverElement))
					return;

				_hoverElement?.UnHover();
				_hoverElement = element;
				element.Hover();
				return;
			}

			_hoverElement?.UnHover();
			_hoverElement = null;
		}

		public void LeftReleaseEvent(Vector2 mouseLocation)
		{
			UserInterfaceMenu menu = GetMenu(mouseLocation);
			menu?.LeftReleaseEvent(mouseLocation);
		}

		public bool ClickEvent(Vector2 mouseLocation)
		{
			UserInterfaceMenu menu = GetMenu(mouseLocation);
			if (menu == null)
				return true;

			menu.CheckLeftClick(mouseLocation);
			return false;
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
				case UserInterfaceStates.Keybindings:
					_uiState = _keybindingsState;
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
				case UserInterfaceStates.Keybindings:
					_uiState = _mainMenuState;
					return;
			}
		}

		public bool AllowClicking()
		{
			return _uiState.AllowClicking;
		}
	}
}
