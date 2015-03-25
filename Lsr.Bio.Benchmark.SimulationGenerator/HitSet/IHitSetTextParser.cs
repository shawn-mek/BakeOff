using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.Benchmark.ScreenSimulator.HitSet {
    /// <summary>
    /// Interface defining the method(s) for loading hit set info from a text source
    /// </summary>
    public interface IHitSetTextParser {
        List<HitSetInfo> LoadHitSetInfo(TextReader reader);
    } //end IHitSetTextParser
}
