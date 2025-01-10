
# ONNXAudioClassifier 

ONNX Audio Classifier is a modular, cross-platform library for real-time audio classification. Built with .NET MAUI and ONNX Runtime, it delivers lightweight inference for a seamless performance across Desktop and Mobile (Android, and iOS).

## Features
- **Cross-Platform Compatibility:** Supports Windows, Android, and iOS via .NET MAUI.
- **High-Performance Inference:** Directly utilizes ONNX runtime for scalable audio classification.
- **Modular Architecture:** Clear separation of Core, AudioProcessor, and CLI components.
- **CLI Support:** Lightweight command-line interface for rapid deployment and testing.

## Getting Started

### Prerequisites

1. **.NET SDK 9.0 or later**
   - Ensure you have the .NET 9.0 SDK or later installed. [Download it from the .NET download page](https://dotnet.microsoft.com/download).

2. **Visual Studio 2022**
   - Install Visual Studio 2022 with the .NET MAUI workload. 
   - This can be done through the Visual Studio Installer by selecting the ".NET Multi-platform App UI development" workload.

3. **NAudio**
   - [NAudio](https://github.com/naudio/NAudio) is used for audio processing. It will be restored automatically via NuGet when you build the solution.

4. **ONNX Runtime**
   - [ONNX Runtime](https://github.com/microsoft/onnxruntime) is used for machine learning inference. It will also be restored automatically via NuGet when you build the solution.

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/mohannadrabie/ONNXAudioClassifier.git
   cd ONNXAudioClassifier
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   msbuild ONNXAudioClassifier.sln /t:Build /p:Configuration=Release
   ```

## Running the Application

### Visual Studio
1. Open `ONNXAudioClassifier.sln` in Visual Studio.
2. Set the desired startup project (CLI).
3. Run the project.

### Command-Line Interface
1. Build the CLI:
   ```bash
   msbuild ONNXAudioClassifier.CLI/ONNXAudioClassifier.CLI.csproj /t:Build /p:Configuration=Release /p:OutputPath=./ReleaseBuild
   ```
2. Run the CLI:
   ```bash
   dotnet ONNXAudioClassifier.CLI\ReleaseBuild\ONNXAudioClassifier.CLI.dll
   ```

### Cross-Platform
Platform-specific dependencies must be implemented in `Platforms/` for Android and iOS.

## Architecture

### Key Components
1. **Core Logic (`ONNXAudioClassifier.Core`)**
   - Provides interfaces for platform services.
   - Implements reusable audio capture and classification logic.

2. **Audio Processing (`ONNXAudioClassifier.AudioProcessor`)**
   - Includes utilities for audio preprocessing, amplification, and data transformation.

3. **Command-Line Interface (`ONNXAudioClassifier.CLI`)**
   - Lightweight interface for testing and interacting with the core functionalities.

## ONNX.runtime Integration
- Centralized inference framework for ONNX models.
- Enables portable, efficient, and low-latency model execution across platforms.

## Future Enhancements
- Complete Android/iOS support for platform-specific recording.
- Implement audio preprocessing for better model accuracy.
- Add support for model fine-tuning and training.

## License
This project is licensed under the MIT License. See `LICENSE.txt` for details.
