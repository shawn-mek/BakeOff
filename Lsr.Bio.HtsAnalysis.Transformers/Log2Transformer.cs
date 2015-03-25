using System;

namespace Lsr.Bio.HtsAnalysis.Transformers {
	/// <summary>
	/// Transformer that calculates the base-2 log of input values.
	/// </summary>
    class Log2Tranformer: Transformer<double, double> {
		/// <summary>
		/// Override of abstract Transform method that calculates the log base 2 of the input value
		/// </summary>
		/// <param name="wellSignal">A double to be transformed</param>
		/// <returns>The log base 2 of the input as a double</returns>
        protected override double Transform(double wellSignal) {
            double result = Math.Log(wellSignal, 2);
            return result;
        }
    } //end class
}
