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
				ClickedEvent = (button, location) =>
				{
					Game.Exit();
				}
			};
			exitButton.Size = new Vector2(250f, exitButton.Size.Y);

			Button backButton = new Button(Font28, new Vector2(), "Back")
			{
				ClickedEvent = (button, location) =>
				{
					mainMenu.MenuActivated?.Invoke(mainMenu);
				}
			};

			mainMenu.AddMenuItem(exitButton);
			mainMenu.AddMenuItem(backButton);

			//Setup option panel
			Menu optionMenu = CreateMenu(new Vector2());
			optionMenu.Margin = new Vector2(10, 10);
			optionMenu.BaseButtonDistance = 10f;
			
			//Add button for dijkstra
			Button dijkstraButton = new Button(Font16, new Vector2(200, 20), "Dijkstra")
			{
				Padding = {Y = 10f}
			};
			optionMenu.AddMenuItem(dijkstraButton);

			//Add button for A*
			Button aStar = new Button(Font16, new Vector2(), "A*")
			{
				Padding = { Y = 10f }
			};
			optionMenu.AddMenuItem(aStar);

			optionMenu.Position = new Vector2(windowWidth - optionMenu.GetMenuSize().X / 2,
				optionMenu.GetMenuSize().Y / 2);

			//Add escape notifier
			Menu escapeMenu = CreateMenu(new Vector2());
			escapeMenu.Margin = new Vector2(5,5);

			Button escapeButton = new Button(Font16, new Vector2(0, 0), "Menu")
			{
				Padding = new Vector2(5, 5),
				ClickedEvent = (button, location) =>
				{
					mainMenu.MenuActivated?.Invoke(mainMenu);
				}
			};
			escapeMenu.AddMenuItem(escapeButton);
			escapeMenu.Position = escapeMenu.GetMenuSize()/2;

			//Setup itemMenu
			Menu itemMenu = CreateMenu(new Vector2(5,5));

			//Create new button
			Button kekButton = new Button(Font28, new Vector2(0, 0), "kek");

			//Add new infoItem
			InfoItem infoItem = new InfoItem(new Vector2());

			//Add textElements to infoItem
			InfoTextElement textElement = new InfoTextElement(infoItem, new Vector2(80, 80), new Vector2(), "Testing leleleelelelelelelele", Font16);
			infoItem.AddInfoText(textElement);
			InfoTextElement textElementKek = new InfoTextElement(infoItem, new Vector2(-80, -80), new Vector2(), "Testing", Font16);
			infoItem.AddInfoText(textElementKek);

			//Finish itemMenu setup
			itemMenu.AddMenuItem(infoItem);
			itemMenu.AddMenuItem(kekButton);
			itemMenu.Position = new Vector2(windowWidth / 2, windowHeight / 2);
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
					IMenuItem item = itemList[i].Data;
					if (Utilities.IsPointWithin(clickLocation, item.GetTopLeft(buttonPositions[i]), item.GetLowerRight(buttonPositions[i])))
					{
						item.ClickedEvent?.Invoke(item, clickLocation);
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
