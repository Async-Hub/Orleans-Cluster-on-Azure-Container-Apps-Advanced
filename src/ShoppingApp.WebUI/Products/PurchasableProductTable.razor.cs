using Microsoft.AspNetCore.Components;
using ShoppingApp.Abstractions;
using ShoppingApp.WebUI.Extensions;

namespace ShoppingApp.WebUI.Products;

public partial class PurchasableProductTable
{
    private string? _filter;

    [Parameter, EditorRequired]
    public HashSet<ProductDetails> Products { get; set; } = null!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = null!;

    [Parameter, EditorRequired]
    public EventCallback<string> OnAddedToCart { get; set; }

    [Parameter, EditorRequired]
    public Func<ProductDetails, bool> IsInCart { get; set; } = null!;

    Task AddToCartAsync(string productId) =>
        OnAddedToCart.HasDelegate
            ? OnAddedToCart.InvokeAsync(productId)
            : Task.CompletedTask;

    bool OnFilter(ProductDetails product) => product.MatchesFilter(_filter);
}