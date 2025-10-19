#include "pch.h"

#include "GaussianBlurCore.h"
#include "GaussianBlur.h"

extern "C" __declspec(dllimport)
void ApplyGaussianBlurNative(const unsigned char* inputData, int length, unsigned char* outputData, int width, int height, int channels);

using namespace GaussianBlurCore;

/// <summary>
/// Applies a Gaussian blur to the input image data using a native implementation.
/// </summary>
/// <param name="input">The input image as a managed byte array (must not be null).</param>
/// <param name="width">Image width in pixels.</param>
/// <param name="height">Image height in pixels.</param>
/// <param name="channels">Number of color channels (e.g., 3 for RGB, 4 for RGBA).</param>
/// <returns>A new managed byte array containing the blurred image data.</returns>
array<Byte>^ GaussianBlurCoreProcessor::ApplyGaussianBlur(array<Byte>^ input, int width, int height, int channels)
{
    if (input == nullptr)
        throw gcnew ArgumentNullException("input");
    if (width <= 0 || height <= 0 || channels <= 0)
        throw gcnew ArgumentException("Width, height, and channels must be positive integers.");
    if (input->Length != width * height * channels)
        throw gcnew ArgumentException("Input array size does not match width * height * channels.");

    int length = input->Length;
    array<Byte>^ output = gcnew array<Byte>(length);

    // Pin managed arrays to obtain stable pointers for native interop
    pin_ptr<Byte> pInput = &input[0];
    pin_ptr<Byte> pOutput = &output[0];

    // Call the native Gaussian blur implementation
    ApplyGaussianBlurNative(
        reinterpret_cast<const unsigned char*>(pInput),
        length,
        reinterpret_cast<unsigned char*>(pOutput),
        width, height, channels
    );

    return output;
}