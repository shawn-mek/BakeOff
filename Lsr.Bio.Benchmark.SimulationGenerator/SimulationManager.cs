using System;
using System.Collections.Generic;
using System.IO;
using Lsr.Bio.Benchmark.ScreenSimulator.HitSet;
using Lsr.Bio.Benchmark.ScreenSimulator.NoiseSet;
using Lsr.Bio.Benchmark.ScreenSimulator.Screen;

namespace Lsr.Bio.Benchmark.ScreenSimulator {
	/// <summary>
	/// Class that takes in inputs defining the characteristics of the desired NoiseMaker simulation(s), runs those 
	/// simulations automatically, and writes out the associated results files.
	/// </summary>
	public class SimulationManager {

		#region members
		/// <summary>
		/// Object holding input arguments needed to set up simulation(s).
		/// </summary>
		private SimulationInputs _InputArguments;

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
		/// List of filled NoiseSetInfos objects holding the definitions of the noise scenarios to be applied to 
		/// each hit set generated within each screen.
		/// </summary>
		private List<NoiseSetInfo> _NoiseSetInfos;
		#endregion

		#region public methods
		/// <summary>
		/// Method that takes in user arguments defining the simulations to run, runs those 
		/// simulations automatically, and writes out the associated results files.
		/// </summary>
		/// <param name="args">A string array containing at least 5 items: working directory, plate map file name,
		/// screen infos file name, hit set infos file name, and noise set infos file name (in that order).</param>
		public void GenerateNoisyScreens(string[] args) {
			this._ResetState();

			this._InputArguments = new SimulationInputs(args);
			IList<string> validationMsgs = this._InputArguments.Validate();
			if (validationMsgs.Count > 0) {
				throw new Exception(string.Format("Validation issues detected:{0}",
					string.Join(Environment.NewLine, validationMsgs)));
			} //end if there were validation problems

			this._LoadInfos(this._InputArguments);

			this._GenerateNoisyScreens(this._InputArguments.WorkingDirectory, this._InputArguments.PlateMapFilePath);
		} //end GenerateNoisyScreens
		#endregion

		#region private methods
		/// <summary>
		/// Method that sets all member variables to null to clear state before running a new set of simulations.
		/// </summary>
		private void _ResetState() {
			this._InputArguments = null;
			this._ScreenInfos = null;
			this._HitSetInfos = null;
			this._NoiseSetInfos = null;
		} //end _ResetState

		/// <summary>
		/// Method that reads in the screen infos, hit set infos, and noise set infos files from the working directory
		/// and stores their contents into member variables.
		/// </summary>
		/// <param name="inputs">A filled InputArguments object holding the user-specified input file names and working 
		/// directory for the simulation(s).</param>
		private void _LoadInfos(SimulationInputs inputs) {
			StreamReader fileReader = File.OpenText(inputs.ScreenInfosFilePath);
			IScreenTextParser screenParser = new ScreenTextParser();
			this._ScreenInfos = screenParser.LoadScreenInfo(fileReader);

			fileReader = File.OpenText(inputs.HitSetInfosFilePath);
			IHitSetTextParser hitSetParser = new HitSetTextParser();
			this._HitSetInfos = hitSetParser.LoadHitSetInfo(fileReader);

			fileReader = File.OpenText(inputs.NoiseSetInfosFilePath);
			INoiseSetTextParser noiseSetParser = new NoiseSetTextParser();
			this._NoiseSetInfos = noiseSetParser.LoadNoiseSetInfo(fileReader);
		} //end _LoadInfos

