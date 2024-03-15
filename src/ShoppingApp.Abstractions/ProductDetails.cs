using JetBrains.Annotations;

namespace ShoppingApp.Abstractions;

[GenerateSerializer, Immutable]
public sealed record ProductDetails
{
	[Id(0)] public string Id { get; set; } = null!;
	[Id(1)] public string Name { get; set; } = null!;
	[Id(2)] public string Description { get; set; } = null!;
	[Id(3)] public ProductCategory Category { get; [UsedImplicitly] set; }
	[Id(4)] public int Quantity { get; set; }
	[Id(5)] public decimal UnitPrice { get; set; }
	[Id(6)] public string DetailsUrl { get; set; } = null!;
	[Id(7)] public string ImageUrl { get; set; } = null!;

	public void Copy(ProductDetails productDetails)
	{
		Id = productDetails.Id;
		Name = productDetails.Name;
		Description = productDetails.Description;
		Category = productDetails.Category;
		Quantity = productDetails.Quantity;
		UnitPrice = productDetails.UnitPrice;
		DetailsUrl = productDetails.DetailsUrl;
		ImageUrl = productDetails.ImageUrl;
	}
}