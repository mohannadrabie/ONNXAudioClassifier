using ONNXAudioClassifier.AudioProcessor;
using NAudio.Wave;

namespace ONNXAudioClassifier.Core.Base
{
    // Windows specific base, assuming Windows is the local Dev environment 
    public class AudioDataAvailableEventArgs : EventArgs
    {
        public byte[] AudioBuffer { get; } // 
        public float[] AudioData { get; } // 
        public int ByteCount { get; set; }
        public double Timestamp { get; }

        public AudioDataAvailableEventArgs(byte[] audioBuffer, float[] audioData, int byteCount, double timestamp)
        {
            AudioData = audioData;
            AudioBuffer = audioBuffer;
            ByteCount = byteCount;
            Timestamp = timestamp;
        }
    }
    public class BaseAudioService : IAudioService
    {
        protected readonly IAudioProcessor audioProcessor = new AudioProcessor.AudioProcessor();
        protected WaveInEvent? waveIn;
        protected WaveFileWriter? waveFileWriter;
        protected readonly int sampleRate = 16000; // Yamnet model requires 16kHz sample rate
        protected readonly int channels = 1; // Mono channel
        public event EventHandler<AudioDataAvailableEventArgs>? AudioDataAvailable = delegate { };

        public virtual async Task StartRecordingAsync()
        {
            await Task.Run(() =>
            {
                waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(sampleRate, channels)
                };

                waveFileWriter = new WaveFileWriter("recorded_audio.wav", waveIn.WaveFormat);

                waveIn.DataAvailable += OnDataAvailable;
                waveIn.StartRecording();
            });
        }

        public virtual async Task StopRecordingAsync()
        {
            await Task.Run(() =>
            {
                if (waveIn != null)
                {
                    waveIn.StopRecording();
                    waveIn.DataAvailable -= OnDataAvailable;
                    waveIn.Dispose();
                    waveIn = null;
                    waveFileWriter?.Dispose();
                    waveFileWriter = null;
                }
            });
        }

        protected virtual void OnDataAvailable(object? sender, WaveInEventArgs e)
        {
            var audioData = audioProcessor.ConvertToFloatArray(e.Buffer, e.BytesRecorded); // needed for audio processing
            var eventArgs = new AudioDataAvailableEventArgs(
                e.Buffer, // needed for writing to WAV file & logging audio data however, this is redundant if you are not writing to WAV file or logging audio data
                audioData,
                e.BytesRecorded,
                DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds);
            AudioDataAvailable?.Invoke(this, eventArgs);
        }
    }
}
