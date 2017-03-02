using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using VectorDraw.Professional.vdObjects;

namespace VectorDrawApp.MatchingLib
{
    /// <summary>
    /// 代表一次图元匹配
    /// </summary>
    public class MatchItem
    {
        public string Name { get; set; } = DateTime.Now.ToShortTimeString();
        public string CadFileName { get; set; }
        /// <summary>
        /// 样本
        /// </summary>
        public FigureSet Sample { get; set; }
        /// <summary>
        /// 匹配结果集
        /// </summary>
        public IList<FigureSet> Results { get; } = new List<FigureSet>();

        public string GetDescription()
        {
            var sb = new StringBuilder();
            sb.Append($"Sample figure count = {Sample.Entities}, major handle={Sample.Major.HandleId}");
            for (int i = 0; i < Results.Count; i++)
            {
                var result = Results[i];
                if (sb.Length > 0)
                    sb.AppendLine();
                sb.Append($"Result[{i}] major handle={result.Major.HandleId}");
            }
            return sb.ToString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(MatchItem));
            writer.WriteAttributeString(nameof(CadFileName), CadFileName);

            writer.WriteStartElement(nameof(Sample));
            Sample.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(Results));
            foreach (var result in Results)
            {
                result.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader, vdDocument document)
        {
            CadFileName = document.FileName;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == nameof(Sample))
                {
                    reader.Read();
                    Sample = new FigureSet();
                    Sample.ReadXml(reader,document);
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == nameof(Results))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == nameof(FigureSet))
                        {
                            var figureSet = new FigureSet();
                            figureSet.ReadXml(reader, document);
                            Results.Add(figureSet);
                        }
                        if (reader.NodeType == XmlNodeType.EndElement && reader.Name == nameof(Results))
                        {
                            reader.Read();
                            break;
                        }
                    }
                }
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == nameof(MatchItem))
                {
                    reader.Read();
                    break;
                }
            }
        }
    }
}