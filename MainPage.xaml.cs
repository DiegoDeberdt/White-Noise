using Microsoft.Graphics.Canvas;
using System;
using System.Runtime.InteropServices;
using System.Timers;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WhiteNoise
{
    [ComImport]
    [Guid("5b0d3235-4dba-4d44-865e-8f1d0e4fd04d")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    unsafe interface IMemoryBufferByteAccess
    {
        void GetBuffer(out byte* buffer, out uint capacity);
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private static System.Timers.Timer aTimer;

        private double canvasWidth;
        private double canvasHeight;

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            DrawingCanvas.Invalidate();
        }

        DateTime _lastTime; // marks the beginning the measurement began
        int _framesRendered; // an increasing count
        int _fps; // the FPS calculated from the last measurement

        unsafe private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            var r = new Random();

            var softwareBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, (int)Math.Floor(canvasWidth), (int)Math.Floor(canvasHeight), BitmapAlphaMode.Ignore);

            using (BitmapBuffer buffer = softwareBitmap.LockBuffer(BitmapBufferAccessMode.Write))
            {
                using (var reference = buffer.CreateReference())
                {
                    byte* dataInBytes;
                    uint capacity;
                    ((IMemoryBufferByteAccess)reference).GetBuffer(out dataInBytes, out capacity);

                    // Fill-in the BGRA plane
                    BitmapPlaneDescription bufferLayout = buffer.GetPlaneDescription(0);
                    for (int i = 0; i < bufferLayout.Width; i++)
                    {
                        for (int j = 0; j < bufferLayout.Height; j++)
                        {
                            byte value = (r.Next(2) != 0) ? (byte)0 : (byte)255;
                            if (value == 0) continue;

                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * j + 4 * i + 0] = value;
                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * j + 4 * i + 1] = value;
                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * j + 4 * i + 2] = value;
                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * j + 4 * i + 3] = (byte)255;
                        }
                    }
                }
            }

            var canvasBitMap = CanvasBitmap.CreateFromSoftwareBitmap(sender, softwareBitmap);
            args.DrawingSession.DrawImage(canvasBitMap, new Rect() { Height = canvasHeight, Width = canvasWidth });

            _framesRendered++;

            if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
            {
                // one second has elapsed 

                _fps = _framesRendered;
                _framesRendered = 0;
                _lastTime = DateTime.Now;
            }

            if (FPS.Text != _fps.ToString()) FPS.Text = _fps.ToString();
        }

        private void DrawingCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            canvasHeight = DrawingCanvas.ActualHeight;
            canvasWidth = DrawingCanvas.ActualWidth;

            aTimer = new System.Timers.Timer(10);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
    }
}
