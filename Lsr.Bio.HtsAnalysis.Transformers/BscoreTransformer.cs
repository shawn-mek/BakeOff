using System.Collections.Generic;
using Lsr.Bio.HtsAnalysis.Core;
using Lsr.Bio.HtsAnalysis.RAnalysis;

namespace Lsr.Bio.HtsAnalysis.Transformers {
	/// <summary>
	/// ITransformer that calculates b scores for input values.
	/// </summary>
    class BscoreTransformer: ITransformer<double, double> {
		#region members
		/// <summary>
		/// A filled ArrayedScreenData object containing the data to be transformed
		/// </summary>
        private readonly ArrayedScreenData _DATA;

		/// <summary>
		/// A list of strings specifying the Kind value(s) of Reagents that should be treated as negative controls
		/// </summary>
        private readonly IList<string> _NEG_CONTROL_REAGENT_KINDS;

		/// <summary>
		/// A list of strings specifying the Kind value(s) of Reagents that should be treated as positive controls
		/// </summary>
        private readonly IList<string> _POS_CONTROL_REAGENT_KINDS;
		#endregion members

		#region constructors
		/// <summary>
		/// Constructor that collects data to transform as well as positive and negative control kinds.
		/// </summary>
		/// <param name="data">A filled ArrayedScreenData object containing the data to be transformed
		/// </param>
		/// <param name="negControlKinds">A list of strings specifying the Kind value(s) of Reagents that should be
		/// treated as negative controls</param>
		/// <param name="posControlKinds">A list of strings specifying the Kind value(s) of Reagents that should be 
		/// treated as positive controls</param>
		public BscoreTransformer(ArrayedScreenData data, IList<string> negControlKinds,
            IList<string> posControlKinds) {

            this._DATA = data;
            this._NEG_CONTROL_REAGENT_KINDS = negControlKinds;
            this._POS_CONTROL_REAGENT_KINDS = posControlKinds;
        }
		#endregion constructors

		#region methods
		#region ITransformer implementation
		/// <summary>
		/// Method that calculates b scores from the data in the input TypedPlatesetDictionary and fills a new 
		/// TypedPlatesetDictionary with those b scores.  Calls out to RNAither package of bioconductor, in R,
		/// to do b score calculations.
		/// </summary>
		/// <param name="original">A filled TypedPlatesetDictionary of doubles to be transformed</param>
		/// <returns>A filled TypedPlatesetDictionary of b scores stored as doubles</returns>
		public KeyedPlatesetInfos<double> Transform(KeyedPlatesetInfos<double> original) {
            RnaitherDatasetGenerator datasetGenerator = new RnaitherDatasetGenerator(this._DATA,
                this._NEG_CONTROL_REAGENT_KINDS, this._POS_CONTROL_REAGENT_KINDS);
            BscoreCalculator<double> bScorer = new BscoreCalculator<double>(datasetGenerator, original, true);
			KeyedPlatesetInfos<double> result = bScorer.GetBscores();
            return result;
		}
		#endregion ITransformer implementation
		#endregion methods
	}
}
