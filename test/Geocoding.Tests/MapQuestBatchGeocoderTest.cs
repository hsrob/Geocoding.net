using Geocoding.MapQuest;
using Xunit;

namespace Geocoding.Tests
{
	[Collection("Settings")]
	public class MapQuestBatchGeocoderTest : BatchGeocoderTest
	{
		protected override IBatchGeocoder CreateBatchGeocoder()
		{
			return new MapQuestGeocoder(settings.MapQuestKey);
		}
	}
}
