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

    pin_ptr<Byte> pInput = &input[0];
    pin_ptr<Byte> pOutput = &output[0];
    unsigned char* inputData = new unsigned char[length](); // Dynamically allocate memory for input buffer
    for (size_t i = 0; i < length; i++)
    {
        inputData[i] = input[i];
    }
    ApplyGaussianBlurNative(inputData, length, pOutput, width, height, channels);
	delete[] inputData; // Free the dynamically allocated memory
    return output;
}