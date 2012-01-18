using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    public interface IScoreEditor2
    {
        void LoadScore(BaseScore baseScore, ScoreCenter center);
        void SaveScore(ref BaseScore baseScore);
        void Clear();
        bool CheckData();
        bool HasTest();
    }
}
