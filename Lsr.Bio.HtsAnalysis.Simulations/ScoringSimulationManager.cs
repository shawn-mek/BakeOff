using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Lsr.Bio.HtsAnalysis.Core;
using Lsr.Bio.HtsAnalysis.Scoring;
using NoiseMaker;

namespace Lsr.Bio.HtsAnalysis.Simulations {
	/// <summary>
	/// Class that takes in inputs defining the characteristics of the desired scoring approaches, runs those on the provided (simulated) screening data
	/// automatically, analyzes performance of each scoring approach, and writes out the associated results files.
	/// </summary>
	class ScoringSimulationManager {
		#region members
		/// <summary>
        /// Object holding input arguments needed to set up simulation(s).
        /// </summary>
        private ScoringSimulationInputArguments _InputArguments;

		/// <summary>
		/// List of filled ScoringSetInfo objects holding the workflows to apply to the simulated data
		/// </summary>
		private ScoringSetInfo _ScoringSetInfo;

		private int _ScreenId;
		private int _HitSetId;
		private int _NoiseSetId;
		private IList<HitIdPerformance> _PerformanceMetrics;
		#endregion

		#region public methods
		/// <summary>
        /// Method that takes in user arguments defining the simulations to run, runs those 
        /// simulations automatically, and writes out the associated results files.
        /// </summary>
        /// <param name="args">A string array containing at least 5 items: working directory, plate map file name,
        /// screen infos file name, hit set infos file name, and noise set infos file name (in that order).</param>
        public void Score(string[] args) {
            this._ResetState();

            this._InputArguments = new ScoringSimulationInputArguments(args);
            IList<string> validationMsgs = this._InputArguments.Validate();
            if (validationMsgs.Count > 0) {
                throw new Exception(string.Format("Validation issues detected:{0}",
                    string.Join(Environment.NewLine, validationMsgs)));
            } //end if there were validation problems

			this._LoadScoringSetInfos(this._InputArguments);

			this.Score(this._InputArguments);
        } //end 

		public void Score(ScoringSimulationInputArguments inputArguments) {
			this._PerformanceMetrics = new List<HitIdPerformance>();
			HitIdPerformanceAnalyzer analyzer = new HitIdPerformanceAnalyzer();

			//for each file of noisy data
			foreach (string currNoisyDataFilePath in inputArguments.NoisyScreenDataFilePaths) {
				this._ParseSimulationIds(currNoisyDataFilePath);
				ArrayedScreenData currNoisyData = this._LoadSimulationData(currNoisyDataFilePath);

				foreach (ScoringWorkflowInfo currWorkflowInfo in this._ScoringSetInfo.ScoringWorkflowInfos) {
					BakeoffWorkflow currWorkflow = new BakeoffWorkflow(currWorkflowInfo);
					IList<string> noisyReplicateNames = this._GetNoisyReplicateNames(currNoisyData);
					currWorkflow.Execute(currNoisyData, noisyReplicateNames);
					//TODO: output workflow results

					HitIdPerformance performanceMetric = analyzer.Analyze(currNoisyData);
					this._PerformanceMetrics.Add(performanceMetric);
				} //next scoring workflow
			}  //next noisy data set

			string outputMetrics = this._WriteOutPerformanceMetrics();

			string outputFileName = this._InputArguments.ScoringSetInfosFileName.Replace(".txt",
				string.Format("_scoringset{0}.txt", this._ScoringSetInfo.ScoringSetId));
			string outputFilePath = Path.Combine(this._InputArguments.WorkingDirectory, outputFileName);
			File.WriteAllText(outputFilePath, outputMetrics);
		}
		#endregion

		#region private methods
		/// <summary>
        /// Method that sets all member variables to null to clear state before running a new set of scorings.
        /// </summary>
        private void _ResetState() {
            this._InputArguments = null;
			this._ScoringSetInfo = null;
			this._ScreenId = 0;
			this._HitSetId = 0;
        	this._NoiseSetId = 0;
        	this._PerformanceMetrics = null;
        } //end _ResetState

