using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
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
        /// <summary>
        /// 获取设备句柄
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        /// <summary>
        /// 释放函数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hdc"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var renderGraphic = e.Graphics;
            var renderHdc = IntPtr.Zero;
            var renderFormHandle = IntPtr.Zero;
            var renderProcessArr = Process.GetProcessesByName("RenderApp");
            if (renderProcessArr.Length > 0)
            {
                //跨进程渲染，如果RenderApp.exe已启动则在该进程的主界面上进行绘制
                renderFormHandle = renderProcessArr[0].MainWindowHandle;
                renderHdc = GetDC(renderFormHandle);
                renderGraphic = Graphics.FromHdc(renderHdc);
            }

            renderGraphic.Clear(Color.CadetBlue);

            var currentContext = BufferedGraphicsManager.Current;
            var buffer = currentContext.Allocate(renderGraphic, ClientRectangle);
            var g = buffer.Graphics;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;


            var layout = Document.ActiveLayOut;
            using (var bitmap = new Bitmap(Width, Height))
            {
                using (var bitmapGraphic = Graphics.FromImage(bitmap))
                {
                    layout.RenderToGraphics(bitmapGraphic, RenderingArea, Width, Height);
                }
                g.DrawImage(bitmap,0,0);
            }
            

            //layout.Printer.InitializeProperties();
            //layout.Printer.Resolution = (int)renderGraphic.DpiX; //Screen DPI
            //layout.Printer.paperSize = new Rectangle(0, 0, Width * 100 / layout.Printer.Resolution, Width * 100 / layout.Printer.Resolution);
            //layout.Printer.PrintWindow = RenderingArea;
            //layout.Printer.RenderToGraphics(g, Color.White);

            //把缓冲区中的内容一次性写入到界面
            buffer.Render(renderGraphic);
            g.Dispose();
            buffer.Dispose();//释放资源

            if (renderHdc != IntPtr.Zero)
                ReleaseDC(renderFormHandle, renderHdc);
        }
    }
}
