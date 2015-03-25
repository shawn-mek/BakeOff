using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Lsr.Bio.Benchmark.ScreenScorer.ScoringSet;
using Lsr.Bio.Benchmark.ScreenSimulationLoader;
using Lsr.Bio.Benchmark.ScreenSimulator.HitSet;
using Lsr.Bio.Benchmark.ScreenSimulator.Screen;
using Lsr.Bio.HtsAnalysis.Core;
using Lsr.Bio.HtsAnalysis.Workflows;
using NoiseMaker;

namespace Lsr.Bio.Benchmark.ScreenScorer {
	/// <summary>
	/// Class that takes in inputs defining the screen dimensions, scoring to use, and the noisy data to score; performs those 
	/// scorings automatically; and writes out the associated results files.
	/// </summary>
	public class ScoringManager {
		#region members
		/// <summary>
		/// ScoringInputs object to hold input arguments needed to set up simulation(s).
		/// </summary>
		private ScoringInputs _InputArguments;

		/// <summary>
		/// List of filled ScreenInfo objects holding the definitions of the screen(s) to simulate.
		/// </summary>
		private List<ScreenInfo> _ScreenInfos;

		/// <summary>
		/// List of filled HitSetInfo objects holding the definitions of the hit set(s) to be generated within each
		/// screen.
		/// </summary>
		private List<HitSetInfo> _HitSetInfos;

		/// <summary>
		/// ScoringSetInfo object holding info on the workflows to apply to the noisy data.
		/// </summary>
		private ScoringSetInfo _ScoringSetInfo;

		private int _ScreenId;
		private int _HitSetId;
		private int _NoiseSetId;

		private IList<HitIdPerformance> _PerformanceMetrics;
		#endregion

		#region public methods
		/// <summary>
		/// Method that takes in user arguments the screen dimensions, scoring to use, and the noisy data to score; performs those 
		/// scorings automatically; and writes out the associated results files.
		/// </summary>
		/// <param name="args">A string array containing at least 4 items: working directory, screen infos file name, hit infos file name, 
		/// scoring set info file name, and noisy data file name(s) (in that order).
		/// </param>
		public void _Score(string[] args) {
			this._ResetState();

			this._InputArguments = new ScoringInputs(args);
			IList<string> validationMsgs = this._InputArguments.Validate();

			if (validationMsgs.Count > 0) {
				throw new Exception(string.Format("Validation issues detected:{0}",
					string.Join(Environment.NewLine, validationMsgs)));
			} //end if there were validation problems

			
			this._LoadInfos(this._InputArguments);

			this._Score(this._InputArguments);
		} //end Score

