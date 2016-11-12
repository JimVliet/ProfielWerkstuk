using Microsoft.Xna.Framework;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class Button
	{
		private float _widthPercent;
		private float _heightPercent;
		private Vector2 _position;

		public Button(Vector2 pos, float widthPercentage, float heightPercentage)
		{
			_position = pos;
			_heightPercent = heightPercentage;
			_widthPercent = widthPercentage;
		}
	}
}
