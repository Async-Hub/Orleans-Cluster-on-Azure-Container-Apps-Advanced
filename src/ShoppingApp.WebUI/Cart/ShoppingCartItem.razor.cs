using Microsoft.AspNetCore.Components;
using ShoppingApp.Abstractions;

namespace ShoppingApp.WebUI.Cart;

public partial class ShoppingCartItem
{
    private int _desiredQuantity;
    private string Title => $"Update {CartItem.Product.Name} quantity in cart";

    [Parameter, EditorRequired]
    public CartItem CartItem { get; set; } = null!;

    [Parameter, EditorRequired]
    public EventCallback<ProductDetails> OnRemoved { get; set; }

    [Parameter, EditorRequired]
    public EventCallback<(int Quantity, ProductDetails Product)> OnUpdated { get; set; }

    protected override void OnParametersSet() => _desiredQuantity = CartItem.Quantity;

    Task SaveOnUpdateAsync(int value)
    {
        _desiredQuantity = value;
        return TryInvokeDelegate(OnUpdated, (_desiredQuantity, CartItem.Product));
    }

    Task OnRemoveAsync() => TryInvokeDelegate(OnRemoved, CartItem.Product);

    Task OnUpdateAsync() => TryInvokeDelegate(OnUpdated, (_desiredQuantity, CartItem.Product));

    Task TryInvokeDelegate<TArg>(EventCallback<TArg> callback, TArg args) =>
        callback.HasDelegate
            ? callback.InvokeAsync(args)
            : Task.CompletedTask;
}