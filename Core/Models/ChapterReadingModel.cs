namespace FiveDayBibleReading.ReaderLibrary.Core.Models;
public class ChapterReadingModel
{
    public string Book { get; set; } = "";
    public int Chapter { get; set; }
    public bool Completed { get; set; }
}