using System;
using System.Collections.Generic;
using System.IO;
using Lsr.Bio.Benchmark.Utilities;

namespace Lsr.Bio.Benchmark.ScreenSimulator {
	class SimulationInputs: InputArguments {
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

		/// <summary>
		/// Enumerable of strings of all input file names.
		/// </summary>
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
		public SimulationInputs() {
			this.MinNumArguments = 5;
		}

		/// <summary>
		/// Constructor that initializes WorkingDirectory and 4 input file name members from list of input arguments.
		/// Expects input string to have (at least) 5 entries; does not use any subsequent entries.
		/// </summary>
		/// <param name="args">A string array containing at least 5 items: working directory, plate map file name,
		/// screen infos file name, hit set infos file name, and noise set infos file name (in that order).</param>
		/// <exception cref="ArgumentException">Thrown if the input is null or fewer than 5 items long.</exception>
		public SimulationInputs(string[] args) : this() {
			this.CheckNumArguments(args);

			this.WorkingDirectory = args[0];
			this.PlateMapFileName = args[1];
			this.ScreenInfosFileName = args[2];
			this.HitSetInfosFileName = args[3];
			this.NoiseSetInfosFileName = args[4];
		} //end constructor
		#endregion
	}
}
