using ONNXAudioClassifier.Core;
using ONNXAudioClassifier.Core.Base;
using ONNXAudioClassifier.AudioProcessor;

namespace ONNXAudioClassifier.CLI
{
    class Program
    {
        private static IAudioService? _audioService;
        private static int _totalSamplesProcessed = 0;
        private const int SampleRate = 16000; // Yamnet model requires 16kHz sample rate
        private static MemoryStream audioBufferStream = new MemoryStream(); // Buffer for audio data to be saved to a WAV file

        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()

                .AddSingleton<IFileWriter, FileWriter>() 
                .AddSingleton<IAudioService>(provider => new BaseAudioService())
                .BuildServiceProvider();
            _audioService = serviceProvider.GetService<IAudioService>() ?? throw new InvalidOperationException("Failed to get IAudioCaptureService from service provider.");
            _audioService.AudioDataAvailable += async (sender, e) => await OnAudioDataAvailableAsync(sender!, e);
            Console.WriteLine("Press 'r' to start recording, 's' to stop recording, and 'q' to quit.");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    await StartRecordingAsync();
                }
                else if (key == ConsoleKey.S)
                {
                    await StopRecordingAsync();
                }
                else if (key == ConsoleKey.Q)
                {
                    break;
                }
            }
        }

        private static async Task StartRecordingAsync()
        {
            try
            {
                if (_audioService == null)
                {
                    throw new InvalidOperationException("Audio service is not initialized.");
                }
                _totalSamplesProcessed = 0;
                await _audioService.StartRecordingAsync();
                Console.WriteLine("Recording started...");
            }
            catch (Exception ex)
            {
                await Task.Run(() => Console.Write(ex.InnerException));
            }
        }

        private static async Task StopRecordingAsync()
        {
            try
            {
                if (_audioService == null)
                {
                    throw new InvalidOperationException("Audio capture service is not initialized.");
                }
                
                await _audioService.StopRecordingAsync();
                Console.WriteLine("Recording stopped.");
                string audioFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Consolerecorded_audio.wav");
                SaveAudioBufferToWav(audioFilePath, SampleRate);
               
            }
            catch (Exception ex)
            {
                await Task.Run(() => Console.Write(ex.InnerException));
            }
        }

        private static void SaveAudioBufferToWav(string filePath, int sampleRate)
        {
            audioBufferStream.Seek(0, SeekOrigin.Begin);
            using var waveFileWriter = new NAudio.Wave.WaveFileWriter(filePath, new NAudio.Wave.WaveFormat(sampleRate, 16, 1));
            waveFileWriter.Write(audioBufferStream.ToArray(), 0, (int)audioBufferStream.Length);
        }

        private static async Task OnAudioDataAvailableAsync(object sender, AudioDataAvailableEventArgs e)
        {
            // useful for debugging
            int currentSecond = _totalSamplesProcessed / SampleRate;
            audioBufferStream.Write(e.AudioBuffer, 0, e.ByteCount);

            try
            {
                var labels = await AudioClassifier.ProcessAudioDataAsync(e.AudioData);

                // Print the classification labels to the console
                Console.WriteLine($"Second {currentSecond}: {string.Join(", ", labels)}");

                // Increment the total samples processed
                _totalSamplesProcessed += e.AudioData.Length;
            }
            catch (Exception ex)
            {
                await Task.Run(() => Console.Write(ex.InnerException));
            }
        }
    }
}

