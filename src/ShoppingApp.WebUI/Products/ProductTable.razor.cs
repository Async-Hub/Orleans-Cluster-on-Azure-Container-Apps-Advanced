using Microsoft.AspNetCore.Components;
using ShoppingApp.Abstractions;
using ShoppingApp.WebUI.Extensions;

namespace ShoppingApp.WebUI.Products;

public partial class ProductTable
{
    private string? _filter;

    private ProductDetails? _productBeforeEdit;

    [Parameter, EditorRequired]
    public HashSet<ProductDetails> Products { get; set; } = null!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = null!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter, EditorRequired]
    public EventCallback<ProductDetails> EditProduct { get; set; }

    void OnEdit(object model)
    {
        if (model is ProductDetails product &&
            EditProduct.HasDelegate)
        {
            _ = EditProduct.InvokeAsync(product);
        }
    }

    void BackupItem(object model)
    {
        if (model is ProductDetails product)
        {
            _productBeforeEdit = product;
        }
    }

    void RevertEditChanges(object model)
    {
        if (model is ProductDetails product &&
            _productBeforeEdit is not null)
        {
            model = _productBeforeEdit with { };
        }
    }

    bool OnFilter(ProductDetails product) => product.MatchesFilter(_filter);
}