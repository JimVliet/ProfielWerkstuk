using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI.Menus;

namespace ProfielWerkstuk.Scripts.GUI.States
{
	public class GridMapState : BaseUserInterfaceState
	{
		public GridMapState(ProfielWerkstuk game, UserInterfaceManager manager) : base(game, manager, UserInterfaceStates.GridMap)
		{

		}

		public override void Setup()
		{
			SetupAlgorithmMenu();
			SetupEscapeButtonMenu();
			SetupOptionMenu();
			SetupToolTips();
			SetupAlgorithmControls();
			SetupGridTypePreview();
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			GridElement element = Game.Grid.GetGridElement(Game.InputManager.GetMouseLocation());
			int explored = Game.AlgorithmManager.GetExplored();
			Game.EventHandlers.TextUpdate?.Invoke(element, explored);

			DrawMenus(spriteBatch, gameTime);
		}

		private void SetupGridTypePreview()
		{
			GridTypePreviewMenu gridPreviewMenu = new GridTypePreviewMenu(this);
			gridPreviewMenu.AddToState();
		}

		private void SetupAlgorithmMenu()
		{
			AlgorithmMenu algorithmMenu = new AlgorithmMenu(this);
			algorithmMenu.AddToState();
		}

		private void SetupEscapeButtonMenu()
		{
			EscapeButtonMenu escapeButtonMenu = new EscapeButtonMenu(this);
			escapeButtonMenu.AddToState();
		}

		private void SetupOptionMenu()
		{
			OptionMenu optionMenu = new OptionMenu(this);
			optionMenu.AddToState();
		}

		private void SetupToolTips()
		{
			ToolTipsMenu toolTips = new ToolTipsMenu(this);
			toolTips.AddToState();
		}

		private void SetupAlgorithmControls()
		{
			AlgorithmControls algorithmControls = new AlgorithmControls(this, Game.TextureManager);
			algorithmControls.AddToState();
		}
	}
}
