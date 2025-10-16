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

    // A .NET memóriát rögzítjük, hogy C++ pointert adhassunk neki
    pin_ptr<Byte> pInput = &input[0];
    pin_ptr<Byte> pOutput = &output[0];

    // Meghívjuk a natív függvényt
    ApplyGaussianBlurNative(pInput, length, pOutput, width, height, channels);

    return output;
}