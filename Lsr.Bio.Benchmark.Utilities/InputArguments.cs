using System;
using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.Benchmark.Utilities {
	/// <summary>
	/// Class that holds the inputs necessary to define a set of NoiseMaker simulation(s) to be generated and run
	/// automatically.
	/// </summary>
	public abstract class InputArguments {
		//not worth testing
		#region members
		/// <summary>
		/// Minimum number of arguments required for correctly-formed arguments.
		/// </summary>
		protected int MinNumArguments;

		/// <summary>
		/// String containing the directory in which input files will be found and output files will be written.
		/// </summary>
		public string WorkingDirectory { get; set; }

		/// <summary>
		/// Enumerable of strings of all input file names.
		/// </summary>
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
			if (args == null || args.Length < this.MinNumArguments) {
				throw new ArgumentException(string.Format(
					"Set of arguments is null or does not contain at least {0} items", this.MinNumArguments));
			} //end if args is null or contains too few items
		} //end _CheckNumArguments
	} //end class InputArguments
}
