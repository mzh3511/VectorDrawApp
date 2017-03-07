using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdObjects;

namespace VectorDrawApp
{
    public partial class FormViewer : Form
    {
        public vdDocument Document { get; set; }
        public Box RenderingArea { get; set; }

        public FormViewer()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            //SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            UpdateStyles();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.CadetBlue);

            var currentContext = BufferedGraphicsManager.Current;
            var buffer = currentContext.Allocate(e.Graphics, ClientRectangle);
            var g = buffer.Graphics;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            var layout = Document.ActiveLayOut;
            layout.Printer.InitializeProperties();
            layout.Printer.Resolution = (int)e.Graphics.DpiX; //Screen DPI
            layout.Printer.paperSize = new Rectangle(0, 0, Width * 100 / layout.Printer.Resolution, Width * 100 / layout.Printer.Resolution);
            layout.Printer.PrintWindow = RenderingArea;
            layout.Printer.RenderToGraphics(g, Color.White);

            //把缓冲区中的内容一次性写入到界面
            buffer.Render(e.Graphics);
            g.Dispose();
            buffer.Dispose();//释放资源 
        }
    }
}
