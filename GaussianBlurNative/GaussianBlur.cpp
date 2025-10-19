#include "pch.h"
#include <opencv2/opencv.hpp>
#include <stdexcept>

/**
 * @brief Applies a Gaussian blur to the input image buffer using OpenCV.
 * 
 * @param inputData   Pointer to the input image data (must not be null).
 * @param length      Total number of bytes in the input and output buffers.
 * @param outputData  Pointer to the output buffer (must not be null, must be at least 'length' bytes).
 * @param width       Image width in pixels.
 * @param height      Image height in pixels.
 * @param channels    Number of channels (3 for RGB, 4 for RGBA).
 * 
 * @note The function expects the input buffer to be tightly packed (no padding).
 * @throws std::invalid_argument if parameters are invalid.
 */
extern "C" __declspec(dllexport)
void ApplyGaussianBlurNative(const unsigned char* inputData, int length, unsigned char* outputData, int width, int height, int channels)
{
    // Validate parameters
    if (!inputData || !outputData)
        throw std::invalid_argument("Input and output buffers must not be null.");
    if (width <= 0 || height <= 0 || (channels != 3 && channels != 4))
        throw std::invalid_argument("Width, height, and channels must be valid. Only 3 (RGB) or 4 (RGBA) channels are supported.");
    if (length != width * height * channels)
        throw std::invalid_argument("Buffer length does not match width * height * channels.");

    cv::setNumThreads(cv::getNumberOfCPUs());

    // Create input image matrix
    int type = (channels == 3) ? CV_8UC3 : CV_8UC4;
    cv::Mat inputImage(height, width, type, const_cast<unsigned char*>(inputData));
    cv::Mat outputImage;

    // Apply Gaussian blur
    cv::GaussianBlur(inputImage, outputImage, cv::Size(9, 9), 2.0);

    // Ensure outputImage is valid before copying
    if (!outputImage.data)
        throw std::runtime_error("GaussianBlur failed to produce output.");

    std::memcpy(outputData, outputImage.data, length);
}
