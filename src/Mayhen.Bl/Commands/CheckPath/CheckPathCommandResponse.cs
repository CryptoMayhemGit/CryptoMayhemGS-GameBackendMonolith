using System.Collections.Generic;

namespace Mayhen.Bl.Commands.CheckPath
{
    public class CheckPathCommandResponse
    {
        public bool PathPossible { get; set; }
        public int Time { get; set; }
        public IEnumerable<long> Lands { get; set; }
    }
}
