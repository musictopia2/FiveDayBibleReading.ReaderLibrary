namespace FiveDayBibleReading.ReaderLibrary.Blazor.Storage;
public class LocalStorageBibleStorage(IJSRuntime js) : IBibleStorage
{
    private readonly string _key = "bibleschedule";
    async Task<ReadingProgressModel?> IBibleStorage.LoadAsync()
    {
        if (await js.ContainsKeyAsync(_key) == false)
        {
            return null;
        }
        var output = await js.StorageGetItemAsync<ReadingProgressModel>(_key);
        return output;
    }
    async Task IBibleStorage.SaveAsync(ReadingProgressModel progress)
    {
        await js.StorageSetItemAsync(_key, progress);
    }
}