
using BasicBlazorLibrary.Components.Basic;

namespace FiveDayBibleReading.ReaderLibrary.Blazor.Components;
public partial class BibleReadingComponent(IBibleContent content)
{
    private ReaderModel _tempModel = new();
    
    [Parameter]
    [EditorRequired]
    public string BookName { get; set; } = "";
    [Parameter]
    [EditorRequired]
    public int Chapter { get; set; }
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    [Parameter]
    [EditorRequired]
    public bool HasMore { get; set; }
    private ReaderComponent<string>? _reader;
    private bool _rendered;
    [Parameter]
    public EventCallback OnNextReading { get; set; }
    [Parameter]
    public EventCallback OnFinished { get; set; }
    private BasicList<string> _verses = new();
    [CascadingParameter]
    private MediaQueryListComponent? Media { get; set; }
    [Inject]
    private IJSRuntime? JS { get; set; }

    private bool _keyboard;
    protected override void OnParametersSet()
    {
        _keyboard = Media!.HasKeyboard(EnumKeyboardCategory.Real);
    }
    protected override async Task OnParametersSetAsync() //try on parameter set instead of oninitialized.
    {
        _tempModel.Reset(); //hopefully this will make it this simple (?)
        if (_rendered && _reader is not null)
        {
            await _reader!.FocusAsync();
        }
        if (Chapter == 0)
        {
            return;
        }
        _verses = await content.GetBookChaptersDataAsync(BookName, Chapter, Chapter);
    }
    private string GetButtonText() => HasMore ? "Next Reading" : "Finished Reading For Day";
    private async Task ProcessAsync()
    {
        if (HasMore)
        {
            await OnNextReading.InvokeAsync();
            return;
        }
        await OnFinished.InvokeAsync();
    }
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _rendered = true;
        }
        base.OnAfterRender(firstRender);
    }

    private void ProcessArrowDown()
    {
        _reader!.ArrowDown();
    }
    private bool _fullScreen;

    private async Task ManuallyOpenCloseFullScreenAsync()
    {
        if (_fullScreen)
        {
            await JS!.ExitFullScreen();
            _fullScreen = false;
        }
        else
        {
            await JS!.OpenFullScreen();
            _fullScreen = true;
        }
    }
    private string GetDisplay => _fullScreen == false ? "Open Full Screen" : "Exit Full Screen";
}