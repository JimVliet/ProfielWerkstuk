using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.GUI.States
{
	public class BaseUserInterfaceState
	{
		public List<Menu> MenuList = new List<Menu>();
		public Game1 Game;
		public UserInterfaceStates StateType;
		public bool AllowClicking = true;

		internal BaseUserInterfaceState(Game1 game, UserInterfaceStates type)
		{
			Game = game;
			StateType = type;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			foreach (Menu menu in MenuList)
			{
				menu.Draw(spriteBatch);
			}
		}

		public void Update(GameTime gameTime)
		{
			
		}

		public void Setup()
		{
			
		}

		public Menu CreateMenu(Vector2 position)
		{
			Menu newMenu = new Menu(position);
			MenuList.Add(newMenu);
			return newMenu;
		}
	}

	public enum UserInterfaceStates
	{
		GridMap, MainMenu
	}
}
