using Microsoft.AspNetCore.Components;
using ShoppingApp.WebUI.Services;

namespace ShoppingApp.WebUI.Shared;

public partial class NavMenu
{
    private int _count = 0;

    [Inject] 
    public ComponentStateChangedObserver Observer { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Observer.OnStateChanged += UpdateCountAsync;

        await UpdateCountAsync();
    }

    private Task UpdateCountAsync() =>
        InvokeAsync(async () =>
        {
            _count = await Cart.GetCartCountAsync();
            StateHasChanged();
        });
}