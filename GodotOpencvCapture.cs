using Godot;
using System;

using OpenCvSharp;
using OpenCvSharp.Extensions;

using System.Drawing.Imaging;
using System.IO;
using System.Drawing;

public class Control : Godot.Control
{
    VideoCapture capture;
    Mat frame;
    Bitmap image;

    public void ini()
    {
        frame = new Mat();
        capture = new VideoCapture(0);
        capture.Open(0);
    }
    public void capture()
    {
        if (capture.IsOpened())
        {
            capture.Read(frame);
            image = BitmapConverter.ToBitmap(frame);

            Godot.Image testImage = new Godot.Image();
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                testImage.LoadPngFromBuffer(ms.ToArray());
            }
            TextureRect textRect = (TextureRect)this.GetNode("TextureRect");
            textRect.SetSize(new Vector2(image.Width, image.Height));

            ImageTexture imageTex = new ImageTexture();
            imageTex.CreateFromImage(testImage);

            textRect.SetTexture(imageTex);
        }
    }
    public override void _Ready()
    {
        ini();   
    }
    public override void _Process(float delta)
    {
        capture();
    }
}