namespace Lsr.Bio.HtsAnalysis.Core {
    /// <summary>
    /// Class to hold information about arrayed (plate-based) screen data and the reagents used to generate these data.
    /// TODO: remove or enforce: It is assumed that all signal sets, and the reagent set, in the ArrayedScreenData have the same number of plates,
    /// rows, and columns.
    /// </summary>
    public class ArrayedScreenData {
        #region members
        /// <summary>
        /// Standard name of the Reagents PlatesetInfo
        /// </summary>
        public readonly string REAGENT_PLATESET_NAME = "reagents";
        #endregion

        #region properties
        /// <summary>
        /// A string containing a name for this data set
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// A non-negative number indicating the number of plates included in each Signal set and the Reagents set
        /// </summary>
        public int NumPlates {get; set;}

        /// <summary>
        /// A non-negative number indicating the number of rows in each plate
        /// </summary>
        public int NumRows {get; set;}

        /// <summary>
        /// A non-negative number indicating the number of columns in each plate
        /// </summary>
        public int NumColumns {get; set;}

        /// <summary>
        /// A PlatesetInfo of Reagent objects, indicating which reagent is at which row and column position on each plate
        /// </summary>
        public PlatesetInfo<Reagent> Reagents { get; set; }

        /// <summary>
        /// A PlatesetDictionary of IPlatesetInfos containing data for the arrayed screen; this may be raw data, processed 
        /// data, or a combination of both.  Each IPlatesetInfo is keyed by a descriptive string.
        /// </summary>
        public KeyedIPlatesetInfos Signals {get; set;}
        #endregion

        #region constructors
        /// <summary>
        /// Constructor that creates a new ArrayedScreenData object with the input number of plates, rows, and columns
        /// </summary>
        /// <param name="numPlates">A non-negative number indicating the number of plates included in each Signal set 
        /// and the Reagents set</param>
        /// <param name="numRows">A non-negative number indicating the number of rows in each plate</param>
        /// <param name="numColumns">A non-negative number indicating the number of columns in each plate</param>
        public ArrayedScreenData(int numPlates, int numRows, int numColumns) {
            this.NumPlates = numPlates;
            this.NumRows = numRows;
            this.NumColumns = numColumns;
            this.Reagents = null;
            this.Signals = new KeyedIPlatesetInfos();
        } //end constructor
        #endregion
    } //end class
}
