
namespace ONNXAudioClassifier.Core.Base
{ 
    
    public class FileWriter : IFileWriter
    {
        // Windows specific base, assuming Windows is the local Dev environment 
        public string RecordingsDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ONNXAudioClassifierRecording");
        private static readonly object fileLock = new object(); 

        public void LogMessage(string fileName, string message)
        {
            lock (fileLock)
            {
                if (!Directory.Exists(RecordingsDirectory))
                {
                    Directory.CreateDirectory(RecordingsDirectory);
                }
                var logFilePath = Path.Combine(RecordingsDirectory, fileName);

                using (var writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:O}: {message}");
                }
            }
        }
    }
}
