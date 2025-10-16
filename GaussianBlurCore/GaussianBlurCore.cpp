#include "pch.h"

#include "GaussianBlurCore.h"
#include "GaussianBlur.h"

extern "C" __declspec(dllimport)
void ApplyGaussianBlurNative(const unsigned char* inputData, int length, unsigned char* outputData, int width, int height, int channels);

using namespace GaussianBlurCore;

array<Byte>^ GaussianBlurCoreProcessor::ApplyGaussianBlur(array<Byte>^ input, int width, int height, int channels)
{
    int length = input->Length;
    array<Byte>^ output = gcnew array<Byte>(length);

    Console::WriteLine("Length: {0}", length);
    Console::WriteLine("Width: {0}", width);
    Console::WriteLine("Height: {0}", height);

	Console::WriteLine("Raw input: {0}", input[0].ToString());
    // A .NET memóriát rögzítjük, hogy C++ pointert adhassunk neki
    pin_ptr<Byte> pInput = &input[0];
    Console::WriteLine("casted input: {0}", pInput[0]);
    unsigned char* inputCharPtr = reinterpret_cast<unsigned char*>(pInput);
    //pin_ptr<Byte> pInput = gcnew Byte();
    pin_ptr<Byte> pOutput = &output[0];
    unsigned char* inputData = new unsigned char[length](); // Dynamically allocate memory for input buffer
    for (size_t i = 0; i < length; i++)
    {
        inputData[i] = input[i];
    }
    Console::WriteLine("casted input2: {0}", inputData[0]);
    // Example static dummy data
    
    //unsigned char outputData[100] = { 0 }; // Example output buffer of 100 bytes
    //length = 50; // Total number of bytes in input/output
    //width = 10;   // Example image width
    //height = 10;  // Example image height
    //channels = 1; // Example: grayscale image
    // Meghívjuk a natív függvényt
    ApplyGaussianBlurNative(inputData, length, pOutput, width, height, channels);

    return output;
}