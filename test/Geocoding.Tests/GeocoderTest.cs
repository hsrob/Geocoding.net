using System.Globalization;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Extensions;

namespace Geocoding.Tests
{
	public abstract class GeocoderTest
	{
		readonly IGeocoder geocoder;

		public GeocoderTest()
		{
            CultureInfo.CurrentCulture = new CultureInfo("en-us");

			geocoder = CreateGeocoder();
		}

		protected abstract IGeocoder CreateGeocoder();

		[Theory]
		[InlineData("1600 pennsylvania ave nw, washington dc")]
		public virtual async void CanGeocodeAddress(string address)
		{
			Address[] addresses = (await geocoder.Geocode(address)).ToArray();
			addresses[0].AssertWhiteHouse();
		}

		[Fact]
		public virtual async void CanGeocodeNormalizedAddress()
		{
			Address[] addresses = (await geocoder.Geocode("1600 pennsylvania ave nw", "washington", "dc", null, null)).ToArray();
			addresses[0].AssertWhiteHouse();
		}

		[Theory]
		[InlineData("en-US")]
		[InlineData("cs-CZ")]
		public virtual async void CanGeocodeAddressUnderDifferentCultures(string cultureName)
		{

            CultureInfo.CurrentCulture = new CultureInfo(cultureName);

			Address[] addresses = (await geocoder.Geocode("24 sussex drive ottawa, ontario")).ToArray();
			addresses[0].AssertCanadianPrimeMinister();
		}

		[Theory]
		[InlineData("en-US")]
		[InlineData("cs-CZ")]
		public virtual async void CanReverseGeocodeAddressUnderDifferentCultures(string cultureName)
		{
			CultureInfo.CurrentCulture = new CultureInfo(cultureName);

			Address[] addresses = (await geocoder.ReverseGeocode(38.8976777, -77.036517)).ToArray();
			addresses[0].AssertWhiteHouseArea();
		}

		[Fact]
		public virtual async void ShouldNotBlowUpOnBadAddress()
		{
			Address[] addresses = (await geocoder.Geocode("sdlkf;jasl;kjfldksj,fasldf")).ToArray();
			Assert.Empty(addresses);
		}

		[Theory]
		[InlineData("Wilshire & Bundy, Los Angeles")]
		public virtual async void CanGeocodeWithSpecialCharacters(string address)
		{
			Address[] addresses = (await geocoder.Geocode(address)).ToArray();

			//asserting no exceptions are thrown and that we get something
			Assert.NotEmpty(addresses);
		}

		[Fact]
		public virtual async void CanReverseGeocode()
		{
			Address[] addresses = (await geocoder.ReverseGeocode(38.8976777, -77.036517)).ToArray();
			addresses[0].AssertWhiteHouseArea();
		}

		[Theory]
		[InlineData("1 Robert Wood Johnson Hosp New Brunswick, NJ 08901 USA")]
		[InlineData("miss, MO")]
		//https://github.com/chadly/Geocoding.net/issues/6
		public virtual async void CanGeocodeInvalidZipCodes(string address)
		{
			Address[] addresses = (await geocoder.Geocode(address)).ToArray();
			Assert.NotEmpty(addresses);
		}
	}
}