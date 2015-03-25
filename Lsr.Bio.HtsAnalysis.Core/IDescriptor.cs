namespace Lsr.Bio.HtsAnalysis.Core {
	/// <summary>
	/// Public interface defining method(s) for generating a descriptor of a scoring element like a Combiner or
	/// a Transformer.
	/// </summary>
	public interface IDescriptor {
		/// <summary>
		/// Method that returns a string describing the scoring element.
		/// </summary>
		/// <returns>A string describing the scoring element, such as "ZScorePerExperiment"</returns>
		string GenerateDescriptor();
	}
}
