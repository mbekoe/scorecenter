#region Copyright (C) 2005-2011 Team MediaPortal

/* 
 *      Copyright (C) 2005-2011 Team MediaPortal
 *      http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

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
        public Editor.BaseScoreEditor CreateEditor(string editorType, Panel testPanel)
        {
            var t = Type.GetType("MediaPortal.Plugin.ScoreCenter.Editor." + editorType);
            if (t == null) t = typeof(Editor.FolderScoreEditor);
            
            var editor = (Editor.BaseScoreEditor)Activator.CreateInstance(t);
            editor.TestPanel = testPanel;
            return editor;
        }

        public string GetParserType(BaseScore score)
        {
            return score.GetType().Name + "Parser";
        }
        private Dictionary<string, Parser.IScoreParser> m_parsers = new Dictionary<string, Parser.IScoreParser>();
        public Parser.IScoreParser GetParser(BaseScore score)
        {
            string parserType = GetParserType(score);
            if (!m_parsers.ContainsKey(parserType))
            {
                var t = Type.GetType("MediaPortal.Plugin.ScoreCenter.Parser." + parserType);
                m_parsers[parserType] = (Parser.IScoreParser)Activator.CreateInstance(t, 0);
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
