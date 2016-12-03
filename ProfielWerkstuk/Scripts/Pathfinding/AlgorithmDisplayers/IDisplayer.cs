using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public interface IDisplayer
	{
		void UpdateDisplayer(List<Vector2> pathDrawingPoints, List<ResultInfo> resultInfo);
		void Draw(SpriteBatch spriteBatch, GameTime gameTime);
	}
}
