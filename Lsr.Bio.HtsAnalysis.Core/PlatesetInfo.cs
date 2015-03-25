using System;

namespace Lsr.Bio.HtsAnalysis.Core {
    /// <summary>
    /// Public class holding data of type T for a set of rectangular microtiter plates
    /// </summary>
    /// <typeparam name="T">The type of data contained in the PlatesetInfo</typeparam>
    public class PlatesetInfo<T> : IPlatesetInfo {
        #region members
        /// <summary>
        /// Plateset indicating true if the data in _PLATESET for that position should be ignored, false otherwise.  
        /// Used to exclude outliers, contaminated wells, etc, from data analysis
        /// </summary>
        private readonly Plateset<bool> _IGNORE_DATA_AT_POSITION;

        /// <summary>
        /// Plateset holding data of type T for each well position
        /// </summary>
        private readonly Plateset<T> _PLATESET;
        #endregion

        #region properties
        #region IPlateSet implementations
    	/// <summary>
    	/// String containing the name of this PlatesetInfo
    	/// </summary>
    	public string Name { get; private set; }

    	/// <summary>
    	/// A filled Provenance object indicating the history of this PlatesetInfo
    	/// </summary>
    	public Provenance Provenance { get; private set; }

		/// <summary>
		/// A non-negative number indicating the number of plates in this PlatesetInfo
		/// </summary>
		public int NumPlates { get; private set; }

		/// <summary>
		/// A non-negative number indicating the number of rows in each plate
		/// </summary>
		public int NumRows { get; private set; }

		/// <summary>
		/// A non-negative number indicating the number of columns in each plate
		/// </summary>
		public int NumColumns { get; private set; }
        #endregion IPlateSet implementations

        /// <summary>
        /// Indexer returning a Plate object of type T holding data for the plate with the input plate index
        /// </summary>
        /// <param name="plateIndex">A non-negative number indicating the 0-based index of the plate for which to
        /// return data</param>
        /// <returns>A Plate object holding data for the plate with the input plate index</returns>
        public Plate<T> this[int plateIndex] {
            get { return this._PLATESET[plateIndex]; }
        }

        /// <summary>
        /// Indexer returning the data value, of type T, for the input plate index, row index, and column index
        /// </summary>
        /// <param name="plateIndex">A non-negative number indicating the 0-based index of the plate for which to
        /// return data</param>
        /// <param name="rowIndex">A non-negative number indicating the 0-based index of the row for which to
        /// return data</param>
        /// <param name="colIndex">A non-negative number indicating the 0-based index of the column for which to
        /// return data</param>
        /// <returns>A Well, containing data of type T, for the input plate index, row index, and column index</returns>
        /// <remarks>The requested Well is returned even if it has a value of true in _IgnoreDataAtPosition</remarks>
        public Well<T> this[int plateIndex, int rowIndex, int colIndex] {
            get { return this._PLATESET[plateIndex, rowIndex, colIndex]; }
            set { this._PLATESET[plateIndex, rowIndex, colIndex] = value; }
        }

        /// <summary>
        /// Plateset holding data of type T for each well position
        /// </summary>
        public Plateset<T> Plateset {
            get { return this._PLATESET; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Constructor that takes in the number of plates, rows, and columns, and creates an empty PlatesetInfo with
        /// these dimensions
        /// </summary>
        /// <param name="name">String containing the name of this PlatesetInfo; should be unique since is used as key if 
        /// PlatesetInfo is added to TypedPlatesetDictionary</param>
        /// <param name="provenance">A filled Provenance object indicating the history of this PlatesetInfo</param>
        /// <param name="numPlates">A non-negative number indicating the number of plates in this PlatesetInfo</param>
        /// <param name="numRows">A non-negative number indicating the number of rows in each plate</param>
        /// <param name="numColumns">A non-negative number indicating the number of columns in each plate</param>
        public PlatesetInfo(string name, Provenance provenance, int numPlates, int numRows, int numColumns) {
            this.Name = name;
            this.Provenance = provenance;
            this.NumPlates = numPlates;
            this.NumRows = numRows;
            this.NumColumns = numColumns;
            this._PLATESET = new Plateset<T>(numPlates, numRows, numColumns);
            this._IGNORE_DATA_AT_POSITION = new Plateset<bool>(numPlates, numRows, numColumns);
        } //end constructor
        #endregion

        #region methods
		#region IPlateSet implementations
		/// <summary>
		/// Method that returns the type of data held within the IPlatesetInfo
		/// </summary>
		/// <returns>A filled Type object indicating the type of data held within the IPlatesetInfo</returns>
		public Type GetPlatesetType() {
			return typeof(T);
		} //end GetPlatesetType

		/// <summary>
		/// Method that casts an IPlatesetInfo to a PlatesetInfo object of particular type TOutput.  An error is thrown
		/// if the PlatesetInfo is not of type TOutput and therefore cannot be cast to it.
		/// </summary>
		/// <typeparam name="TOutput">The type of data to which the PlatesetInfo should be cast</typeparam>
		/// <returns>A filled PlatesetInfo object holding data of type TOutput</returns>
		/// <exception cref="ArgumentException">Thrown if TOutput is not the same as T and therefore PlatesetInfo 
		/// cannot be cast to type TOutput</exception>
		public PlatesetInfo<TOutput> CastToPlatesetInfo<TOutput>() {
			PlatesetInfo<TOutput> result = this as PlatesetInfo<TOutput>;
			if (result == null) {
				throw new ArgumentException(string.Format("Cannot cast PlatesetInfo<{0}> to PlatesetInfo<{1}>",
					typeof(T), typeof(TOutput)));
			} //end if type being cast to is not the same as PlatesetInfo type
			return result;
		} //end CastToPlatesetInfo

		/// <summary>
		/// Method that returns a boolean indicating whether or not the data value at the input plate, row, and column 
		/// index should be ignored during data analysis
		/// </summary>
		/// <param name="plateIndex">A non-negative number indicating the 0-based index of the plate for which to
		/// return data</param>
		/// <param name="rowIndex">A non-negative number indicating the 0-based index of the row for which to
		/// return data</param>
		/// <param name="colIndex">A non-negative number indicating the 0-based index of the column for which to
		/// return data</param>
		/// <returns>A boolean indicating whether or not the data value at the input plate, row, and column index
		/// should be ignored during data analysis</returns>
		public bool GetIgnoreValue(int plateIndex, int rowIndex, int colIndex) {
			return this._IGNORE_DATA_AT_POSITION[plateIndex, rowIndex, colIndex].Value;
		} //end GetIgnoreValue
		#endregion IPlateSet implementations
        #endregion methods
    } //end class
}
