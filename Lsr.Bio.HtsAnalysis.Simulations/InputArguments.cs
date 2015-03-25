using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lsr.Bio.HtsAnalysis.Simulations {
	/// <summary>
	/// Class that holds the inputs necessary to define a set of NoiseMaker simulation(s) to be generated and run
	/// automatically.
	/// </summary>
	internal abstract class InputArguments {
		//not worth testing
		#region members
		protected int MIN_NUM_ARGUMENTS;

		/// <summary>
		/// String containing the directory in which input files will be found and output files will be written.
		/// </summary>
		public string WorkingDirectory { get; set; }

		protected virtual IEnumerable<string> InputFileNames {
			get {return  new List<string>();}
		} 
		#endregion

		#region public methods
		/// <summary>
		/// Public method that checks whether the working directory exists and contains one and only one instance
		/// of each of the input files.
		/// </summary>
		/// <returns>An instantiated list of messages reporting validation failures; empty if no failures were found.
		/// </returns>
		public IList<string> Validate() {
			IEnumerable<string> inputFileNames = this.InputFileNames;
			IList<string> result = new List<string>();

			//check if the working directory exists
			DirectoryInfo dirInfo = new DirectoryInfo(this.WorkingDirectory);
			if (!dirInfo.Exists) {
				result.Add(string.Format("Working directory '{0}' does not exist", this.WorkingDirectory));
			}

			//check if each of the input files exists in the working directory
			foreach (string currFileName in inputFileNames) {
				FileInfo[] files = dirInfo.GetFiles(currFileName);
				if (files.Length != 1) {
					result.Add(string.Format("Found {0} files with name {1} in directory {2}; expected 1.",
						files.Length, currFileName, this.WorkingDirectory));
				} //end if one and only one file of given name not found in working directory
			} //next input file name
			return result;
		} //end Validate
		#endregion

		/// <summary>
		/// Method that checks if the input array has the expected number of arguments and throws an
		/// exception otherwise.
		/// </summary>
		/// <param name="args">A string array containing at least the expected number of arguments.</param>
		/// <exception cref="ArgumentException">Thrown if the input is null or fewer than the expected 
		/// number of items long.</exception>
		protected void CheckNumArguments(string[] args) {
			if (args == null || args.Length < this.MIN_NUM_ARGUMENTS) {
				throw new ArgumentException(string.Format(
					"Set of arguments is null or does not contain at least {0} items", this.MIN_NUM_ARGUMENTS));
			} //end if args is null or contains too few items
		} //end _CheckNumArguments
	} //end class InputArguments

	internal class DataSimulationInputArguments: InputArguments {
		#region members
		/// <summary>
		/// String containing the file name (not path) of a file holding the plate map for the simulations to be run.
		/// </summary>
		public string PlateMapFileName { get; set; }

		/// <summary>
		/// String containing the file name (not path) of a file holding the definition(s) of the screen(s) to be
		/// simulated.
		/// </summary>
		public string ScreenInfosFileName { get; set; }

		/// <summary>
		/// String holding the file name (not path) of a file holding the definition(s) of the hit set(s) to be 
		/// assigned within each simulated screen.
		/// </summary>
		public string HitSetInfosFileName { get; set; }

		/// <summary>
		/// String holding the file name (not path) of a file holding the definition(s) of the noise set(s) to be
		/// applied to each hit set within each simulated screen.
		/// </summary>
		public string NoiseSetInfosFileName { get; set; }
		#endregion

		#region properties
		/// <summary>
		/// String containing the full path to the plate map file.
		/// </summary>
		public string PlateMapFilePath {
			get { return Path.Combine(this.WorkingDirectory, this.PlateMapFileName); }
		}

		/// <summary>
		/// String containing the full path to the screen infos file.
		/// </summary>
		public string ScreenInfosFilePath {
			get { return Path.Combine(this.WorkingDirectory, this.ScreenInfosFileName); }
		}

		/// <summary>
		/// String containing the full path to the hit set infos file.
		/// </summary>
		public string HitSetInfosFilePath {
			get { return Path.Combine(this.WorkingDirectory, this.HitSetInfosFileName); }
		}

		/// <summary>
		/// String containing the full path to the noise set infos file.
		/// </summary>
		public string NoiseSetInfosFilePath {
			get { return Path.Combine(this.WorkingDirectory, this.NoiseSetInfosFileName); }
		}

		protected override IEnumerable<string> InputFileNames {
			get {
				List<string> result = new List<string>{this.PlateMapFileName, this.ScreenInfosFileName,
                    this.HitSetInfosFileName, this.NoiseSetInfosFileName};
				return result;
			}
		}
		#endregion

		#region constructors
		/// <summary>
		/// Default constructor; does nothing.
		/// </summary>
		public DataSimulationInputArguments() {
			this.MIN_NUM_ARGUMENTS = 5;
		}

		/// <summary>
		/// Constructor that initializes WorkingDirectory and 4 input file name members from list of input arguments.
		/// Expects input string to have (at least) 5 entries; does not use any subsequent entries.
		/// </summary>
		/// <param name="args">A string array containing at least 5 items: working directory, plate map file name,
		/// screen infos file name, hit set infos file name, and noise set infos file name (in that order).</param>
		/// <exception cref="ArgumentException">Thrown if the input is null or fewer than 5 items long.</exception>
		public DataSimulationInputArguments(string[] args) : this() {
			this.CheckNumArguments(args);

			this.WorkingDirectory = args[0];
			this.PlateMapFileName = args[1];
			this.ScreenInfosFileName = args[2];
			this.HitSetInfosFileName = args[3];
			this.NoiseSetInfosFileName = args[4];
		} //end constructor
		#endregion
	} //end class DataSimulationInputArguments

	internal class ScoringSimulationInputArguments: InputArguments {
		#region members
		/// <summary>
		/// String containing the file name (not path) of a file holding the definition(s) of the 
		/// scoring sets to be applied to the simulated data.
		/// </summary>
		public string ScoringSetInfosFileName { get; set; }

		/// <summary>

		/// </summary>
		public IEnumerable<string> NoisyScreenDataFileNames { get; set; }
		#endregion

		#region properties
		/// <summary>

		/// </summary>
		public IEnumerable<string> NoisyScreenDataFilePaths {
			get {
				return this.NoisyScreenDataFileNames.Select(currFileName => 
					Path.Combine(this.WorkingDirectory, currFileName)).ToList();
			}
		}

		/// <summary>
		/// String containing the full path to the scoring set infos file
		/// </summary>
		public string ScoringSetInfosFilePath {
			get { return Path.Combine(this.WorkingDirectory, this.ScoringSetInfosFileName); }
		}


		protected override IEnumerable<string> InputFileNames {
			get {
				List<string> result = new List<string>();
				result.AddRange(this.NoisyScreenDataFileNames);
				result.Add(this.ScoringSetInfosFileName);
				return result;
			}
		}
		#endregion

		#region constructors
		/// <summary>
		/// Default constructor; does nothing.
		/// </summary>
		public ScoringSimulationInputArguments() {
			this.MIN_NUM_ARGUMENTS = 3;
		}

		/// <summary>
		/// Constructor that initializes WorkingDirectory and 4 input file name members from list of input arguments.
		/// Expects input string to have (at least) 5 entries; does not use any subsequent entries.
		/// </summary>
		/// <param name="args">A string array containing at least 5 items: working directory, plate map file name,
		/// screen infos file name, hit set infos file name, and noise set infos file name (in that order).</param>
		/// <exception cref="ArgumentException">Thrown if the input is null or fewer than 5 items long.</exception>
		public ScoringSimulationInputArguments(string[] args) : this() {
			this.CheckNumArguments(args);

			this.WorkingDirectory = args[0];
			this.ScoringSetInfosFileName = args[1];
			List<string> dataSetFileNames = new List<string>();
			for (int i = 2; i < args.Length; i++ ) {
				dataSetFileNames.Add(args[i]);
			} //next argument index
			this.NoisyScreenDataFileNames = dataSetFileNames;
		} //end constructor
		#endregion
	} //end class ScoringSimulationInputArguments
}
