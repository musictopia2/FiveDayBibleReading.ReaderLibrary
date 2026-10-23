namespace FiveDayBibleReading.ReaderLibrary.Core.Services;
public interface IBibleStorage
{
    Task<ReadingProgressModel?> LoadAsync();
    Task SaveAsync(ReadingProgressModel progress);
}