using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace ShoppingApp.E2ETests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MyTest : PageTest
{
    private string _webUiCaUrl = "https://localhost:7159";

    [OneTimeSetUp]
    public void Init()
    {
        var webUiCaUrl = TestContext.Parameters["webUiCaUrl"];

        if (string.IsNullOrWhiteSpace(webUiCaUrl)) return;

        _webUiCaUrl = $"https://{webUiCaUrl}";
    }

    [Test]
    public async Task ShouldHaveTheCorrectSlogan()
    {
        await Page.GotoAsync(_webUiCaUrl);
        await Expect(Page.GetByText("Welcome to the Orleans Shopping Cart")).ToBeVisibleAsync();
    }

    [Test]
    public async Task CreateNewProductButtonWorks()
    {
        await Page.GotoAsync(_webUiCaUrl + "/products",
            new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

        await Page.WaitForTimeoutAsync(3000);
        await Page.Locator("//button[contains(.,'Create New Product')]").ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync();
    }
}