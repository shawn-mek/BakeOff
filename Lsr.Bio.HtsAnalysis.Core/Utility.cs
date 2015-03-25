using System;

namespace Lsr.Bio.HtsAnalysis.Core {
	/// <summary>
	/// Class containing static methods useful for working with plate-based array data and PlatesetInfos.
	/// </summary>
    public static class Utility {
        /// <summary>
        /// Static method that instantiates an empty jagged array of the input number of plates, each of which contains an 
        /// array of the input number of rows, each of which contains an array of the input number of columns, all of 
        /// type T.
        /// </summary>
        /// <param name="numPlates">A non-negative number indicating the number of plates</param>
        /// <param name="numRows">A non-negative number indicating the number of rows on each plate</param>
        /// <param name="numColumns">A non-negative number indicating the number of columns on each plate</param>
        /// <returns>An instantiated but empty jagged, rectangular array of the input dimensions</returns>
        public static T[][][] InstantiateArray<T>(int numPlates, int numRows, int numColumns) {
            T[][][] result = new T[numPlates][][];
            for (int plateIndex = 0; plateIndex < numPlates; plateIndex++) {
                result[plateIndex] = new T[numRows][];
                for (int rowIndex = 0; rowIndex < numRows; rowIndex++) {
                    result[plateIndex][rowIndex] = new T[numColumns];
                } //next row
            } //next plate

            return result;
        } //end InstantiateArray

		/// <summary>
		/// Static method that instantiates an empty jagged array of the input number of rows, each of which contains an 
		/// array of the input number of columns, all of type Well{T}.
		/// </summary>
		/// <param name="numRows">A non-negative number indicating the number of rows on each plate</param>
		/// <param name="numColumns">A non-negative number indicating the number of columns on each plate</param>
		/// <returns>An instantiated but empty jagged, rectangular array of the input dimensions</returns>
		public static Well<T>[][] InstantiateArray<T>(int numRows, int numColumns) {
			Well<T>[][] result = new Well<T>[numRows][];
				for (int rowIndex = 0; rowIndex < numRows; rowIndex++) {
					result[rowIndex] = new Well<T>[numColumns];
				} //next row

			return result;
		} //end InstantiateArray


        /// <summary>
        /// Method that ensures the input jagged 3-D array is actually rectangular, in that the second and third dimensions
        /// of every top level array are the same
        /// </summary>
        /// <typeparam name="T">The type of object contained in the array</typeparam>
        /// <param name="inputArray">A jagged, three-dimensional array of objects of type T</param>
        public static void ValidateRectangularArray<T>(T[][][] inputArray) {
            if (inputArray == null) { throw new ArgumentException("input array may not be null"); }

            int? numRows = null;
            int? numCols = null;
            for (int plateIndex = 0; plateIndex < inputArray.Length; plateIndex++) {
                if (numRows == null) {
                    numRows = inputArray[plateIndex].Length;
                } else {
                    if (numRows != inputArray[plateIndex].Length) {
                        throw new ArgumentException("input array contains plate arrays with different numbers of rows");
                    } //end if this plate array doesn't have same number of rows as past ones
                } //end if this is/isn't first plate array checked

                for (int rowIndex = 0; rowIndex < numRows; rowIndex++) {
                    if (numCols == null) {
                        numCols = inputArray[plateIndex][rowIndex].Length;
                    } else {
                        if (numCols != inputArray[plateIndex][rowIndex].Length) {
                            throw new ArgumentException(
                                "input array contains row arrays with different numbers of columns");
                        } //end if this row array doesn't have same number of columns as past ones     
                    }//end if this is/isn't first row array checked
                } //next row index
            } //next plate index 
        } //end ValidateRectangularArray

		/// <summary>
		/// Method that flattens a three-dimensional jagged array of doubles into a one-dimensional array of 
		/// the same.
		/// </summary>
		/// <param name="array">A jagged, three-dimensional array of doubles</param>
		/// <returns>A filled array of doubles containing the flattened contents of the input array</returns>
		/// <remarks>Flattening is done by reading out the SECOND (middle) dimension first, then the first
		/// (innermost) dimension, then the third (top-level) dimension.  This is because plate data is typically
		/// represented as [plate][row][column] and but is read across rows, not down columns.  Thus, assuming a 
		/// plate had values
		/// 0 1 2
		/// 3 4 5
		/// it would be flattened to become
		/// 0 1 2 3 4 5</remarks>
        public static double[] Flatten(double[][][] array) {
            double[][] partiallyFlattened = new double[array.Length][];

            for (int i = 0; i < array.Length; i++) {
                double[][] subarray = array[i];
                partiallyFlattened[i] = Flatten(subarray);
            }

            return Flatten(partiallyFlattened);
        }

