using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Combiners {
	/// <summary>
	/// Public interface defining method(s) for combining multiple input KeyedPlatesetInfos to create a single 
	/// new output PlatesetInfo, potentially of a different type.
	/// </summary>
	/// <typeparam name="TInput">The Type of data contained in all the input KeyedPlatesetInfos</typeparam>
	/// <typeparam name="TOutput">The Type of data contained in the output PlatesetInfo</typeparam>
    public interface ICombiner<TInput, TOutput> {
		/// <summary>
		/// Method that combines data in all the input KeyedPlatesetInfos and fills a new 
		/// PlatesetInfo with the combined data.
		/// </summary>
		/// <param name="inputs">A filled KeyedPlatesetInfos of data to be combined</param>
		/// <returns>A filled PlatesetInfo of combined data</returns>
		PlatesetInfo<TOutput> Combine(KeyedPlatesetInfos<TInput> inputs);
    } //end ICombiner interface
}
