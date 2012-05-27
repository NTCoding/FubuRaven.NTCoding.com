using HtmlTags;

namespace Web.Utilities
{
	public static class ViewHelper
	{
		public static HtmlTag GetRatingStars(int rating)
		{
			var t = new HtmlTag("span");

			for (int i = 0; i < rating; i++)
			{
				var img = new HtmlTag("img");
				img.Attr("src", "/Public/images/rating.png");
				img.AddClass("ratingImage");
				t.Append(img);
			}

			return t;
		}
	}
}