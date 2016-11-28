using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public interface IDisplayer
	{
		List<Vector2> PathDrawingPoints { get; set; }

		void Draw(SpriteBatch spriteBatch);
	}
}
