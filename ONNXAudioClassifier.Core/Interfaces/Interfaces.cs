using ONNXAudioClassifier.Core.Base;

namespace ONNXAudioClassifier.Core
{
    public interface IAudioService
    {
        Task StartRecordingAsync();
        Task StopRecordingAsync();
        event EventHandler<AudioDataAvailableEventArgs> AudioDataAvailable;
  
    }

 
    public interface IFileWriter
    {
        string RecordingsDirectory { get; }

        void LogMessage(string logFilePath, string message);
    }

}