// See https://aka.ms/new-console-template for more information
using System.Diagnostics.Metrics;
using System.Drawing;
using GaussianBlurCore;

string inputPath = @"C:\Users\MOH6BP\Pictures\átszeli.png"; // legyen a projekt mappában
if (!File.Exists(inputPath))
{
    Console.WriteLine($"Hiba: {inputPath} nem található!");
    return;
}
Bitmap bmp = new Bitmap(inputPath);
byte[] inputBytes = null;
// Lock the bitmap's bits to access raw bytes
Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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

var processor = new GaussianBlurCoreProcessor();


int width = bmp.Width;
int height = bmp.Height; 
int channels = 3; // RGB
DateTime start = DateTime.Now;
byte[] resultBytes = processor.ApplyGaussianBlur(inputBytes, width, height, channels);
bmp.Save("original.png");
DateTime end = DateTime.Now;
Console.WriteLine($"Feldolgozási idő: {(end - start).TotalMilliseconds} ms");

// Convert byte array (raw RGB) to Bitmap
Bitmap resultbmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
Rectangle resultRect = new Rectangle(0, 0, width, height);
System.Drawing.Imaging.BitmapData resultData = resultbmp.LockBits(resultRect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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


void ConvertImagesToBase64(string folder, string extension)
{
    string[] files = Directory.GetFiles(folder, $"*{extension}");
    foreach (var file in files)
    {
        Bitmap bmp = new Bitmap(file);
        using (MemoryStream ms = new MemoryStream())
        {
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] imageBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            File.WriteAllText(file + ".b64.txt", base64String);
            Console.WriteLine($"Converted {file} to base64.");
        }
    }
}