
using CommonBasicLibraries.BasicDataSettingsAndProcesses;

namespace FiveDayBibleReading.ReaderLibrary.Blazor.Components;

public partial class SimpleCheckReadingComponent(BibleReaderService readerService)
{
    private enum EnumStatus
    {
        Starting,
        ShowReading,
        FinishedReading
    }
    [Parameter]
    public EventCallback FinishedReading { get; set; }
    //their focusfirst is not working.
    //[Parameter]
    //public bool NeedsFocusFirst { get; set; } = false; //sometimes you need to focus first.
    private EnumStatus _status = EnumStatus.Starting;

    private bool _processing;
    [CascadingParameter]
    private MediaQueryListComponent? Media { get; set; }
    private bool _keyboard;
    protected override void OnParametersSet()
    {
        _keyboard = Media!.HasKeyboard(EnumKeyboardCategory.Real);
    }
    protected override async Task OnInitializedAsync()
    {
        _processing = true;

        try
        {
            await readerService.LoadAsync();

            _status = readerService.IsReadingDue
                ? EnumStatus.ShowReading
                : EnumStatus.FinishedReading;
        }
        finally
        {
            _processing = false;
        }
    }
    private async Task FinishReadingAsync()
    {
        _status = EnumStatus.FinishedReading;

        await readerService.CompleteCurrentChapterAsync();

        //ScheduleService.FinishedReading?.Invoke(); //if not null, then invoke.
        if (FinishedReading.HasDelegate)
        {
            await FinishedReading.InvokeAsync();
        }
    }
    private async Task FinishedSectionAsync()
    {
        if (_processing)
        {
            return;
        }

        _processing = true;

        try
        {
            var chapter = readerService.CurrentChapter;

            await readerService.CompleteCurrentChapterAsync();

            var next = readerService.CurrentChapter;

            _status = readerService.IsReadingDue
                ? EnumStatus.ShowReading
                : EnumStatus.FinishedReading;
        }
        
        finally
        {
            _processing = false;
        }
    }
}