namespace Lsr.Bio.HtsAnalysis.Scoring.Transformers {
	/// <summary>
	/// Transformer that identifies screening samples as either hits or non-hits based on their values and a
	/// specified hit identification method
	/// </summary>
    class HitIdTransformer: Transformer<double, bool> {
		#region members
		/// <summary>
		/// Delegate that takes in a double value and returns true if it should be treated as a hit and 
		/// false otherwise.
		/// </summary>
		/// <param name="input">A double representing a screening value to call as a hit or non-hit</param>
		/// <returns>True if the input is a hit, false if it is not</returns>
        public delegate bool CallHit(double input);

		/// <summary>
		/// A method of the type CallHit that specifies how this particular HitIdTransformer object identifies hits
		/// </summary>
        protected CallHit HitIdMethod;

		/// <summary>
		/// A string containing a name describing this particular HitIdTransformer object, such as 
		/// "score > 2"
		/// </summary>
        protected string Name;
		#endregion members

		#region constructors
		/// <summary>
		/// Constructor that takes a name and hit identification method to create a specific HitIdTransformer
		/// </summary>
		/// <param name="name">A string containing a name describing this particular HitIdTransformer</param>
		/// <param name="hitIdMethod"> A method of the type CallHit that specifies how this particular HitIdTransformer
		/// identifies hits</param>
		public HitIdTransformer(string name, CallHit hitIdMethod) {
            this.Name = name;
            this.HitIdMethod = hitIdMethod;
        }
		#endregion constructors

		#region methods
		/// <summary>
		/// Override of virtual GenerateDescriptor method.  The abstract method just returns the class name
		/// (HitIdTransformer) but this is not specific enough to describe a particular instance of this class,
		/// so a user-specified (and hopefully more descriptive!) value is returned instead.
		/// </summary>
		/// <returns></returns>
		public override string GenerateDescriptor() {
			return this.Name;
		} //end GenerateDescriptor

		/// <summary>
		/// Override of abstract Transform method that calls the specific HitIdMethod for this HitIdTransformer
		/// </summary>
		/// <param name="wellSignal">A double representing a screening value to call as a hit or non-hit</param>
		/// <returns>True if the input is a hit, false if it is not</returns>
		protected override bool Transform(double wellSignal) {
            return this.HitIdMethod(wellSignal);
        }
		#endregion methods
	}
}
