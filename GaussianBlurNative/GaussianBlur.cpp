#include "pch.h"
#include <opencv2/opencv.hpp>

extern "C" __declspec(dllexport)
void ApplyGaussianBlurNative(const unsigned char* inputData, int length, unsigned char* outputData, int width, int height, int channels)
{
    //cv::setNumThreads(0);
    cv::Mat inputImage(height, width, (channels == 3) ? CV_8UC3 : CV_8UC1, (void*)inputData);
    cv::Mat outputImage;

    cv::GaussianBlur(inputImage, outputImage, cv::Size(9, 9), 2.0);

    std::memcpy(outputData, outputImage.data, length);
}
