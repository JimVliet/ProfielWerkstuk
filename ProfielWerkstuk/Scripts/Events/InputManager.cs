using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Events
{
	public class InputManager
	{
		public GridElement DragElement;
		private MouseState _mouseState;
		private KeyboardState _keyboardState;
		private MouseState _oldMouseState;
		private KeyboardState _oldKeyboardState;
		private readonly ProfielWerkstuk _game;
		private Vector2 _lastLeftClick;
		private bool _outsideMenuClick;

		public InputManager(ProfielWerkstuk game)
		{
			_game = game;
		}

		public void Update()
		{
			_keyboardState = Keyboard.GetState();
			_mouseState = Mouse.GetState();
			Vector2 clickLocation = GetMouseLocation();

			_game.UserInterface.CheckMouseHover(clickLocation);
			CheckClickEvent(clickLocation);
			CheckRightClickEvent(clickLocation);
			CheckScroll();
			EscapePushed();

			_oldMouseState = _mouseState;
			_oldKeyboardState = _keyboardState;
		}

		public Vector2 GetMouseLocation()
		{
			return new Vector2(_mouseState.X, _mouseState.Y);
		}

		private void CheckRightClickEvent(Vector2 clickLocation)
		{
			if (_mouseState.RightButton == ButtonState.Pressed)
			{
				if (_oldMouseState.RightButton == ButtonState.Pressed)
					RightMouseHold(clickLocation);
				else
					RightMouseClick();
			}
			else if (_oldMouseState.RightButton == ButtonState.Pressed)
				RightMouseRelease();
		}

		private void RightMouseRelease()
		{
			_game.Grid.GridHoldType = GridElementType.Null;
		}

		private void RightMouseHold(Vector2 clickLocation)
		{
			if(_game.UserInterface.GetMenu(clickLocation) == null && _game.UserInterface.AllowClicking())
				_game.Grid.GridHoldClick(clickLocation);
		}

		private void RightMouseClick()
		{
			
		}

		private void EscapePushed()
		{
			if (!(_keyboardState.IsKeyDown(Keys.Escape) && !_oldKeyboardState.IsKeyDown(Keys.Escape)))
				return;
			_game.UserInterface.SwitchState();
		}

		private void CheckClickEvent(Vector2 clickLocation)
		{
			if(_oldMouseState.LeftButton == ButtonState.Released &&
				_mouseState.LeftButton == ButtonState.Pressed)
				LeftClickEvent(clickLocation);
			if (_oldMouseState.LeftButton == ButtonState.Pressed)
			{
				if(_mouseState.LeftButton == ButtonState.Released)
					LeftReleaseEvent(clickLocation);
				else if (_game.UserInterface.GetMenu(clickLocation) == null)
				{
					LeftDragEvent();
				}
			}
		}

		private void CheckScroll()
		{
			int deltaScroll = _mouseState.ScrollWheelValue - _oldMouseState.ScrollWheelValue;
			if (deltaScroll != 0)
				ScrollEvent(deltaScroll);
		}

		private void ScrollEvent(int deltaScroll)
		{
			if(_game.UserInterface.AllowClicking())
				_game.CameraManager.CheckScroll(deltaScroll);
		}

		private void LeftClickEvent(Vector2 clickLocation)
		{
			_outsideMenuClick = _game.UserInterface.ClickEvent(clickLocation);

			if (_outsideMenuClick && _game.UserInterface.AllowClicking())
			{
				GridElement element = _game.Grid.GetGridElement(clickLocation);
				if (element != null && (element.Type == GridElementType.Start || element.Type == GridElementType.End))
				{
					DragElement = element;
				}
			}

			//Store the new lastLeftClick location
			_lastLeftClick = clickLocation;
		}

		private void LeftReleaseEvent(Vector2 clickLocation)
		{
			if (!_outsideMenuClick)
			{
				_game.UserInterface.LeftReleaseEvent(clickLocation);
				return;
			}

			if(!_game.UserInterface.AllowClicking())
				return;

			if (DragElement != null)
			{
				GridElement element = _game.Grid.GetGridElement(clickLocation);
				if (element != null && element.Type != GridElementType.End && element.Type != GridElementType.Start)
				{
					if(DragElement.Type == GridElementType.Start)
						_game.Grid.ChangeStartElement(element);
					else
						_game.Grid.ChangeEndElement(element);
				}

				DragElement = null;
			}
			else
			{
				Vector2 deltaClick = clickLocation - _lastLeftClick;

				if(deltaClick.Length() > 2)
					return;

				_game.Grid.GridClicked(clickLocation);
			}
		}

		private void LeftDragEvent()
		{
			if (DragElement == null && _game.UserInterface.AllowClicking() && _outsideMenuClick)
				_game.CameraManager.CheckPanning(_mouseState, _oldMouseState);
		}
	}
}
