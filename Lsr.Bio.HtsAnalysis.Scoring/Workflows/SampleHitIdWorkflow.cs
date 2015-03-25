using Lsr.Bio.HtsAnalysis.Core;
using Lsr.Bio.HtsAnalysis.Scoring.Combiners;
using Lsr.Bio.HtsAnalysis.Scoring.Transformers;

namespace Lsr.Bio.HtsAnalysis.Scoring.Workflows {
	/// <summary>
	/// Sample workflow similar to that used to analyze the TMO proteasome screen: b scores are calculated for 
	/// the screen data, z scores of those b scores are calculated, and hits are identified as samples having
	/// z scores of b scores greater than 2 or less than -2.
	/// </summary>
	public class SampleHitIdWorkflow : IWorkflow {
		/// <summary>
		/// Method that executes the workflow to identify both up- and down-regulating hits by z score of b
		/// score and write out the results.
		/// </summary>
		/// <param name="data">A filled ArrayedScreenData object containing the screen data to analyze</param>
		public void Execute(ArrayedScreenData data) {
			//gather the signals together in a typed format
			KeyedPlatesetInfos<double> rawSignals = this.GetRawSignals(data);

			////do a b-score transform on each replicate
			//List<string> negControlKinds = new List<string> { "neg control" };
			//List<string> posControlKinds = new List<string> { "pos control" };
			//BscoreTransformer bScorer = new BscoreTransformer(data, negControlKinds, posControlKinds);
			//TypedPlatesetDictionary<double> bScores = bScorer.Transform(rawSignals);

			//take median of b-score across replicates
			PlatesetInfo<double> medianBscores = new MedianCombiner().Combine(rawSignals);

			//calc z-scores of median b-scores
			PlatesetInfo<double> zScoresOfBscores = new ZScorePerPlate().Transform(medianBscores);

			//identify up-regulated hits based on z score of median b-score
			HitIdTransformer upZhitsTranformer = new HitIdTransformer("up hits", x => x >= 2);
			PlatesetInfo<bool> upZhits = upZhitsTranformer.Transform(zScoresOfBscores);

			//identify down-regulated hits based on z score of median b-score
			HitIdTransformer downZhitsTranformer = new HitIdTransformer("down hits", x => x <= -2);
			PlatesetInfo<bool> downZhits = downZhitsTranformer.Transform(zScoresOfBscores);

			//combine up and down hits to get final hit list
			KeyedPlatesetInfos<bool> hitSets = new KeyedPlatesetInfos<bool>(upZhits, downZhits);
			PlatesetInfo<bool> hits = new OrCombiner().Combine(hitSets);

			//write out
			//TODO: fill in output
		}

		/// <summary>
		/// Method that gets the type-agnostic signals in the input ArrayedScreenData and puts them into a 
		/// TypedPlatesetDictionary containing doubles.
		/// </summary>
		/// <param name="data">A filled ArrayedScreenData object</param>
		/// <returns>A filled TypedPlatesetDictionary containing the signals in the ArrayedScreenData as
		/// PlatesetInfos of doubles</returns>
		/// <remarks>This method almost certainly doesn't belong HERE--perhaps in ArrayedScreenData?  Also,
		/// it should include some sort of checking to make sure it really is getting only the raw signals.
		/// </remarks>
		protected KeyedPlatesetInfos<double> GetRawSignals(ArrayedScreenData data) {
			KeyedPlatesetInfos<double> result = new KeyedPlatesetInfos<double>();
			foreach (IPlatesetInfo currIPlatesetInfo in data.Signals.Values) {
				PlatesetInfo<double> currPlatesetInfo = currIPlatesetInfo.CastToPlatesetInfo<double>();
				result.Add(currPlatesetInfo);
			}
			return result;
		} 
	}
}
