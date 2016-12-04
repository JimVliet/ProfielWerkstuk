using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI.States
{
	public class GridMapState : BaseUserInterfaceState
	{
		public GridMapState(Game1 game) : base(game, UserInterfaceStates.GridMap)
		{

		}

		public new void Setup()
		{
			SetupOptionMenu();
			SetupEscapeButtonMenu();
			SetupControlMenu();
		}

		public new void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			base.Draw(spriteBatch, gameTime);


		}

		private void SetupOptionMenu()
		{
			float windowWidth = Utilities.GetWindowWidth(Game);

			//Setup option panel
			Menu optionMenu = CreateMenu(new Vector2());
			optionMenu.Margin = new Vector2(10, 10);
			optionMenu.BaseButtonDistance = 10f;

			//Add button for dijkstra
			Button dijkstraButton = new Button(Game.UserInterface.Font16, new Vector2(200, 20), "Dijkstra")
			{
				Padding = { Y = 10f },
				ClickedEvent = DijkstraButtonClick
			};
			optionMenu.AddMenuItem(dijkstraButton);

			//Add button for A*
			Button aStar = new Button(Game.UserInterface.Font16, new Vector2(), "A*")
			{
				ClickedEvent = AStarButtonClick,
				Padding = { Y = 10f }
			};
			optionMenu.AddMenuItem(aStar);

			optionMenu.Position = new Vector2(windowWidth - optionMenu.GetMenuSize().X / 2,
				optionMenu.GetMenuSize().Y / 2);
		}

		private void SetupEscapeButtonMenu()
		{
			//Add button for manual opening of the main menu
			Menu escapeMenu = CreateMenu(new Vector2());
			escapeMenu.Margin = new Vector2(5, 5);

			Button escapeButton = new Button(Game.UserInterface.Font16, new Vector2(0, 0), "Menu")
			{
				Padding = new Vector2(5, 5),
				ClickedEvent = EscapeButtonClick
			};
			escapeMenu.AddMenuItem(escapeButton);
			escapeMenu.Position = escapeMenu.GetMenuSize() / 2;
		}

		private void SetupControlMenu()
		{
			float windowWidth = Utilities.GetWindowWidth(Game);
			float windowHeight = Utilities.GetWindowHeight(Game);

			//Setup itemMenu
			Menu controlMenu = CreateMenu(new Vector2());
			controlMenu.Margin = new Vector2(10, 10);
			controlMenu.BaseButtonDistance = 10;

			//Add new infoItem
			InfoItem infoItem = new InfoItem(new Vector2());

			//Add textElements to infoItem
			InfoTextElement calculateElement = new InfoTextElement(new Vector2(), new Vector2(), "Selecteer een algoritme", Game.UserInterface.Font16)
			{
				Padding = new Vector2(0, 0)
			};
			infoItem.AddInfoText(calculateElement);

			Game.CustomEvents.CalculateEvent += calculating =>
			{
				calculateElement.Text = calculating ? "Calculating..." : Game.AlgorithmManager.CurrentAlgorithm.GetName();
			};

			//AllowDiagonal button
			Button diagonal = new Button(Game.UserInterface.Font16, new Vector2(200, 0), "Diagonaal", Color.Red)
			{
				ButtonHoverColor = new Color(255, 30, 30),
				ClickedEvent = DiagonalButtonClick
			};

			//Finish itemMenu setup
			controlMenu.AddMenuItem(infoItem);
			controlMenu.AddMenuItem(diagonal);
			controlMenu.Position = new Vector2(windowWidth / 2, windowHeight - controlMenu.GetMenuSize().Y / 2);
		}

		private void DijkstraButtonClick(IMenuItem item, Vector2 location)
		{
			Game.AlgorithmManager.Calculate();
		}

		private void EscapeButtonClick(IMenuItem item, Vector2 location)
		{
			Game.UserInterface.SwitchToState(UserInterfaceStates.MainMenu);
		}

		private void AStarButtonClick(IMenuItem item, Vector2 location)
		{
			//Game.AlgorithmManager.Calculate();
		}

		private void DiagonalButtonClick(IMenuItem item, Vector2 location)
		{
			if (Game.AlgorithmManager.AllowDiagonal)
			{
				Game.AlgorithmManager.AllowDiagonal = false;
				((Button)item).ButtonColor = Color.Red;
				((Button)item).ButtonHoverColor = new Color(255, 30, 30);
			}
			else
			{
				Game.AlgorithmManager.AllowDiagonal = true;
				((Button)item).ButtonColor = Color.Green;
				((Button)item).ButtonHoverColor = new Color(40, 128, 40);
			}
		}
	}
}
