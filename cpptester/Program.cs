// See https://aka.ms/new-console-template for more information
using System.Diagnostics.Metrics;
using System.Drawing;
using GaussianBlurCore;

    // 1️⃣ Betöltjük a teszt képet
    string inputPath = @"C:\Users\mohac\Downloads\TTR_2572-Enhanced-NR-Vacsora_Lakodalom.jpg"; // legyen a projekt mappában
    if (!File.Exists(inputPath))
    {
        Console.WriteLine($"Hiba: {inputPath} nem található!");
        return;
    }
    Bitmap bmp = new Bitmap(inputPath);
    byte[] inputBytes = null;
    // Lock the bitmap's bits to access raw bytes
    Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
    System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
    try
    {
        int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
        inputBytes = new byte[bytes];
        System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, inputBytes, 0, bytes);
    }
    finally
    {
        bmp.UnlockBits(bmpData);
    }
    // 2️⃣ Példányosítjuk a C++/CLI processzort
    var processor = new GaussianBlurCoreProcessor();

    // 3️⃣ Meghívjuk a feldolgozó függvényt
    int width = bmp.Width; // a kép szélessége, vagy olvasd ki System.Drawing.Bitmap-ből
    int height = bmp.Height; // magasság
    int channels = 3; // RGB
DateTime start = DateTime.Now;
byte[] resultBytes = processor.ApplyGaussianBlur(inputBytes, width, height, channels);
bmp.Save("original.png");
DateTime end = DateTime.Now;
Console.WriteLine($"Feldolgozási idő: {(end - start).TotalMilliseconds} ms");

// Convert byte array (raw RGB) to Bitmap
Bitmap resultbmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
Rectangle resultRect = new Rectangle(0, 0, width, height);
System.Drawing.Imaging.BitmapData resultData = resultbmp.LockBits(resultRect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
try
{
    System.Runtime.InteropServices.Marshal.Copy(resultBytes, 0, resultData.Scan0, resultBytes.Length);
}
finally
{
    resultbmp.UnlockBits(resultData);
}
resultbmp.Save("blurred.jpg");


    
    File.WriteAllBytes("blurredorig.png", inputBytes);
    Console.WriteLine("Feldolgozás kész! Létrejött: blurred.png");
