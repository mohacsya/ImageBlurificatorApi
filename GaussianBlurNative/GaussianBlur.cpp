#include "pch.h"
#include <opencv2/opencv.hpp>
#include <fstream>

extern "C" __declspec(dllexport)
void ApplyGaussianBlurNative(const unsigned char* inputData, int length, unsigned char* outputData, int width, int height, int channels)
{
    //std::cout << cv::getBuildInformation() << std::endl;
	//std::cout << "CPU number " << (cv::getNumberOfCPUs()) << std::endl;
    //cv::setNumThreads(cv::getNumberOfCPUs());
    cv::setNumThreads(0);
    std::cout << "CPU number " << (cv::getNumThreads()) << std::endl;

    cv::Mat inputImage(height, width, (channels == 3) ? CV_8UC3 : CV_8UC1, (void*)inputData);
    cv::Mat outputImage;
    // Start timing
    auto start = std::chrono::high_resolution_clock::now();
    cv::GaussianBlur(inputImage, outputImage, cv::Size(9, 9), 2.0);
    // End timing
    auto end = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();
    std::cout << "GaussianBlur duration: " << duration << " ms" << std::endl;

    std::memcpy(outputData, outputImage.data, length);
}
