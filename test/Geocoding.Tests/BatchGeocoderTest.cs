﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Extensions;

namespace Geocoding.Tests
{
	public abstract class BatchGeocoderTest
	{
		readonly IBatchGeocoder batchGeocoder;
        protected readonly SettingsFixture settings;
        public BatchGeocoderTest() 
		{
			CultureInfo.CurrentCulture = new CultureInfo("en-us");
            settings = new SettingsFixture();
			batchGeocoder = CreateBatchGeocoder();
		}

		protected abstract IBatchGeocoder CreateBatchGeocoder();

		[Theory]
		[MemberData("BatchGeoCodeData")]
		public virtual async void CanGeoCodeAddress(string[] addresses)
		{
			Assert.NotEmpty(addresses);

			IEnumerable<ResultItem> results = await batchGeocoder.Geocode(addresses);
			Assert.NotEmpty(results);
			Assert.Equal(addresses.Length, results.Count());

			var ahash = new HashSet<string>(addresses);
			Assert.Equal(ahash.Count, results.Count());

			foreach (ResultItem r in results)
			{
				Assert.NotNull(r);
				Assert.NotNull(r.Request);
				Assert.NotNull(r.Response);

				Assert.Contains(r.Request.FormattedAddress, ahash);

				Address[] respa = r.Response.ToArray();
				Assert.NotEmpty(respa);

				ahash.Remove(r.Request.FormattedAddress);
			}
			Assert.Empty(ahash);
		}

		public static IEnumerable<object[]> BatchGeoCodeData
		{
			get
			{
				yield return new object[] 
				{
					new string[] 
					{
						"1600 pennsylvania ave nw, washington dc",
						"1460 4th Street Ste 304, Santa Monica CA 90401",
					},
				};
			}
		}

	}
}
