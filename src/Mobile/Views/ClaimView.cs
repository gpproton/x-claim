using CommunityToolkit.Maui.Markup;

namespace XClaim.Mobile.Views;

public class ClaimView : ContentPage
{
	public ClaimView()
	{
		Content = new VerticalStackLayout
		{
			Children = {
                new Label { Text = "Claim view!" }.TextCenterHorizontal().TextCenterVertical()
            }
		};
	}
}