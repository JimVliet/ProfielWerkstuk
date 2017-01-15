using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class GridTypePreviewMenu : UserInterfaceMenu
	{
		public GridTypePreviewMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Center, VerticalAlignment.Top)
		{
			SetupPreview();
		}

		private void SetupPreview()
		{
			MenuContainer container = new MenuContainer(this);

			PreviewName previewName = new PreviewName(container, new Vector2(), "Solid", State.UiManager.Font14)
			{
				Padding = new Vector2(20, 10),
				Offset = new Vector2(-35, 0)
			};
			previewName.AddToContainer();

			GridPreview preview = new GridPreview(container, new Vector2(32, 32))
			{
				Padding = new Vector2(10),
				Offset = new Vector2(38, 0)
			};

			preview.AddToContainer();
			container.AddToMenu();
		}
	}

	internal class GridPreview : ColoredMenuElement
	{
		public GridPreview(MenuContainer parentContainer, Vector2 preview) : base(parentContainer, preview)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().GridPreviewClicked?.Invoke();
		}

		protected override Color GetPreviewColor()
		{
			switch (GetEventHandlers().GetPreviewType?.Invoke())
			{
				case GridElementType.Empty:
					return Color.WhiteSmoke;
				case GridElementType.Solid:
					return Color.DarkGray;
				case GridElementType.Forest:
					return Color.DarkGreen;
				case GridElementType.River:
					return new Color(86, 122, 158);
				case GridElementType.Mountain:
					return new Color(212, 143, 121);
				default:
					return Color.Black;
			}
		}
	}

	internal class PreviewName : TextMenuElement
	{
		public PreviewName(MenuContainer parentContainer, Vector2 offset, string text, SpriteFont font) : base(parentContainer, offset, text, font)
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (GetEventHandlers().GetPreviewType == null)
				return;
			Text = GetEventHandlers().GetPreviewType.Invoke().ToString();
			base.Draw(spriteBatch);
		}
	}
}
