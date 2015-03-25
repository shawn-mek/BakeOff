using System;
using System.Linq;
using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Core {
    /// <summary>
    /// A dictionary of objects implementing the IPlatesetInfo interface, each keyed by a name or descriptive string
    /// </summary>
    public class KeyedIPlatesetInfos {
    	protected Dictionary<string, IPlatesetInfo> InternalKeyedIPlatesetInfos; 

        #region properties
    	public IPlatesetInfo this[string key] {
			get { return this.InternalKeyedIPlatesetInfos[key]; }
			set { this.InternalKeyedIPlatesetInfos[key] = value; }
    	}

    	/// <summary>
    	/// A non-negative number indicating the number of plates in each contained PlatesetInfo.  Zero if dictionary
    	/// contains no PlatesetInfos.
    	/// </summary>
    	public int NumPlates { get; private set; }

    	/// <summary>
    	/// A non-negative number indicating the number of rows in each plate.  Zero if dictionary contains no 
    	/// PlatesetInfos.
    	/// </summary>
    	public int NumRows { get; private set; }

    	/// <summary>
    	/// A non-negative number indicating the number of columns in each plate.  Zero if dictionary contains no 
    	/// PlatesetInfos.
    	/// </summary>
    	public int NumColumns { get; private set; }

		public int Count {
			get { return this.InternalKeyedIPlatesetInfos.Count; }
		}

    	public IEnumerable<string> Keys {
			get { return this.InternalKeyedIPlatesetInfos.Keys; }
    	} 

    	public IEnumerable<IPlatesetInfo> Values {
			get { return this.InternalKeyedIPlatesetInfos.Values; }
    	}
    	#endregion

        #region constructors
        /// <summary>
        /// Default constructor; does nothing.
        /// </summary>
        public KeyedIPlatesetInfos() {
        	this.InternalKeyedIPlatesetInfos = new Dictionary<string, IPlatesetInfo>();
        }

        /// <summary>
        /// Constructor that adds the input PlatesetInfos to the dictionary, keyed by their names, after validating
        /// that they all have the same plate, row, and column dimensions.
        /// </summary>
        /// <param name="inputIPlatesetInfos">A param array containing instantiated PlatesetInfos with data of type T</param>
        public KeyedIPlatesetInfos(params IPlatesetInfo[] inputIPlatesetInfos) : this() {
            foreach (IPlatesetInfo currIPlatesetInfo in inputIPlatesetInfos) {
                this.Add(currIPlatesetInfo);
            } //next plateset
        }
        #endregion

        #region methods
		public bool ContainsKey(string key) {
			return this.InternalKeyedIPlatesetInfos.ContainsKey(key);
		}

		public KeyValuePair<string, IPlatesetInfo> First() {
			return this.InternalKeyedIPlatesetInfos.First();
		}

        /// <summary>
        /// Method that adds the input PlatesetInfo to the dictionary, keyed by its name, after validating
        /// that it has the same plate, row, and column dimensions as any PlatesetInfos already in the dictionary.
        /// </summary>
        /// <param name="iplatesetInfo">An instantiated PlatesetInfo with data of type T</param>
        public void Add(IPlatesetInfo iplatesetInfo) {
			if (iplatesetInfo == null) { throw new ArgumentException("Input IPlatesetInfo may not be null"); }

			if (this.Count == 0) {
				//set the dimensions of all sets of info in the dictionary
				this.NumPlates = iplatesetInfo.NumPlates;
				this.NumRows = iplatesetInfo.NumRows;
				this.NumColumns = iplatesetInfo.NumColumns;
			} else {
				//validate that this plateset has same dimensions as others in dictionary
                if (iplatesetInfo.NumPlates != this.NumPlates) {
                    throw new ArgumentException(string.Format(
                        "Input IPlatesetInfo {0} has {1} plates, while existing dictionary contents have {2}",
						iplatesetInfo.Name, iplatesetInfo.NumPlates, this.NumPlates));
                }

                if (iplatesetInfo.NumRows != this.NumRows) {
					throw new ArgumentException(string.Format(
						"Input IPlatesetInfo {0} has {1} rows, while existing dictionary contents have {2}",
						iplatesetInfo.Name, iplatesetInfo.NumRows, this.NumRows));                   
                }

                if (iplatesetInfo.NumColumns != this.NumColumns) {
					throw new ArgumentException(string.Format(
						"Input IPlatesetInfo {0} has {1} columns, while existing dictionary contents have {2}",
						iplatesetInfo.Name, iplatesetInfo.NumColumns, this.NumColumns));                     
                }
            } //end if dictionary already doesn't/does have contents

            this.InternalKeyedIPlatesetInfos.Add(iplatesetInfo.Name, iplatesetInfo);
        } //end Add method

		public void Add<T>(KeyedPlatesetInfos<T> platesetInfos) {
			foreach (string currKey in platesetInfos.Keys) {
				this.Add(platesetInfos[currKey]);
			}
		}

		/// <summary>
		/// Method that returns the type of data held within the IPlatesetInfo
		/// </summary>
		/// <returns>A filled Type object indicating the type of data held within the IPlatesetInfo</returns>
		public Type GetPlatesetType() {
			Type result = null;
			if (this.Count > 0) {
				result = this.InternalKeyedIPlatesetInfos.First().Value.GetPlatesetType();
			}
			return result;
		} //end GetPlatesetType
        #endregion    	
    }
}
