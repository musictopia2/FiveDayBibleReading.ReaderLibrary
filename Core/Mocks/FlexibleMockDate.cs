namespace FiveDayBibleReading.ReaderLibrary.Core.Mocks;
public class FlexibleMockDate : IDateOnlyPicker
{
    DateOnly IDateOnlyPicker.GetCurrentDate => dd1.DateToUse;
}