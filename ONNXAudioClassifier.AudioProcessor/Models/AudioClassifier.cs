using System.Reflection;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace ONNXAudioClassifier.Core
{
    public static class AudioClassifier
    {
        private const int InputSize = 15600; 
        private static InferenceSession OnnxSession; // avoiding ML.NET for performance reasons
        private static List<string> Labels; 

        static AudioClassifier()
        {
            // Get the application's base directory -- NOT platform agnostic
            // TODO: Use a platform-agnostic method to get the base directory
            string baseDirectory = AppContext.BaseDirectory;
            // Build the full path to the ONNX model file -- NOT platform agnostic
            // TODO: Use a platform-agnostic method to get the base directory
            string modelPath = Path.Combine(baseDirectory, "Assets", "yamnet.onnx");
            // TODO: Use a platform-agnostic method to get the base directory
            // Load labels from labels.txt -- NOT platform agnostic
            var labelsPath = Path.Combine(baseDirectory, "Assets", "labels.txt");

            if (File.Exists(labelsPath))
            {
                using (var reader = new StreamReader(labelsPath))
                {
                    Labels = new List<string>();
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Labels.Add(line);
                    }
                }
            }
            else
            {
                throw new FileNotFoundException($"The file {labelsPath} does not exist.");
            }
            OnnxSession = new InferenceSession(modelPath);
        }

        public static async Task<List<string>> ProcessAudioDataAsync(float[] audioData)
        {
            // Adjust the audio data to match the expected input size
            if (audioData.Length > InputSize)
            {
                // Trim the audio data
                audioData = audioData.Take(InputSize).ToArray();
            }
            else if (audioData.Length < InputSize)
            {
                // Pad the audio data with zeros
                float[] paddedAudio = new float[InputSize];
                Array.Copy(audioData, paddedAudio, audioData.Length);
                audioData = paddedAudio;
            }

            // Call ClassifyAudio method
            return await Task.Run(() => ClassifyAudio(audioData));
        }

        private static List<string> ClassifyAudio(float[] audio)
        {
            // Validate audio size
            if (audio.Length != InputSize)
            {
                throw new Exception($"Audio size mismatch. Expected {InputSize}, but got {audio.Length}.");
            }

            // Prepare input tensor
            var tensor = new DenseTensor<float>(audio, new[] { InputSize });

            // Run inference
            var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("waveform_binary", tensor)
                };

            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = OnnxSession.Run(inputs);

            var output = results.First(output => output.Name == "tower0/network/layer32/final_output").AsEnumerable<float>().ToArray();

            var topPredictions = output
                .Select((score, index) => new { Score = score, Index = index })
                .OrderByDescending(x => x.Score)
                .Take(2) // Adjust to the desired number of top predictions, top 2 is good enough for testing  
                .ToList();

            List<string> topLabels = new List<string>();
            foreach (var prediction in topPredictions)
            {
                string labelName = Labels[prediction.Index];
                topLabels.Add($"Label: {labelName}, Score: {prediction.Score:F4}");
            }
            return topLabels;
        }


    }
}
