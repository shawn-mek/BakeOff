using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lsr.Bio.Benchmark.Utilities;

namespace Lsr.Bio.Benchmark.ScreenScorer {
	internal class ScoringInputs: InputArguments {
		#region members
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
		/// String containing the file name (not path) of a file holding the definition(s) of the 
		/// scoring sets to be applied to the simulated data.
		/// </summary>
		public string ScoringSetInfosFileName { get; set; }

		/// <summary>
		/// Strings containing the file name(s) (not path(s)) of file(s) holding the simulated noisy screen data.
		/// </summary>
		public IEnumerable<string> NoisyScreenDataFileNames { get; set; }
		#endregion

		#region properties
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
		/// String containing the full path to the scoring set infos file.
		/// </summary>
		public string ScoringSetInfosFilePath {
			get { return Path.Combine(this.WorkingDirectory, this.ScoringSetInfosFileName); }
		}

		/// <summary>
		/// Enumerable of string(s) containing the full path(s) to the simulated noisy screen data files.
		/// </summary>
		public IEnumerable<string> NoisyScreenDataFilePaths {
			get {
				return this.NoisyScreenDataFileNames.Select(currFileName =>
					Path.Combine(this.WorkingDirectory, currFileName)).ToList();
			}
		}
		/// <summary>
		/// Enumerable of strings of all input file names.
		/// </summary>
		protected override IEnumerable<string> InputFileNames {
			get {
				List<string> result = new List<string> {this.ScreenInfosFileName, this.HitSetInfosFileName, this.ScoringSetInfosFileName};
				result.AddRange(this.NoisyScreenDataFileNames);
				return result;
			}
		}
		#endregion

		#region constructors
		/// <summary>
		/// Default constructor; does nothing.
		/// </summary>
		public ScoringInputs() {
			this.MinNumArguments = 4;
		}

		/// <summary>
		/// Constructor that initializes WorkingDirectory, the scoring set info file name, and at least one (and possibly more than one)
		/// file name of noisy input data files.
		/// </summary>
		/// <param name="args">A string array containing at least MIN_NUM_ARGUMENTS items: working directory, screen infos file name, hit infos file name, 
		/// scoring set info file name, and noisy data file name(s) (in that order).
		/// </param>
		/// <exception cref="ArgumentException">Thrown if the input is null or fewer than MIN_NUM_ARGUMENTS items long.</exception>
		public ScoringInputs(string[] args)
			: this() {
			this.CheckNumArguments(args);

			this.WorkingDirectory = args[0];
			this.ScreenInfosFileName = args[1];
			this.HitSetInfosFileName = args[2];
			this.ScoringSetInfosFileName = args[3];
			List<string> dataSetFileNames = new List<string>();
			for (int i = 5; i < args.Length; i++) {
				dataSetFileNames.Add(args[i]);
			} //next argument index
			this.NoisyScreenDataFileNames = dataSetFileNames;
		} //end constructor
		#endregion
	} //end class ScorerInputs

}
