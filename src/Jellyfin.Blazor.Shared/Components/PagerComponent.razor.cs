using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Components;

/// <summary>
/// The pager component.
/// </summary>
public partial class PagerComponent
{
    private int _startPage;
    private int _endPage;

    /// <summary>
    /// Gets or sets the page count.
    /// </summary>
    [Parameter]
    public int PageCount { get; set; }

    /// <summary>
    /// Gets or sets the current page.
    /// </summary>
    [Parameter]
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the visible page count.
    /// </summary>
    [Parameter]
    public int VisiblePageCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the page changed callback function.
    /// </summary>
    [Parameter]
    public Func<int, Task>? OnPageChanged { get; set; }

    /// <summary>
    /// Gets or sets the nav css.
    /// </summary>
    [Parameter]
    public string NavigationCss { get; set; }
        = "relative z-0 inline-flex rounded-md shadow-sm -space-x-px";

    /// <summary>
    /// Gets or sets the chevron css.
    /// </summary>
    [Parameter]
    public string ChevronCss { get; set; }
        = "w-5 h-5";

    /// <summary>
    /// Gets or sets the current page css.
    /// </summary>
    [Parameter]
    public string CurrentPageCss { get; set; }
        = "z-10 bg-indigo-50 border-indigo-500 text-indigo-600 relative inline-flex items-center px-4 py-2 border text-sm font-medium";

    /// <summary>
    /// Gets or sets the page css.
    /// </summary>
    [Parameter]
    public string PageCss { get; set; }
        = "bg-white border-gray-300 text-gray-500 hover:bg-gray-50 relative inline-flex items-center px-4 py-2 border text-sm font-medium";

    /// <summary>
    /// Gets or sets the cursor pointer class.
    /// </summary>
    [Parameter]
    public string CursorHover { get; set; }
        = "cursor-pointer";

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        var pagesToShow = Math.Min(VisiblePageCount, PageCount);
        var halfPages = (int)Math.Floor(pagesToShow / 2d);
        _startPage = Math.Max(CurrentPage - halfPages, 0);
        _endPage = Math.Min(CurrentPage + halfPages, PageCount);

        if (_startPage == 0)
        {
            _endPage = pagesToShow;
        }

        if (_endPage >= PageCount)
        {
            _endPage = PageCount - 1;
            _startPage = PageCount - pagesToShow;
        }

        if (_startPage < 0)
        {
            _startPage = 0;
        }

        InvokeAsync(() => StateHasChanged());
        base.OnParametersSet();
    }

    private void PageChanged(int page)
    {
        if (page != CurrentPage && page >= 0 && page < PageCount)
        {
            OnPageChanged?.Invoke(page);
        }
    }
}
