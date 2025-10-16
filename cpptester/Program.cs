// See https://aka.ms/new-console-template for more information
using System.Diagnostics.Metrics;
using System.Drawing;
using GaussianBlurCore;

try
{
    // 1️⃣ Betöltjük a teszt képet
    string inputPath = "C:\\Users\\mohac\\Pictures\\robimagyark\\00025.png"; // legyen a projekt mappában
    if (!File.Exists(inputPath))
    {
        Console.WriteLine($"Hiba: {inputPath} nem található!");
        return;
    }
    Bitmap bmp = new Bitmap(inputPath);
    byte[] inputBytes = File.ReadAllBytes(inputPath);

    // 2️⃣ Példányosítjuk a C++/CLI processzort
    var processor = new GaussianBlurCoreProcessor();

    // 3️⃣ Meghívjuk a feldolgozó függvényt
    int width = bmp.Width; // a kép szélessége, vagy olvasd ki System.Drawing.Bitmap-ből
    int height = bmp.Height; // magasság
    int channels = 3; // RGB

    byte[] resultBytes = processor.ApplyGaussianBlur(inputBytes, width, height, channels);

    // 4️⃣ Visszaírjuk fájlba
    File.WriteAllBytes("blurred.png", resultBytes);

    Console.WriteLine("Feldolgozás kész! Létrejött: blurred.png");
}
catch (Exception ex)
{
    Console.WriteLine("Hiba a teszt futtatása közben: " + ex.Message);
}