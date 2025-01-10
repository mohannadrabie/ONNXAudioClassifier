namespace ONNXAudioClassifier.AudioProcessor
{
    public class AudioProcessor : IAudioProcessor
    {
        public float[] Amplify(byte[] buffer, int bytesRecorded, float amplificationFactor)
        {
            throw new System.NotImplementedException();
        }

        public float[] ConvertToFloatArray(byte[] buffer, int bytesRecorded)
        {
            float[] audioBuffer = new float[bytesRecorded / 2];
            for (int i = 0; i < bytesRecorded; i += 2)
            {
                audioBuffer[i / 2] = BitConverter.ToInt16(buffer, i) / 32768f;
            }
            return audioBuffer;
        }

    }

}