		/// <summary>
		/// Method that takes in a filled ScoringInputs object, applies each scoring workflow defined in it to each noisy data file defined in it,
		/// collects performance metrics for each scoring workflow, and writes out scoring results and performance metrics files. 
		/// </summary>
		/// <param name="inputArguments">A filled ScoringInputs object.</param>
		private void _Score(ScoringInputs inputArguments) {
			this._PerformanceMetrics = new List<HitIdPerformance>();
			BasicHitIdPerformanceAnalyzer analyzer = new BasicHitIdPerformanceAnalyzer();

			//analyze each set of noisy data with each provided scoring workflow, write out results, and capture performance metrics
			foreach (string currNoisyDataFilePath in inputArguments.NoisyScreenDataFilePaths) {
				this._ParseSimulationIds(currNoisyDataFilePath);
				//find the screen info and hit set info relevant to this simulation
				ScreenInfo currScreenInfo = this._ScreenInfos.Find(x => x.ScreenId == this._ScreenId);
				HitSetInfo currHitSetInfo = this._HitSetInfos.Find(x => x.HitSetId == this._HitSetId);

				//load the data for this simulation
				ArrayedScreenData currNoisyData = this._LoadSimulationData(inputArguments, currNoisyDataFilePath);

				foreach (ScoringWorkflowInfo currWorkflowInfo in this._ScoringSetInfo.ScoringWorkflowInfos) {
					BenchmarkWorkflow currWorkflow = new BenchmarkWorkflow(currWorkflowInfo);
					IEnumerable<string> noisyReplicateNames = this._GetNoisyReplicateNames(currNoisyData);
					currWorkflow.Execute(currNoisyData, noisyReplicateNames);
					//TODO: output workflow results

					HitIdPerformance performanceMetric = analyzer.Analyze(currNoisyData); //pass in the hit info
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
		/// Method that reads in the scoring set info from the working directory
		/// and stores its contents into a member variable.
		/// </summary>
		/// <param name="inputs">A filled ScoringInputs object holding the user-specified scoring settings and working 
		/// directory.</param>
		private void _LoadInfos(ScoringInputs inputs) {
			StreamReader fileReader = File.OpenText(inputs.ScreenInfosFilePath);
			IScreenTextParser screenParser = new ScreenTextParser();
			this._ScreenInfos = screenParser.LoadScreenInfo(fileReader);

			fileReader = File.OpenText(inputs.HitSetInfosFilePath);
			IHitSetTextParser hitSetParser = new HitSetTextParser();
			this._HitSetInfos = hitSetParser.LoadHitSetInfo(fileReader);

			fileReader = File.OpenText(inputs.ScoringSetInfosFilePath);
			IScoringSetTextParser scoringSetParser = new ScoringSetTextParser();
			List<ScoringSetInfo> infosList = scoringSetParser.LoadScoringSetInfo(fileReader);
			if (infosList.Count != 1) {
				throw new Exception(string.Format(
					"Code can only accommodate one scoring set, but input file {0} contains {1} scoring sets",
					inputs.ScoringSetInfosFilePath, infosList.Count));
			}
			this._ScoringSetInfo = infosList[0];
		} //end _LoadScoringSetInfos

		/// <summary>
		/// Method that parses the file name of the noisy data file path to retrieve the screen id, hitset id, and noiseset id and store these in 
		/// member variables.
		/// </summary>
		/// <param name="simulatedDataFilePath">A string containing the file path to a noisy data file of the format/naming format created by the 
		/// Lsr.Bio.Benchmark.ScreenSimulator.</param>
		private void _ParseSimulationIds(string simulatedDataFilePath) {
			Regex nameRegEx = new Regex("screen(<screenid>\\d+)_hitset(<hitsetid>\\d+)_noiseset(<noisesetid>\\d+)"); //"\\d" means "any digit"
			MatchCollection matchCollection = nameRegEx.Matches(simulatedDataFilePath);
			if (matchCollection.Count != 1) {
				throw new ArgumentException(string.Format("Input simulation data file path {0} does not match the expected format 'screen<id>_hitset<id>_noiseset<id>'",
					simulatedDataFilePath));
			} //end if

			//uses capture of named groups from regex
			this._ScreenId = Convert.ToInt32(matchCollection[0].Groups["screenid"]);
			this._HitSetId = Convert.ToInt32(matchCollection[0].Groups["hitsetid"]);
			this._NoiseSetId = Convert.ToInt32(matchCollection[0].Groups["noisesetid"]);
		} //end _ParseSimulationIds

		/// <summary>
		/// Method that loads and returns an ArrayedScreenData object with data from the input file path, assuming the plate, row, and column dimensions in the 
		/// input ScoringInputs object.
		/// </summary>
		/// <param name="inputArguments">A filled ScoringInputs object defining the plate, row, and column dimensions of the data to load.</param>
		/// <param name="simulationsDataFilePath">A string containing the file path to the data to load.</param>
		/// <returns>A filled ArrayedScreenData object.</returns>
		private ArrayedScreenData _LoadSimulationData(ScoringInputs inputArguments, ScreenInfo screenInfo, string simulationsDataFilePath) {
			SimulationLoader simulationLoader = new SimulationLoader();
			StreamReader inReader = File.OpenText(simulationsDataFilePath);
			//TODO: figure out how to replace "tempsource" with something meaningful
			ArrayedScreenData result = simulationLoader.LoadFromReader<double>(inReader, "tempsource", inputArguments.NumPlates, inputArguments.NumRows,
				inputArguments.NumColumns);
			return result;
		} //end _LoadSimulationData

		/// <summary>
		/// Method that gets an enumerable of all the signal keys in the input ArrayedScreenData that start with the
		/// prefix used by NoiseMaker to indicate noisy replicates.
		/// </summary>
		/// <param name="currNoisyScreenData">A filled ArrayedScreenData object</param>
		/// <returns>An instantiated enumerable of strings of the names of all signals that start with the noisy replicate prefix.</returns>
		private IEnumerable<string> _GetNoisyReplicateNames(ArrayedScreenData currNoisyScreenData) {
			IList<string> result = new List<string>();
			foreach (string currKey in currNoisyScreenData.Signals.Keys) {
				if (currKey.StartsWith(ScreenNoiseGenerator.NoisyReplicatePrefix)) {
					result.Add(currKey);
				} //end if this signal represents a noisy replicate
			} //next signal key

			return result;
		} //end _GetNoisyReplicateNames


		//screen	hitset	noiseset	workflow	<workflow params>	valuetype	FPR	FNR	TPR	rating
		private string _WriteOutPerformanceMetrics() {
			StringBuilder stringBuilder = new StringBuilder();
			foreach (HitIdPerformance currPerformanceMetrics in this._PerformanceMetrics) {
				stringBuilder.AppendLine(currPerformanceMetrics.ToString());
			}
			return stringBuilder.ToString();
		} //end _WriteOutPerformanceMetrics
		#endregion
	} //end class
} //end namespace
