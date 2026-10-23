using FiveDayBibleReading.CoreLibrary.Extensions;
using FiveDayBibleReading.CoreLibrary.Services;

namespace FiveDayBibleReading.ReaderLibrary.Core.Services;
public class BibleReaderService(
    IBibleCategoryService category,
    IBibleStorage storage,
    IDateOnlyPicker datePicker)
{
    private ReadingProgressModel? _progress;

    public DateOnly ReadDate => GetProgress().ReadDate;

    public ChapterReadingModel? CurrentChapter =>
        GetProgress().Readings.FirstOrDefault(x => x.Completed == false);

    public bool IsReadingDue =>
        ReadDate <= datePicker.GetCurrentDate;

    public async Task LoadAsync()
    {
        DateOnly today = datePicker.GetCurrentDate;
        ReadingProgressModel? progress = await storage.LoadAsync();

        bool hasCurrentReading = today.WeekdayOfYear > 0;

        if (progress is null)
        {
            DateOnly readingDate = hasCurrentReading
                ? today
                : today.NextFiveDayReadingDate();

            progress = await CreateProgressAsync(readingDate);
            await storage.SaveAsync(progress);
        }
        else if (hasCurrentReading && progress.ReadDate < today)
        {
            progress = await CreateProgressAsync(today);
            await storage.SaveAsync(progress);
        }
        else if (progress.Completed)
        {
            DateOnly nextDate = progress.ReadDate.NextFiveDayReadingDate();

            progress = await CreateProgressAsync(nextDate);
            await storage.SaveAsync(progress);
        }

        _progress = progress;
    }

    //public async Task<BasicList<string>> GetCurrentChapterVersesAsync()
    //{
    //    ChapterReadingModel? chapter = CurrentChapter;

    //    if (chapter is null || IsReadingDue == false)
    //    {
    //        return [];
    //    }

    //    return await content.GetBookChaptersDataAsync(
    //        chapter.Book,
    //        chapter.Chapter,
    //        chapter.Chapter);
    //}

    public async Task CompleteCurrentChapterAsync()
    {
        ReadingProgressModel progress = GetProgress();

        if (IsReadingDue == false)
        {
            throw new InvalidOperationException(
                "The reading assignment is scheduled for a future date.");
        }

        ChapterReadingModel chapter = CurrentChapter
            ?? throw new InvalidOperationException(
                "There is no unfinished chapter.");

        chapter.Completed = true;

        try
        {
            await storage.SaveAsync(progress);
        }
        catch
        {
            chapter.Completed = false;
            throw;
        }

        if (progress.Completed)
        {
            await LoadAsync();
        }
    }

    private async Task<ReadingProgressModel> CreateProgressAsync(
        DateOnly readingDate)
    {
        var assignments = await category.GetReadingListAsync(readingDate);

        if (assignments.Count == 0)
        {
            throw new InvalidOperationException(
                $"No readings were returned for {readingDate:yyyy-MM-dd}.");
        }

        ReadingProgressModel progress = new()
        {
            ReadDate = readingDate
        };

        foreach (var assignment in assignments)
        {
            progress.Readings.Add(new ChapterReadingModel
            {
                Book = assignment.Book,
                Chapter = assignment.Chapter,
                Completed = false
            });
        }

        return progress;
    }
    public bool HasMoreChapters =>
    GetProgress().Readings.Count(x => x.Completed == false) > 1;
    private ReadingProgressModel GetProgress()
    {
        return _progress
            ?? throw new InvalidOperationException(
                "Call LoadAsync before accessing reading progress.");
    }
}