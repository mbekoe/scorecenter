using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MediaPortal.Plugin.ScoreCenter.Editor;
using System.Windows.Forms;
using MediaPortal.Plugin.ScoreCenter.Parser;
using System.Collections;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class ScoreFactory
    {
        private static ScoreFactory m_instance = new ScoreFactory();

        private Dictionary<string, Type> m_scoreTypes = new Dictionary<string, Type>();

        /// <summary>
        /// Factory Constructor.
        /// </summary>
        private ScoreFactory()
        {
            List<Type> types = new List<Type>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetExportedTypes())
            {
                if (type.BaseType != null && type.IsSubclassOf(typeof(BaseScore)))
                {
                    string name = type.Name;
                    if (name.EndsWith("Score")) name = name.Substring(0, name.Length - 5);

                    m_scoreTypes.Add(name, type);
                }
            }
        }

        public static ScoreFactory Instance
        {
            get { return m_instance; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public BaseScore CreateScore(string typeName)
        {
            if (!m_scoreTypes.ContainsKey(typeName))
                throw new NullReferenceException();

            Type scoreType = m_scoreTypes[typeName];
            return CreateScore(scoreType);
        }

        public BaseScore CreateScore(Type scoreType)
        {
            BaseScore score = (BaseScore)Activator.CreateInstance(scoreType);
            score.Id = Tools.GenerateId();

            return score;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetScoreTypes()
        {
            return m_scoreTypes.Keys.ToList();
        }

        public string GetEditorType(BaseScore score)
        {
            return score.GetType().Name + "Editor";
        }
        public BaseScoreEditor CreateEditor(string editorType, Panel testPanel)
        {
            var t = Type.GetType("MediaPortal.Plugin.ScoreCenter.Editor." + editorType);
            if (t == null) t = typeof(FolderScoreEditor);
            
            var editor = (BaseScoreEditor)Activator.CreateInstance(t);
            editor.TestPanel = testPanel;
            return editor;
        }

        public string GetParserType(BaseScore score)
        {
            return score.GetType().Name + "Parser";
        }
        private Dictionary<string, IScoreParser> m_parsers = new Dictionary<string, IScoreParser>();
        public IScoreParser GetParser(BaseScore score)
        {
            string parserType = GetParserType(score);
            if (!m_parsers.ContainsKey(parserType))
            {
                var t = Type.GetType("MediaPortal.Plugin.ScoreCenter.Parser." + parserType);
                m_parsers[parserType] = (IScoreParser)Activator.CreateInstance(t, 0);
            }
            return m_parsers[parserType];
        }

        public void ClearCache()
        {
            foreach (var parser in m_parsers.Values)
            {
                parser.ClearCache();
            }
        }

        public static string[][] Parse(BaseScore score, bool reload, ScoreParameter[] parameters)
        {
            return Instance.GetParser(score).Read(score, reload, parameters);
        }
        /*
        public static IList<T> Build<T>(BaseScore score, string[][] labels, int startLine, int startColumn,
            int startX, int startY, int pnlWidth, int pnlHeight,
            CreateControlDelegate<T> createControl,
            out bool overRight, out bool overDown, out int lineNumber, out int colNumber)
        {
            overRight = false;
            overDown = false;
            lineNumber = -1;
            colNumber = -1;

            return Instance.GetBuilder<T>(score).Build(score, labels, startLine, startColumn, startX, startY, pnlWidth, pnlHeight,
                createControl, out overRight, out overDown, out lineNumber, out colNumber);
        }*/

        private Dictionary<string, IScoreBuilder> m_builders = new Dictionary<string, IScoreBuilder>();

        public IScoreBuilder<T> GetBuilder<T>(BaseScore score)
        {
            string builderType = score.GetType().Name + "Builder";

            Type bt = Type.GetType("MediaPortal.Plugin.ScoreCenter." + builderType + "`1");
            Type gbt = bt.MakeGenericType(typeof(T));
            string name = gbt.Name;

            if (!m_builders.ContainsKey(name))
            {
                m_builders[name] = (IScoreBuilder<T>)Activator.CreateInstance(gbt);
            }

            //return (IScoreBuilder<T>)Activator.CreateInstance(gbt);
            return (IScoreBuilder<T>)m_builders[name];
        }
    }
}
