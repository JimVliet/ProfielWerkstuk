using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class UserInterfaceManager
	{
		public List<Menu> MenuList = new List<Menu>();
		public SpriteFont Font28;
		public SpriteFont Font24;
		public SpriteFont Font16;
		public Game1 Game;

		public UserInterfaceManager(Game1 game)
		{
			Game = game;
		}

		public void Setup()
		{
			float windowWidth = Game.Graphics.PreferredBackBufferWidth;
			float windowHeight = Game.Graphics.PreferredBackBufferHeight;

			//Setup mainmenu
			Menu mainMenu = CreateMenu(new Vector2(windowWidth / 2f, windowHeight / 2f));
			mainMenu.MenuActivated = main =>
			{
				main.IsActive = !main.IsActive;

				foreach (Menu menu in MenuList)
				{
					if (menu != main)
						menu.IsBeingDisabled = main.IsActive;
				}
			};
			Game.InputManager.EscapeTriggerList.Add(mainMenu);
			mainMenu.AllowClicking = false;
			mainMenu.IsActive = false;

			Button exitButton = new Button(Font28, new Vector2(), "Exit")
			{
				ButtonSize = { X = 250f },
				ButtonClickedEvent = (button, location) =>
				{
					Game.Exit();
				}
			};

			Button backButton = new Button(Font28, new Vector2(), "Back")
			{
				ButtonClickedEvent = (button, location) =>
				{
					mainMenu.MenuActivated?.Invoke(mainMenu);
				}
			};

			mainMenu.AddButton(exitButton);
			mainMenu.AddButton(backButton);

			//Setup option panel
			Menu optionMenu = CreateMenu(new Vector2());
			optionMenu.Margin = new Vector2(10, 10);
			optionMenu.BaseButtonDistance = 10f;
			
			//Add button for dijkstra
			Button dijkstraButton = new Button(Font16, new Vector2(200, 20), "Dijkstra")
			{
				Padding = {Y = 10f}
			};
			optionMenu.AddButton(dijkstraButton);

			//Add button for A*
			Button aStar = new Button(Font16, new Vector2(), "A*")
			{
				Padding = { Y = 10f }
			};
			optionMenu.AddButton(aStar);

			optionMenu.Position = new Vector2(windowWidth - (optionMenu.GetMenuSize().X / 2),
				optionMenu.GetMenuSize().Y / 2);

			//Add escape notifier
			Menu escapeMenu = CreateMenu(new Vector2());
			escapeMenu.Margin = new Vector2(5,5);

			Button escapeButton = new Button(Font16, new Vector2(0, 0), "Menu")
			{
				Padding = new Vector2(5, 5),
				ButtonClickedEvent = (button, location) =>
				{
					mainMenu.MenuActivated?.Invoke(mainMenu);
				}
			};
			escapeMenu.AddButton(escapeButton);
			escapeMenu.Position = escapeMenu.GetMenuSize()/2;
		}

		/// <summary>
		/// This method needs to be used when the user has clicked somewhere
		/// </summary>
		/// <returns>Returns true if the click location is outside any menus</returns>
		public bool ClickEvent(Vector2 clickLocation)
		{
			bool isOutsideMenus = true;

			foreach (Menu menu in MenuList)
			{
				if (menu.IsBeingDisabled || !menu.IsActive || !Utilities.IsPointWithin(clickLocation, menu.GetTopLeft(), menu.GetLowerRight()))
					continue;
				
				List<MenuItem> itemList = menu.GetMenuItems();
				List<Vector2> buttonPositions = menu.GetButtonPositions();
				for (int i = 0; i < itemList.Count; i++)
				{
					Button button = itemList[i].Data;
					if (Utilities.IsPointWithin(clickLocation, button.GetTopLeft(buttonPositions[i]), button.GetLowerRight(buttonPositions[i])))
					{
						button.ButtonClickedEvent?.Invoke(button, clickLocation);
					}
							
				}

				isOutsideMenus = false;
			}

			return isOutsideMenus;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Menu menu in MenuList)
			{
				if(!menu.IsActive || menu.IsBeingDisabled)
					continue;
				menu.Draw(spriteBatch);
			}
		}

		public Menu CreateMenu(Vector2 position)
		{
			Menu newMenu = new Menu(position);
			MenuList.Add(newMenu);
			return newMenu;
		}
	}
}
