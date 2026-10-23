namespace FiveDayBibleReading.ReaderLibrary.Core.Models;
public class ReadingProgressModel
{
    public DateOnly ReadDate { get; set; }
    public BasicList<ChapterReadingModel> Readings { get; set; } = [];
    public bool Completed => Readings.Count > 0
        && Readings.All(x => x.Completed);
}