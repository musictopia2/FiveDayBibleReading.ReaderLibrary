namespace FiveDayBibleReading.ReaderLibrary.Core.Services;
public class BibleFileStorage(IReadingPath path) : IBibleStorage
{
    async Task<ReadingProgressModel?> IBibleStorage.LoadAsync()
    {
        if (ff1.FileExists(path.ReadingPath) == false)
        {
            return null;
        }
        var output = await ff2.RetrieveSavedObjectAsync<ReadingProgressModel>(path.ReadingPath);
        return output;
    }

    async Task IBibleStorage.SaveAsync(ReadingProgressModel progress)
    {
        await ff2.SaveObjectAsync(path.ReadingPath, progress);
    }
}