using System.Collections.Generic;
using System.Linq;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Scoring.Combiners {
	/// <summary>
	/// Abstract class that implements shared functions of Combiners.
	/// </summary>
	/// <typeparam name="TInput">The Type of input data to be combined</typeparam>
	/// <typeparam name="TOutput">The Type of the output data to be returned after combination</typeparam>
    public abstract class Combiner<TInput, TOutput>: ICombiner<TInput, TOutput>, IDescriptor {
		#region members
		/// <summary>
		/// String containing descriptor of the method by which the combination output was created, such as "And",
		/// "Or", or "Median"
		/// </summary>
		protected string OutputDescriptor;
		#endregion members

		#region methods
		/// <summary>
		/// Abstract method that must be provided by all child classes to combine a list of input values, as from a 
		/// single well across multiple replicates, into a single output value.
		/// </summary>
		/// <param name="wellValues">A list of input values, as from a single well across multiple replicates
		/// </param>
		/// <returns>A single combined output value</returns>
		protected abstract TOutput Combine(IEnumerable<TInput> wellValues);

		protected virtual Well<TOutput> Combine(IEnumerable<Well<TInput>> wells) {
			IEnumerable<TInput> wellValues = wells.Select(x => x.Value);
			TOutput combinedValue = this.Combine(wellValues);
			return  new Well<TOutput>(combinedValue);
		} 

		#region IDescriptor implementation
		/// <summary>
		/// Virtual method that allows child classes to specify a string describing what kind of combine
		/// they perform; this string is used in generating the names of output platesets.  Default implementation 
		/// uses the OutputDescriptor property as the descriptor.
		/// </summary>
		/// <returns>A string describing the kind of combine performed by the Combiner, such as "Median"
		/// </returns>
		public virtual string GenerateDescriptor() {
			return this.OutputDescriptor;
		}
		#endregion IDescriptor implementation

		#region ICombiner implementation
		/// <summary>
		/// Public method that loops over all values in the input TypedPlatesetDictionary, aggregates all data from
		/// the same well, combines that data, and creates a new PlatesetInfo containing the combined data and an
		/// automatically generated Provenance.
		/// </summary>
		/// <param name="inputs">A filled TypedPlatesetDictionary of data to be combined</param>
		/// <returns>A filled PlatesetInfo of combined data</returns>
		/// <remarks>Well values that are set to be ignored are not included in the combination</remarks>
		public PlatesetInfo<TOutput> Combine(KeyedPlatesetInfos<TInput> inputs) {
			Provenance provenance = this.GenerateProvenance(inputs);
			string newPlatesetInfoName = this.GeneratePlatesetInfoName(inputs);
			PlatesetInfo<TOutput> result = new PlatesetInfo<TOutput>(newPlatesetInfoName, provenance,
				inputs.NumPlates, inputs.NumRows, inputs.NumColumns);

			for (int plateIndex = 0; plateIndex < inputs.NumPlates; plateIndex++) {
				for (int rowIndex = 0; rowIndex < inputs.NumRows; rowIndex++) {
					for (int colIndex = 0; colIndex < inputs.NumColumns; colIndex++) {
						IList<Well<TInput>> wells = new List<Well<TInput>>();

						foreach (string platesetKey in inputs.Keys) {
							PlatesetInfo<TInput> currPlatesetInfo = inputs[platesetKey];
							bool ignoreValue = currPlatesetInfo.GetIgnoreValue(plateIndex, rowIndex, colIndex);

							if (!ignoreValue) {
								Well<TInput> currWell = currPlatesetInfo[plateIndex, rowIndex, colIndex];
								wells.Add(currWell);
							} //end if not supposed to ignore this well
						} //next plateset

						Well<TOutput> combinedWell = this.Combine(wells);
						result[plateIndex, rowIndex, colIndex] = combinedWell;
					} //next column
				} //next row
			} //next plate

			return result;
		} //end Combine
		#endregion ICombiner implementation


		/// <summary>
		/// Method that automatically generates names for a combined output plateset based on the names of
		/// the input platesets of which it is the combination.
		/// </summary>
		/// <param name="inputs">A TypedPlatesetDictionary containing the platesets that were input to the 
		/// combination</param>
		/// <returns>A string containing a name for the new combined output plateset</returns>
		protected string GeneratePlatesetInfoName(KeyedIPlatesetInfos inputs) {
        	return this.GeneratePlatesetInfoName(inputs.Keys.ToList());
        } //end GeneratePlatesetInfoName
		#endregion methods
	} //end class Combiner
}
