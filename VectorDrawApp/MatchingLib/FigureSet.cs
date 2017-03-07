using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Xml;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDrawApp.MatchingLib.Comparers;
using VectorDrawApp.VdUtils;

namespace VectorDrawApp.MatchingLib
{
    [DebuggerDisplay("Count={_entities.Count}")]
    public class FigureSet
    {
        private Box _boundingBox = new Box();
        private readonly List<vdFigure> _entities = new List<vdFigure>();

        public IList<vdFigure> Entities { get; private set; }
        public vdFigure Major { get; private set; }

        public FigureSet(List<vdFigure> items = null)
        {
            if (items == null)
                return;
            _entities.AddRange(items);
            _entities.Sort(new FigureMidPointComparer());
            Entities = new ReadOnlyCollection<vdFigure>(_entities);

            var list4Major = new List<vdFigure>(_entities);
            list4Major.Sort(new MajorFigureComparer());
            Major = list4Major[0];
        }

        public FigureSet(List<vdFigure> items, vdFigure major)
        {
            _entities.AddRange(items);
            _entities.Sort(new FigureMidPointComparer());
            Entities = new ReadOnlyCollection<vdFigure>(_entities);

            Major = major;
        }

        public Box GetBoundingBox()
        {
            if (_boundingBox.IsEmpty && _entities.Count != 0)
                _boundingBox = VdActionUtil.GetBoundingBox(_entities);
            return _boundingBox;
        }

        public string GetDescription()
        {
            var sb = new StringBuilder();
            foreach (var figure in Entities)
            {
                if (sb.Length > 0)
                    sb.AppendLine();
                sb.Append($"{figure._TypeName}, handle={figure.HandleId}");
            }
            return sb.ToString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(FigureSet));

            writer.WriteStartElement(nameof(Major));
            writer.WriteValue(Major.HandleId.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(Entities));
            foreach (var entity in Entities)
            {
                writer.WriteStartElement("Entity");
                writer.WriteValue(entity.HandleId.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();

        }

        public void ReadXml(XmlReader reader, vdDocument document)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == nameof(Major))
                {
                    var strMajor = reader.ReadString();
                    Major = VdSqlUtil.GetFigureByHandle(document, ulong.Parse(strMajor));
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                {
                    var strEntity = reader.ReadString();
                    var entity = VdSqlUtil.GetFigureByHandle(document, ulong.Parse(strEntity));
                    if (entity == null)
                        throw new XmlException($"找不到HandleId={strEntity}的图元");
                    _entities.Add(entity);
                }
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == nameof(FigureSet))
                {
                    reader.Read();
                    break;
                }
            }
            Entities = new ReadOnlyCollection<vdFigure>(_entities);
        }
    }
}