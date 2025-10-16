#pragma once

#ifdef IMAGEPROCESSINGNATIVE_EXPORTS
#define IMAGE_API __declspec(dllexport)
#else
#define IMAGE_API __declspec(dllimport)
#endif

extern "C"
{
    IMAGE_API void ApplyGaussianBlurNative(
        const unsigned char* inputData,
        int length,
        unsigned char* outputData,
        int width,
        int height,
        int channels
    );
}