		/// <summary>
		/// Method that loops over each screen info and hit set and creates true data for each combination of the 
		/// two, then creates as many noisy data sets as there are noise set infos for each true data set.
		/// </summary>
		/// <param name="workingDir">String containing the directory in which output files will be written.</param>
		/// <param name="plateMapFilePath">String containing the file name (not path) of a file holding the plate map for 
		/// the simulations to be run.</param>
		private void _GenerateNoisyScreens(string workingDir, string plateMapFilePath) {
			foreach (ScreenInfo currScreenInfo in this._ScreenInfos) {
				foreach (HitSetInfo currHitSetInfo in this._HitSetInfos) {
					string trueHitsFilePath = this._GenerateTrueData(workingDir, plateMapFilePath,
						currScreenInfo, currHitSetInfo);

					foreach (NoiseSetInfo currNoiseSetInfo in this._NoiseSetInfos) {
						this._GenerateNoisyData(workingDir, currScreenInfo, trueHitsFilePath, currNoiseSetInfo);
					} //next noise set
				} //next hit set
			} //next screen
		} //end _GenerateNoisyScreens

		/// <summary>
		/// Method that generates random true hits for the input platemap, screen info, and hit set info, stores
		/// those true hits to a file, and returns the path to that file.
		/// </summary>
		/// <param name="workingDir">String containing the directory in which output files will be written.</param>
		/// <param name="plateMapFilePath">String containing the file name (not path) of a file holding the plate map for 
		/// the simulations to be run.</param>
		/// <param name="screenInfo">Filled ScreenInfo object holding the definition of the screen with which the true
		/// data are associated.</param>
		/// <param name="hitSetInfo">Filled HitSetInfo object holding the definition of the hit set with which the true
		/// data are associated.</param>
		/// <returns>A string containing the full path to the newly-created file holding the true data.</returns>
		private string _GenerateTrueData(string workingDir, string plateMapFilePath, ScreenInfo screenInfo,
			HitSetInfo hitSetInfo) {

			string trueHitsFileName = string.Format("screen{0}_hitset{1}.txt", screenInfo.ScreenId,
				hitSetInfo.HitSetId);
			string trueHitsFilePath = Path.Combine(workingDir, trueHitsFileName);
			NoiseMaker.RandomHitMaker hitMaker = new NoiseMaker.RandomHitMaker();
			hitMaker.MakeRandomHits(plateMapFilePath, screenInfo.ControlInfosDict,
				hitSetInfo.HitInfos, screenInfo.DefaultValue, trueHitsFilePath);
			return trueHitsFilePath;
		} //end _GenerateTrueData

		/// <summary>
		/// Method that generates noisy data for the input screen info, true hits, and noise set info, stores
		/// those noisy data to a file, and returns the path to that file.
		/// </summary>
		/// <param name="workingDir">String containing the directory in which output files will be written.</param>
		/// <param name="screenInfo">Filled ScreenInfo object holding the definition of the screen with which the noisy
		/// data are associated.</param>
		/// <param name="trueHitsFilePath">>A string containing the full path to the file holding the true data.</param>
		/// <param name="noiseSetInfo">Filled NoiseSetInfo object holding the definition of the noise set with which the
		/// noisy data are associated.</param>
		/// <returns>A string containing the full path to the newly-created file holding the noisy data.</returns>
		private string _GenerateNoisyData(string workingDir, ScreenInfo screenInfo, string trueHitsFilePath,
			NoiseSetInfo noiseSetInfo) {

			string noisyDataFileName = trueHitsFilePath.Replace(".txt", string.Format("_noiseset{0}.txt",
					noiseSetInfo.NoiseSetId));
			string noisyDataFilePath = Path.Combine(workingDir, noisyDataFileName);
			NoiseMaker.ScreenNoiseGenerator generator = new NoiseMaker.ScreenNoiseGenerator(
				screenInfo.NumReplicates, screenInfo.NumRows, screenInfo.NumColumns,
				noiseSetInfo.Floor, noiseSetInfo.Ceiling);
			generator.AddScreenNoise(trueHitsFilePath, noiseSetInfo.NoiseGeneratorInfos,
				noisyDataFilePath);
			return noisyDataFilePath;
		} //end _GenerateNoisyData
		#endregion
	} //end class SimulationManager
}
