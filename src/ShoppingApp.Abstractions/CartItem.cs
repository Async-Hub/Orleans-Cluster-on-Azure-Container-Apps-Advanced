namespace ShoppingApp.Abstractions;

[GenerateSerializer, Immutable]
public sealed record CartItem(
    string UserId, int Quantity, ProductDetails Product)
{
    [JsonIgnore]
    public decimal TotalPrice =>
        Math.Round(Quantity * Product.UnitPrice, 2);
}