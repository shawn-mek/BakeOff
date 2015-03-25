using System;

namespace Lsr.Bio.HtsAnalysis.Core {
    /// <summary>
    /// Public interface defining properties of objects that can participate in a PlatesetDictionary.  An interface is 
    /// used for this instead of a class because a PlatesetDictionary may mix PlatesetInfos of various different base types.
   /// </summary>
   public interface IPlatesetInfo {
        #region properties
        /// <summary>
        /// A string containing the name of the IPlatesetInfo
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A filled Provenance object indicating the history of this IPlatesetInfo
        /// </summary>
        Provenance Provenance { get; }

    	int NumPlates { get; }

    	int NumRows { get; }

    	int NumColumns { get; }
    	#endregion

		#region methods
		/// <summary>
		/// Method that returns the type of data held within the IPlatesetInfo
		/// </summary>
		/// <returns>A filled Type object indicating the type of data held within the IPlatesetInfo</returns>
		Type GetPlatesetType();

		/// <summary>
		/// Method that casts a type-agnostic IPlatesetInfo to a PlatesetInfo object of particular type T
		/// </summary>
		/// <typeparam name="T">The type of data held within the IPlatesetInfo</typeparam>
		/// <returns>A filled PlatesetInfo object holding data of type T</returns>
		PlatesetInfo<T> CastToPlatesetInfo<T>();

    	bool GetIgnoreValue(int plateIndex, int rowIndex, int colIndex);
    	#endregion
   }
}