		/// <summary>
		/// Method that reads in the screen infos, hit set infos, and noise set infos files from the working directory
		/// and stores their contents into member variables.
		/// </summary>
		/// <param name="inputs">A filled InputArguments object holding the user-specified input file names and working 
		/// directory for the simulation(s).</param>
		private void _LoadScoringSetInfos(ScoringSimulationInputArguments inputArguments) {
			StreamReader fileReader = File.OpenText(inputArguments.ScoringSetInfosFilePath);
			IScoringSetTextParser scoringSetParser = new ScoringSetTextParser();
			List<ScoringSetInfo> infosList = scoringSetParser.LoadScoringSetInfo(fileReader);
			if (infosList.Count != 1) {
				throw new Exception(string.Format(
					"Code can only accommodate one scoring set, but input file {0} contains {1} scoring sets",
					inputArguments.ScoringSetInfosFilePath, infosList.Count));
			}
			this._ScoringSetInfo = infosList[0];
		} //end _LoadScoringSetInfos

		private ArrayedScreenData _LoadSimulationData(string simulationDataFilePath) {
			DataSimulationLoader simulationLoader = new DataSimulationLoader();
			ArrayedScreenData result = simulationLoader.LoadFromReader<double>(inReader, inputSource, numPlates,
			                                                                           numRows, numCols);
			return result;
		}

		private IList<string> _GetNoisyReplicateNames(ArrayedScreenData currNoisyScreenData) {
			IList<string> result = new List<string>();
			foreach (string currKey in currNoisyScreenData.Signals.Keys) {
				if (currKey.StartsWith(ScreenNoiseGenerator.NoisyReplicatePrefix)) {
					result.Add(currKey);
				} //end if this signal represents a noisy replicate
			} //next signal key

			return result;
		} 

		//uses capture of named groups from regex
		private void _ParseSimulationIds(string simulatedDataFilePath) {
			Regex nameRegEx = new Regex("screen(<screenid>[0-9]+)_hitset(<hitsetid>[0-9]+)_noiseset(<noisesetid>[0-9]+)");
			MatchCollection matchCollection = nameRegEx.Matches(simulatedDataFilePath);
			if (matchCollection.Count != 1) {
				throw new ArgumentException(string.Format("Input simulation data file path {0} does not match the expected format 'screen<id>_hitset<id>_noiseset<id>'",
					simulatedDataFilePath));
			} //end if

			this._ScreenId = Convert.ToInt32(matchCollection[0].Groups["screenid"]);
			this._HitSetId = Convert.ToInt32(matchCollection[0].Groups["hitsetid"]);
			this._NoiseSetId = Convert.ToInt32(matchCollection[0].Groups["noisesetid"]);
		}

		//screen	hitset	noiseset	workflow	<workflow params>	valuetype	FPR	FNR	TPR	rating
		private string _WriteOutPerformanceMetrics() {
			StringBuilder stringBuilder = new StringBuilder();
			foreach (HitIdPerformance currPerformanceMetrics in this._PerformanceMetrics) {
				stringBuilder.AppendLine(currPerformanceMetrics.ToString());
			}
			return stringBuilder.ToString();
		}
		#endregion

		internal class HitIdPerformanceAnalyzer {
			public HitIdPerformance Analyze(ArrayedScreenData hitCalledData) {
				//TODO: implement
			}
		}

		internal class HitIdPerformance {
			public int ScreenId { get; set; }
			public int HitSetId { get; set; }
			public int NoiseSetId { get; set; }
			public ScoringWorkflowInfo ScoringWorkflow { get; set; }
			public double FalsePositiveRate { get; set; }
			public double TruePositiveRate { get; set; }
			public double Rating { get; set; }

// IF(AND(TPR=1,FPR<0.1),4,
//IF(AND(TPR>0.95,FPR<0.1),3,
//IF(AND(TPR>0.75,FPR<0.1),2,
//IF(AND(TPR>0.5,FPR<0.1),1,
//IF(OR(TPR<0.1,FPR>0.2),-1,0)

			public override string ToString() {
				string result = string.Join(SimulationUtilities.DELIMITER, ScreenId, HitSetId, NoiseSetId, ScoringWorkflow.ToString(),
				    ScoringWorkflow.NormalizationMethod, ScoringWorkflow.ReplicateCombBeforeScoringMethod,
				    ScoringWorkflow.ScoringMethod, ScoringWorkflow.ReplicateCombAfterScoringMethod,
					ScoringWorkflow.HitThresholdExpression, TruePositiveRate, FalsePositiveRate,
				    Rating);

				return result;
			}
		}
	}
}
