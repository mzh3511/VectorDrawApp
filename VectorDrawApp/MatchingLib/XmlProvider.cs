using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VectorDraw.Professional.vdObjects;

namespace VectorDrawApp.MatchingLib
{
    public class XmlProvider
    {
        /// <summary>
        /// 构建Xml
        /// </summary>
        /// <param name="matchItems"></param>
        /// <returns></returns>
        public string BuildXml(List<MatchItem> matchItems)
        {
            if (matchItems == null || matchItems.Count == 0)
                return string.Empty;

            var output = new StringBuilder();
            try
            {
                var writer = CreateXmlWriter(output);

                //写xml文件开始<?xml version="1.0" encoding="utf-8" ?>
                writer.WriteStartDocument(false);
                //写根节点
                writer.WriteStartElement("Root");
                foreach (var matchItem in matchItems)
                {
                    matchItem.WriteXml(writer);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Dispose();
            }
            catch (XmlException ex)
            {
                output.AppendLine(ex.Message);
                output.AppendLine(ex.StackTrace);
            }
            return output.ToString();
        }

        /// <summary>
        /// 创建XmlWriter
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private XmlWriter CreateXmlWriter(StringBuilder output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            var settings = new XmlWriterSettings
            {
                Indent = true, //要求缩进
                               //注意如果不设置encoding默认将输出utf-16
                               //注意这儿不能直接用Encoding.UTF8如果用Encoding.UTF8将在输出文本的最前面添加4个字节的非xml内容
                Encoding = new UTF8Encoding(false),
                //设置换行符
                NewLineChars = Environment.NewLine
            };
            return XmlWriter.Create(output, settings);
        }

        /// <summary>
        /// 解析Xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public List<MatchItem> ParseXml(string xml, vdDocument document)
        {
            var result = new List<MatchItem>();
            if (string.IsNullOrEmpty(xml))
                return result;
            try
            {
                var setting = new XmlReaderSettings { IgnoreWhitespace = true };
                var reader = XmlReader.Create(new StringReader(xml), setting);
                while (reader.Read())
                {
                    //如果不是Field节点
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == nameof(MatchItem))
                    {
                        var matchItem = new MatchItem();
                        matchItem.ReadXml(reader, document);
                        result.Add(matchItem);
                    }
                }
                reader.Dispose();
            }
            catch (XmlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
    }
}