		/// <summary>
		/// Method that flattens a two-dimensional jagged array of doubles into a one-dimensional array of 
		/// the same.
		/// </summary>
		/// <param name="array">A jagged, two-dimensional array of doubles</param>
		/// <returns>A filled array of doubles containing the flattened contents of the input array</returns>
		/// <remarks>Flattening is done by reading out the first (top-level) dimension, then the second 
		/// (innermost) dimension.  This is because plate data is typically represented as [row][column] and but 
		/// is read across rows, not down columns.  Thus, assuming a plate had values
		/// 0 1 2
		/// 3 4 5
		/// it would be flattened to become
		/// 0 1 2 3 4 5</remarks>
        public static T[] Flatten<T>(T[][] array) {
            int index = 0;
            T[] result = new T[array.Length * array[0].Length];

            // ReSharper disable ForCanBeConvertedToForeach
            // I actually want to see the indices here, even though this *could* be a foreach
            for (int i = 0; i < array.Length; i++) {
                // ReSharper restore ForCanBeConvertedToForeach
                for (int j = 0; j < array[i].Length; j++) {
                    result[index] = array[i][j];
                    index++;
                }
            }
            return result;
        }


		/// <summary>
		/// Method that determines what plate, row, and column the input index of 
		/// a flattened array maps to; for instance, assuming 2 row x 3 column plates, a zero-based input index of 7
		/// maps to zero-based plate 1, zero-based row 0, and zero-based column 1.
		/// </summary>
		/// <param name="index">A zero-based index of the item of interest in a flattened array</param>
		/// <param name="numRows">A positive number indicating the number of rows per plate.</param>
		/// <param name="numCols">A positive number indicating the number of columns per plate.</param>
		/// <param name="plateIndex">Output parameter containing a zero-based non-negative index of the plate
		/// on which the input flattened index occurs.</param>
		/// <param name="rowIndex">Output parameter containing a zero-based non-negative index of the row
		/// in which the input flattened index occurs</param>
		/// <param name="colIndex">Output parameter containing a zero-based non-negative index of the column
		/// in which the input flattened index occurs</param>
		/// <remarks>ALL indexes are ZERO-based.  Method assumes that flattening was done by reading across each 
		/// row of each plate, so 
		/// 0 1 2
		/// 3 4 5
		/// became
		/// 0 1 2 3 4 5
		/// </remarks>
        public static void GetLocationFromFlattenedPlates(int index, int numRows, int numCols, 
            out int plateIndex, out int rowIndex, out int colIndex) {

            int numWellsOnCompletedPlates;
            int numWellsPerPlate = numRows * numCols;

            plateIndex = index/numWellsPerPlate; //integer division intended
            numWellsOnCompletedPlates = numWellsPerPlate * plateIndex;
            rowIndex = (index - numWellsOnCompletedPlates)/numCols; //integer division intended
            colIndex = (index - numWellsOnCompletedPlates)%numCols;
        }

    	/// <summary>
    	/// Method that determines what replicate, plate, row, and column the input index of 
    	/// a flattened array maps to; for instance, assuming 2 row x 3 column plates, a zero-based input index of 7
    	/// maps to zero-based replicate 0, zero-based plate 1, zero-based row 0, and zero-based column 1.
    	/// </summary>
    	/// <param name="index">A zero-based index of the item of interest in a flattened array</param>
    	/// <param name="numRows">A positive number indicating the number of rows per plate.</param>
    	/// <param name="numCols">A positive number indicating the number of columns per plate.</param>
		/// <param name="replicateIndex">Output parameter containing a zero-based non-negative index of the replicate
		/// in which the input flattened index occurs</param>
    	/// <param name="plateIndex">Output parameter containing a zero-based non-negative index of the plate
    	/// on which the input flattened index occurs.</param>
    	/// <param name="rowIndex">Output parameter containing a zero-based non-negative index of the row
    	/// in which the input flattened index occurs</param>
    	/// <param name="colIndex">Output parameter containing a zero-based non-negative index of the column
    	/// in which the input flattened index occurs</param>
    	/// <remarks>ALL indexes are ZERO-based.  Method assumes that flattening was done by reading across each 
    	/// row of each plate, so 
    	/// 0 1 2
    	/// 3 4 5
    	/// became
    	/// 0 1 2 3 4 5
    	/// Additionally, the method assumes that the first replicates of all plates are listed together, followed 
    	/// by the second replicates of all plates, etc.  Thus, if there are two 6-well plates, each with two 
    	/// replicates, the first 6 values are for the first replicate of the first plate, the second 6 values are for 
    	/// the first replicate of the second plate, the third 6 values are for the second replicate of the first 
    	/// plate, etc.
    	/// </remarks>
    	public static void GetLocationFromFlattenedReplicates(int index, int numRows, int numCols,
            out int replicateIndex, out int plateIndex, out int rowIndex, out int colIndex) {

            int numWellsInCompletedReplicates;
            int numWellsOnCompletedPlates;
            int numWellsPerPlate = numRows * numCols;

            replicateIndex = index / numWellsPerPlate; //integer division intended
            numWellsInCompletedReplicates = numWellsPerPlate * replicateIndex;
            plateIndex = (index - numWellsInCompletedReplicates) / numWellsPerPlate; //integer division intended
            numWellsOnCompletedPlates = numWellsInCompletedReplicates + (numWellsPerPlate * plateIndex);
            rowIndex = (index - numWellsOnCompletedPlates) / numCols; //integer division intended
            colIndex = (index - numWellsOnCompletedPlates) % numCols;
        }
    } //end class
} //end namespace
