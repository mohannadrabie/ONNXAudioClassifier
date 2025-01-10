using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONNXAudioClassifier.AudioProcessor
{
    public interface IAudioProcessor
    {
        float[] Amplify(byte[] buffer, int bytesRecorded, float amplificationFactor);
        float[] ConvertToFloatArray(byte[] buffer, int bytesRecorded);

    }
}
