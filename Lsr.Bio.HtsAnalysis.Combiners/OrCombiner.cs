using System.Collections.Generic;
using System.Linq;

namespace Lsr.Bio.HtsAnalysis.Combiners {
	/// <summary>
	/// Combiner that calculates the logical OR of boolean input values
	/// </summary>
    public class OrCombiner: Combiner<bool, bool> {
		#region constructors
		/// <summary>
		/// Default constructor that sets the value of the OutputDescriptor property
		/// </summary>
        public OrCombiner() {
            this.OutputDescriptor = "Or";
        } //end constructor
		#endregion constructors

		#region methods
		/// <summary>
		/// Override of abstract Combine method that calculates the logical OR of the boolean input values
		/// </summary>
		/// <param name="wellValues">A list of input boolean values, as from a single well across multiple 
		/// replicates</param>
		/// <returns>The logical OR of the input boolean values</returns>
        protected override bool Combine(IEnumerable<bool> wellValues) {
			// ReSharper disable RedundantBoolCompare
			// Specifying the 'true' here is redundant, but is also easier to comprehend
            return wellValues.Any(p => p == true);
			// ReSharper restore RedundantBoolCompare
        } //end Combine
		#endregion methods
	} //end OrCombiner
}
