using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;

namespace ProfielWerkstuk.Scripts.GUI.States
{
	public class BaseUserInterfaceState
	{
		public readonly List<UserInterfaceMenu> UserInterfaceMenuList = new List<UserInterfaceMenu>();
		public readonly ProfielWerkstuk Game;
		public readonly UserInterfaceManager UiManager;
		public readonly UserInterfaceStates StateType;
		public bool AllowClicking = true;

		internal BaseUserInterfaceState(ProfielWerkstuk game, UserInterfaceManager uiManager, UserInterfaceStates type)
		{
			Game = game;
			UiManager = uiManager;
			StateType = type;
		}

		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			DrawMenus(spriteBatch, gameTime);
		}

		protected void DrawMenus(SpriteBatch spriteBatch, GameTime gameTime)
		{
			foreach (var menu in UserInterfaceMenuList)
			{
				menu.Draw(spriteBatch);
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			
		}

		public virtual void Setup()
		{
			
		}

		public void AddUserInterfaceMenu(UserInterfaceMenu uiMenu)
		{
			UserInterfaceMenuList.Add(uiMenu);
		}

		public EventHandlers GetEventHandlers()
		{
			return UiManager.Game.EventHandlers;
		} 
	}

	public enum UserInterfaceStates
	{
		GridMap, MainMenu, Keybindings
	}
}
