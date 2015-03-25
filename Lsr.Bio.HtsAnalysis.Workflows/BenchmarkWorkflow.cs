using System;
using System.Collections.Generic;
using Lsr.Bio.HtsAnalysis.Combiners;
using Lsr.Bio.HtsAnalysis.Core;
using Lsr.Bio.HtsAnalysis.Transformers;

namespace Lsr.Bio.HtsAnalysis.Workflows {
	public class BenchmarkWorkflow: IWorkflow {
		private ScoringWorkflowInfo _WorkflowInfo;
		private ArrayedScreenData _ScreenData;
		public readonly string HIT_SIGNAL_KEY = "isHit";

		public BenchmarkWorkflow(ScoringWorkflowInfo workflowInfo) {
			this._WorkflowInfo = workflowInfo;
		}

		public override string ToString() {
			return this._WorkflowInfo.ToString();
		}

		public void Execute(ArrayedScreenData data, IEnumerable<string> signalNames) {
			this._ScreenData = data;
			KeyedPlatesetInfos<double> currSignals = this._SelectRelevantSignals<double>(signalNames);

			//normalize
			KeyedPlatesetInfos<double> normalizedData = this._RunReflectedTransformer<double, double>(
				this._WorkflowInfo.NormalizationMethod, currSignals);
			this._ReplaceCurrentSignals(ref currSignals, normalizedData);
			
			//combine replicates before scoring
			PlatesetInfo<double> combinedBeforeScoringData = this._RunReflectedCombiner<double, double>(
				this._WorkflowInfo.ReplicateCombBeforeScoringMethod, currSignals);
			this._ReplaceCurrentSignals(ref currSignals, combinedBeforeScoringData);

			//score
			KeyedPlatesetInfos<double> scoredData = this._RunReflectedTransformer<double, double>(
				this._WorkflowInfo.ScoringMethod, currSignals);
			this._ReplaceCurrentSignals(ref currSignals, scoredData);

			//combine replicates after scoring
			PlatesetInfo<double> combinedAfterScoringData = this._RunReflectedCombiner<double, double>(
				this._WorkflowInfo.ReplicateCombAfterScoringMethod, currSignals);
			this._ReplaceCurrentSignals(ref currSignals, combinedAfterScoringData);

			//id hits
			HitIdTransformer.CallHit hitIdDelegate = this._GenerateCallHitDelegate(this._WorkflowInfo.HitThresholdExpression);
			HitIdTransformer hitIdTranformer = new HitIdTransformer(HIT_SIGNAL_KEY, hitIdDelegate);
			KeyedPlatesetInfos<bool> hitIdsData = hitIdTranformer.Transform(currSignals);
			this._ScreenData.Signals.Add(hitIdsData);
		}

		private KeyedPlatesetInfos<T> _SelectRelevantSignals<T>(IEnumerable<string> signalNames) {
			KeyedPlatesetInfos<T> result = new KeyedPlatesetInfos<T>();
			foreach (string currKey in signalNames) {
				if (!this._ScreenData.Signals.ContainsKey(currKey)) {
					throw new ArgumentException(string.Format("No signals found for key {0}.", currKey));
				} //end if there isn't a signal set for this key
				result.Add(this._ScreenData.Signals[currKey]);
			} //next key
			return result;
		} //end _SelectRelevantSignals

		private KeyedPlatesetInfos<TOutput>  _RunReflectedTransformer<TInput, TOutput>(string iTransformerClassName, 
			KeyedPlatesetInfos<TInput> inputs) {

			KeyedPlatesetInfos<TOutput> result = null;
			
			if (!String.IsNullOrWhiteSpace(iTransformerClassName)) {
				Type iTransformerType = Type.GetType(iTransformerClassName);
				ITransformer<TInput, TOutput> transformer = (ITransformer<TInput, TOutput>) Activator.CreateInstance(iTransformerType);
				result = transformer.Transform(inputs);				
			}

			return result;
		} //end _RunReflectedTransformer

		private PlatesetInfo<TOutput> _RunReflectedCombiner<TInput, TOutput>(string iCombinerClassName, 
			KeyedPlatesetInfos<TInput> inputs) {

			PlatesetInfo<TOutput> result = null;

			if (!String.IsNullOrWhiteSpace(iCombinerClassName)) {
				Type iCombinerType = Type.GetType(iCombinerClassName);
				ICombiner<TInput, TOutput> combiner = (ICombiner<TInput, TOutput>) Activator.CreateInstance(iCombinerType);
				result = combiner.Combine(inputs);
			}

			return result;			
		} //end _RunReflectedCombiner

		private void _ReplaceCurrentSignals<T>( ref KeyedPlatesetInfos<T> currSignals, 
			KeyedPlatesetInfos<T> potentialNewSignals) {

			if (potentialNewSignals != null) {
				currSignals = potentialNewSignals;
				this._ScreenData.Signals.Add(potentialNewSignals);
			}
		} //end _ReplaceCurrentSignals

		private void _ReplaceCurrentSignals<T>(ref KeyedPlatesetInfos<T> currSignals, PlatesetInfo<T> potentialNewSignals) {
			if (potentialNewSignals != null) {
				currSignals = new KeyedPlatesetInfos<T>();
				currSignals.Add(potentialNewSignals);	
				this._ScreenData.Signals.Add(potentialNewSignals);
			}
		} //end _ReplaceCurrentSignals

		private HitIdTransformer.CallHit _GenerateCallHitDelegate(string hitIdExpression) {
			string[] pieces = hitIdExpression.Split(); //if the separator parameter is null or contains no characters, white-space characters are assumed to be the delimiters.
			if (pieces.Length != 2) {
				throw new ArgumentException(string.Format("Hit id expression expected to be of the format 'operator value', like '<= 2', but was actually {0}",
					hitIdExpression));
			}

			HitIdTransformer.CallHit result;
			string operatorString = pieces[0];
			double threshold = Convert.ToDouble(pieces[1]);
			switch (operatorString) {
				case ">":
					result = x => x > threshold; break;
				case "<":
					result = x => x < threshold; break;
				case ">=":
					result = x => x >= threshold; break;
				case "<=":
					result = x => x <= threshold; break;
				case "!=":
					result = x => x != threshold; break;
				case "==":
					result = x => x == threshold; break;
				default:
					throw new ArgumentException(string.Format("Unrecognized operator: {0}", operatorString));
			} //end switch

			return result;
		} //end _GenerateCallHitDelegate
	}
}
