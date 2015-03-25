namespace Lsr.Bio.HtsAnalysis.Core {
    /// <summary>
    /// Class that stores information about a reagent used in an arrayed screen
    /// </summary>
    public class Reagent {
		#region properties
		/// <summary>
        /// A string containing an identifier for this reagent
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A string containing a name for this reagent
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A string containing a descriptor for this reagent, such as "sample" or "negative control"
        /// </summary>
        public string Kind { get; set; }
		#endregion properties

		#region constructors
		/// <summary>
        /// Constructor that creates a new Reagent from the input id, name, and kind
        /// </summary>
        /// <param name="id">A string containing an identifier for this reagent</param>
        /// <param name="name">A string containing a name for this reagent</param>
        /// <param name="kind">A string containing a descriptor for this reagent, such as "sample" or "negative control"
        /// </param>
        public Reagent(string id, string name, string kind) {
            this.Id = id;
            this.Name = name;
            this.Kind = kind;
        } //end constructor
		#endregion constructors
	} //end class
}
