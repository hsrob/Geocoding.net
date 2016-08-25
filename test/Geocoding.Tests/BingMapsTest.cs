﻿using System.Linq;
using Geocoding.Microsoft;
using Xunit;

namespace Geocoding.Tests
{
	[Collection("Settings")]
	public class BingMapsTest : GeocoderTest
	{
		readonly SettingsFixture settings;
		BingMapsGeocoder geoCoder;

		public BingMapsTest(SettingsFixture settings)
		{
			this.settings = settings;
		}

		protected override IGeocoder CreateGeocoder()
		{
			geoCoder = new BingMapsGeocoder(settings.BingMapsKey);
			return geoCoder;
		}

		[Theory]
		[InlineData("United States", "fr", "États-Unis")]
		[InlineData("Montreal", "en", "Montreal, QC")]
		[InlineData("Montreal", "fr", "Montréal, QC")]
		public async void ApplyCulture(string address, string culture, string result)
		{
			geoCoder.Culture = culture;
			BingAddress[] addresses = (await geoCoder.Geocode(address)).ToArray();
			Assert.Equal(result, addresses[0].FormattedAddress);
		}

		[Theory]
		[InlineData("Montreal", 45.512401580810547, -73.554679870605469, "Canada")]
		[InlineData("Montreal", 43.949058532714844, 0.20011000335216522, "France")]
		[InlineData("Montreal", 46.428329467773438, -90.241783142089844, "United States")]
		public async void ApplyUserLocation(string address, double userLatitude, double userLongitude, string country)
		{
			geoCoder.UserLocation = new Location(userLatitude, userLongitude);
			BingAddress[] addresses = (await geoCoder.Geocode(address)).ToArray();
			Assert.Equal(country, addresses[0].CountryRegion);
		}

		[Theory]
		[InlineData("Montreal", 45, -73, 46, -74, "Canada")]
		[InlineData("Montreal", 43, 0, 44, 1, "France")]
		[InlineData("Montreal", 46, -90, 47, -91, "United States")]
		public async void ApplyUserMapView(string address, double userLatitude1, double userLongitude1, double userLatitude2, double userLongitude2, string country)
		{
			geoCoder.UserMapView = new Bounds(userLatitude1, userLongitude1, userLatitude2, userLongitude2);
			BingAddress[] addresses = (await geoCoder.Geocode(address)).ToArray();
			Assert.Equal(country, addresses[0].CountryRegion);
		}

		[Fact]
		//https://github.com/chadly/Geocoding.net/issues/8
		public async void CanReverseGeocodeIssue8()
		{
			BingAddress[] addresses = (await geoCoder.ReverseGeocode(38.8976777, -77.036517)).ToArray();
			Assert.NotEmpty(addresses);
		}
	}
}
