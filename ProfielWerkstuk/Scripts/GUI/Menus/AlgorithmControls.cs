using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;
using ProfielWerkstuk.Scripts.GUI.Textures;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class AlgorithmControls : UserInterfaceMenu
	{
		private readonly TextureManager _textureManager;

		public AlgorithmControls(BaseUserInterfaceState state, TextureManager textures) : base(state, HorizontalAlignment.Center, VerticalAlignment.Bottom)
		{
			_textureManager = textures;
			SetupControls();
		}

		private void SetupControls()
		{
			MenuContainer container = new MenuContainer(this);

			PlayPauseButton playPauseButton = new PlayPauseButton(container, _textureManager);
			playPauseButton.AddToContainer();

			FastBackwardButton fastBackward = new FastBackwardButton(container, _textureManager.FastBackward)
			{
				Offset = new Vector2(-60, 0)
			};
			fastBackward.AddToContainer();

			FastForwardButton fastForward = new FastForwardButton(container, _textureManager.FastForward)
			{
				Offset = new Vector2(60, 0)
			};
			fastForward.AddToContainer();

			SkipToStartButton skipToStart = new SkipToStartButton(container, _textureManager.SkipStart)
			{
				Offset = new Vector2(-120, 0)
			};
			skipToStart.AddToContainer();

			SkipToEndButton skipToEnd = new SkipToEndButton(container, _textureManager.SkipEnd)
			{
				Offset = new Vector2(120, 0)
			};
			skipToEnd.AddToContainer();

			container.AddToMenu();
		}
	}

	internal class PlayPauseButton : ControlMenuElement
	{
		private readonly Texture2D _play;
		private readonly Texture2D _pause;

		public PlayPauseButton(MenuContainer parentContainer, TextureManager textures) : base(parentContainer, textures.Pause)
		{
			_play = textures.Play;
			_pause = textures.Pause;
			GetEventHandlers().PlayPauseEvent += PlayPauseEvent;
		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().PlayPauseButtonClicked?.Invoke();
		}

		private void PlayPauseEvent(bool isPaused)
		{
			Texture = isPaused ? _play : _pause;
		}
	}

	internal class FastForwardButton : ControlMenuElement
	{
		private bool _isBeingClicked;

		public FastForwardButton(MenuContainer parentContainer, Texture2D texture) : base(parentContainer, texture)
		{

		}

		public override void LeftReleaseEvent()
		{
			if (!_isBeingClicked)
				return;

			GetEventHandlers().FastForwardEnd?.Invoke();
			_isBeingClicked = false;
		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().FastForwardStart?.Invoke();
			_isBeingClicked = true;
		}

		public override void UnHover()
		{
			base.UnHover();
			if (_isBeingClicked)
				GetEventHandlers().FastForwardEnd?.Invoke();

			_isBeingClicked = false;
		}
	}

	internal class FastBackwardButton : ControlMenuElement
	{
		private bool _isBeingClicked;

		public FastBackwardButton(MenuContainer parentContainer, Texture2D texture) : base(parentContainer, texture)
		{

		}

		public override void LeftReleaseEvent()
		{
			if (!_isBeingClicked)
				return;

			GetEventHandlers().FastBackwardEnd?.Invoke();
			_isBeingClicked = false;
		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().FastBackwardStart?.Invoke();
			_isBeingClicked = true;
		}

		public override void UnHover()
		{
			base.UnHover();
			if(_isBeingClicked)
				GetEventHandlers().FastBackwardEnd?.Invoke();

			_isBeingClicked = false;
		}
	}

	internal class SkipToStartButton : ControlMenuElement
	{
		public SkipToStartButton(MenuContainer parentContainer, Texture2D texture) : base(parentContainer, texture)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().SkipToStartClicked?.Invoke();
		}
	}

	internal class SkipToEndButton : ControlMenuElement
	{
		public SkipToEndButton(MenuContainer parentContainer, Texture2D texture) : base(parentContainer, texture)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().SkipToEndClicked?.Invoke();
		}
	}
}
