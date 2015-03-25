using System.Collections.Generic;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Transformers {
	/// <summary>
	/// Abstract class that implements shared functions of Transformers.
	/// </summary>
	/// <typeparam name="TInput">The Type of input data to be transformed</typeparam>
	/// <typeparam name="TOutput">The Type of the output data to be returned after transformation</typeparam>
    public abstract class Transformer<TInput, TOutput>: ITransformer<TInput, TOutput>, IDescriptor {
		#region methods
		/// <summary>
		/// Abstract method that must be provided by all child classes to transform the input value, as from a 
		/// single well, to its output value.
		/// </summary>
		/// <param name="wellSignal">A single input value, as from a single well</param>
		/// <returns>A single tranformed output value</returns>
		protected abstract TOutput Transform(TInput wellSignal);

		protected virtual Well<TOutput> Transform(Well<TInput> well) {
			TOutput transformedValue = this.Transform(well.Value);
			return new Well<TOutput>(transformedValue);
		}

		#region IDescriptor implementation
		/// <summary>
		/// Virtual method that allows child classes to specify a string describing what kind of transform
		/// they perform; this string is used in generating the names of output platesets.  Default implementation 
		/// uses the name of the child class as the descriptor.
		/// </summary>
		/// <returns>A string describing the kind of transform performed by the Transformer, such as 
		/// "ZScorePerExperiment"</returns>
		public virtual string GenerateDescriptor() {
			return this.GetTypeName();
		} //end GenerateTransformerDescriptor
		#endregion IDescriptor implementation

		/// <summary>
		/// Virtual method that allows child classes to perform some set-up for each experiment before
		/// any transforms for that experiment are performed.  Default implementation does nothing.
		/// </summary>
		/// <param name="plateRowColSignals">A filled TInput array of data for the experiment</param>
        protected virtual void SetUpTransformForExpt(Plateset<TInput> plateset) { }

		/// <summary>
		/// Virtual method that allows child classes to perform some set-up for each plate before
		/// any transforms for that plate are performed.  Default implementation does nothing.
		/// </summary>
		/// <param name="rowColSignals">A filled TInput array of data for the plate</param>
        protected virtual void SetUpTransformForPlate(Plate<TInput> plate) { }

		#region ITransformer implementation
		/// <summary>
		/// Public method that loops over all values in the input TypedPlatesetDictionary and transforms each
		/// one individually, creating a new TypedPlatesetDictionary containing the transformed data and an
		/// automatically generated Provenance.
		/// </summary>
		/// <param name="original">A filled TypedPlatesetDictionary of data to be transformed</param>
		/// <returns>A filled TypedPlatesetDictionary of transformed data</returns>
		public KeyedPlatesetInfos<TOutput> Transform(KeyedPlatesetInfos<TInput> original) {
			Provenance provenance = this.GenerateProvenance(original);
			KeyedPlatesetInfos<TOutput> result = new KeyedPlatesetInfos<TOutput>();

			foreach (string currKey in original.Keys) { //foreach plateset
				Plateset<TInput> currPlateset = original[currKey].Plateset;
				string newSetName = this.GeneratePlatesetInfoName(currKey);
				PlatesetInfo<TOutput> currResult = new PlatesetInfo<TOutput>(newSetName, provenance, 
					original.NumPlates, original.NumRows, original.NumColumns);

				this.SetUpTransformForExpt(currPlateset);
				for (int plateIndex = 0; plateIndex < currPlateset.Length; plateIndex++) {
					Plate<TInput> currPlate = currPlateset[plateIndex];
					this.SetUpTransformForPlate(currPlate);

					for (int rowIndex = 0; rowIndex < currPlate.NumRows; rowIndex++) {
						Row<TInput> currRow = currPlate.GetRow(rowIndex);
						for (int colIndex = 0; colIndex < currRow.Length; colIndex++) {
							Well<TInput> currWell = currRow[colIndex];
							currResult[plateIndex, rowIndex, colIndex] = this.Transform(currWell);
						} //next colIndex
					} //next rowIndex
				} //next plateIndex

				result[newSetName] = currResult;
			} //next plateset key

			return result;
		}  //end Transform
		#endregion ITransformer implementation

		/// <summary>
		/// Method that automatically generates names for a transformed output plateset based on the name of
		/// the input plateset of which it is the transform.
		/// </summary>
		/// <param name="inputPlatesetInfoName">A string containing the name of the input plateset that 
		/// was transformed</param>
		/// <returns>A string containing a name for the new transformed output plateset</returns>
		protected string GeneratePlatesetInfoName(string inputPlatesetInfoName) {
			return this.GeneratePlatesetInfoName(new List<string> {inputPlatesetInfoName});
        } //end GeneratePlatesetInfoName
		#endregion methods
	} //end class
}
