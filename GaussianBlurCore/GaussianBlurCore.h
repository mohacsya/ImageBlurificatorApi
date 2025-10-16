#pragma once
using namespace System;

namespace GaussianBlurCore
{
    public ref class GaussianBlurCoreProcessor
    {
    public:
        array<Byte>^ ApplyGaussianBlur(array<Byte>^ input, int width, int height, int channels);
    };
}