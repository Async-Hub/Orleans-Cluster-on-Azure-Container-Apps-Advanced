using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingApp.Abstractions;
using ShoppingApp.WebUI.Extensions;

namespace ShoppingApp.WebUI.Products;

public partial class ManageProductModal
{
    private bool _isSaving;
    private MudForm? _form;

    public ProductDetails Product { get; set; } = new();

    [CascadingParameter] 
    private IMudDialogInstance? MudDialog { get; set; }

    [Parameter, EditorRequired]
    public EventCallback<ProductDetails> ProductUpdated { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    public async Task OpenAsync(string title, Func<ProductDetails, Task> onProductUpdated) =>
        await DialogService.ShowAsync<ManageProductModal>(
            title, new DialogParameters()
            {
                {
                    nameof(ProductUpdated),
                    new EventCallbackFactory().Create(this, onProductUpdated)
                }
            });

    public void Close() => MudDialog?.Cancel();

    private void Bogus() => Product = Product.GetBogusFaker().Generate();

    private Task Save()
    {
        if (_form is not null)
        {
            _form.Validate();
            if (_form.IsValid)
            {
                return OnValidSubmitAsync();
            }
        }

        return Task.CompletedTask;
    }

    private async Task OnValidSubmitAsync()
    {
        if (!string.IsNullOrWhiteSpace(Product.Id) && ProductUpdated.HasDelegate)
        {
            try
            {
                _isSaving = true;
                await ProductUpdated.InvokeAsync(Product);
            }
            finally
            {
                _isSaving = false;
                Close();
            }
        }
    }
}