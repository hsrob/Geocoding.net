using Microsoft.Extensions.Configuration;
using Xunit;

namespace Geocoding.Tests
{
	public class SettingsFixture
	{
		readonly IConfigurationRoot config;

		public SettingsFixture()
		{
			config = new ConfigurationBuilder()
				.AddJsonFile("settings.json")
				.AddJsonFile("settings-override.json", optional: true)
				.Build();
		}

		public string YahooConsumerKey
		{
			get { return GetStringOrDefault("yahooConsumerKey"); }
		}

		public string YahooConsumerSecret
		{
			get { return GetStringOrDefault("yahooConsumerSecret"); }
		}

		public string BingMapsKey
		{
			get { return GetStringOrDefault("bingMapsKey"); }
		}

		public string GoogleApiKey
		{
			get { return GetStringOrDefault("googleApiKey"); }
		}

		public string MapQuestKey
		{
			get { return GetStringOrDefault("mapQuestKey"); }
		}

	    string GetStringOrDefault(string keyName)
	    {
	        var val = config.GetValue<string>(keyName);
	        return !string.IsNullOrWhiteSpace(val) ? val : "";
	    }
	}

	[CollectionDefinition("Settings")]
	public class SettingsCollection : ICollectionFixture<SettingsFixture>
	{
		// https://xunit.github.io/docs/shared-context.html
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
