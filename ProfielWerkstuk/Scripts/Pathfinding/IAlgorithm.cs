namespace ProfielWerkstuk.Scripts.Pathfinding
{
	public interface IAlgorithm
	{
		AlgorithmType Type { get; set; }
		void CalculatePath();
		void Callback(AlgorithmManager manager);
	}
}
