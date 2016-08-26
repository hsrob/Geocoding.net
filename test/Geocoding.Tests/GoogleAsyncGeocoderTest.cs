using System;
using System.Linq;
using Geocoding.Google;
using Xunit;

namespace Geocoding.Tests
{
	[Collection("Settings")]
	public class GoogleAsyncGeocoderTest : GeocoderTest
	{
		GoogleGeocoder geoCoder;

		[Theory]
		[InlineData("United States", GoogleAddressType.Country)]
		[InlineData("Illinois, US", GoogleAddressType.AdministrativeAreaLevel1)]
		[InlineData("New York, New York", GoogleAddressType.Locality)]
		[InlineData("90210, US", GoogleAddressType.PostalCode)]
		[InlineData("1600 pennsylvania ave washington dc", GoogleAddressType.StreetAddress)]
		public void CanParseAddressTypes(string address, GoogleAddressType type)
		{
			geoCoder.Geocode(address).ContinueWith(task =>
			{
				GoogleAddress[] addresses = task.Result.ToArray();
				Assert.Equal(type, addresses[0].Type);
			});
		}

	    protected override IGeocoder CreateGeocoder()
	    {
            string apiKey = settings.GoogleApiKey;

            if (string.IsNullOrEmpty(apiKey))
            {
                geoCoder = new GoogleGeocoder();
            }
            else
            {
                geoCoder = new GoogleGeocoder(apiKey);
            }

            return geoCoder;
        }
	}
}
