using System.Collections.Generic;
using System.Linq;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Scoring {
	/// <summary>
	///  Class containing extension methods for scoring elements, such as Combiners and Transformers.
	/// </summary>
    public static class ScoringExtensions {
		/// <summary>
		/// Extension method that creates and fills a Provenance object based on the properties of the 
		/// scoring element on which it is called and the input TypedPlatesetDictionary.
		/// </summary>
		/// <typeparam name="TScorer">The Type of scoring element for which a Provenance is to be generated
		/// </typeparam>
		/// <param name="scorer">A scoring element, such as a Combiner or a Transformer, that 
		/// implements the IDescriptor interface</param>
		/// <param name="inputs">A filled TypedPlatesetDictionary of the inputs to the scoring element</param>
		/// <returns>A filled Provenance object</returns>
        public static Provenance GenerateProvenance<TScorer>(this TScorer scorer, KeyedIPlatesetInfos inputs) 
			where TScorer : IDescriptor {

            Provenance result = new Provenance();
            result.InputNames = inputs.Keys.ToList();
        	result.Action = scorer.GenerateDescriptor();
            return result;            
        } //end GenerateProvenance

		/// <summary>
		/// Extension method that returns the name of the type of the input object.
		/// </summary>
		/// <typeparam name="T">The Type of object for which to get the Type name</typeparam>
		/// <param name="input">An object for which to get the Type name</param>
		/// <returns>The name of the Type of the input object</returns>
		public static string GetTypeName<T> (this T input) {
			return input.GetType().Name;
		}

		/// <summary>
		/// Extension method that generates a new PlatesetInfo name based on the properties of the scoring
		/// element on which it is called and the input list of input PlatesetInfo names.
		/// </summary>
		/// <typeparam name="TScorer">The Type of scoring element for which a new PlatesetInfo name is to be 
		/// generated </typeparam>
		/// <param name="scorer">A scoring element, such as a Combiner or a Transformer, that 
		/// implements the IDescriptor interface</param>
		/// <param name="inputPlatesetInfoNames">A list of strings containing the names of the PlatesetInfos that
		/// were used as input to the scorer</param>
		/// <returns>A string containing a new PlatesetInfo name</returns>
		public static string GeneratePlatesetInfoName<TScorer> (this TScorer scorer, 
			List<string> inputPlatesetInfoNames) where TScorer : IDescriptor {

			List<string> namePieces = new List<string> {scorer.GenerateDescriptor(), "of"};
			namePieces.AddRange(inputPlatesetInfoNames);
			string result = string.Join("_", namePieces);
			return result;
		} //end GeneratePlatesetInfoName
    }
}
