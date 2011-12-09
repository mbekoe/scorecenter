using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    public class FolderScoreParser : ScoreParser<FolderScore>
    {
        public FolderScoreParser(int lifetime)
            : base(lifetime)
        {

        }

        protected override string[][] Parse(FolderScore score, bool reload, ScoreParameter[] parameters)
        {
            return null;
        }
    }
}
