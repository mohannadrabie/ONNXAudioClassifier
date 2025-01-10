
using ONNXAudioClassifier.Core.Base;
namespace ONNXAudioClassifier.Core.Platforms.iOS
{
    public class AudioService : IAudioService
    {
        public event EventHandler<AudioDataAvailableEventArgs> AudioDataAvailable = delegate { };
        public Task StartRecordingAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopRecordingAsync()
        {
            throw new NotImplementedException();
        }
    }
}
