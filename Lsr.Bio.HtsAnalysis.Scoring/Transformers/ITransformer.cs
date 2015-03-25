using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Scoring.Transformers {
	/// <summary>
	/// Public interface defining method(s) for transforming an input KeyedPlatesetInfos to an output 
	/// KeyedPlatesetInfos, potentially of a different type.
	/// </summary>
	/// <typeparam name="TInput">The Type of data contained in the input KeyedPlatesetInfos</typeparam>
	/// <typeparam name="TOutput">The Type of data contained in the output KeyedPlatesetInfos</typeparam>
    public interface ITransformer<TInput, TOutput> {
		/// <summary>
		/// Method that transforms data in the input KeyedPlatesetInfos and fills a new 
		/// KeyedPlatesetInfos with the transformed data.
		/// </summary>
		/// <param name="original">A filled KeyedPlatesetInfos of data to be transformed</param>
		/// <returns>A filled KeyedPlatesetInfos of transformed data</returns>
		KeyedPlatesetInfos<TOutput> Transform(KeyedPlatesetInfos<TInput> original);
    }
}
