﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelpfulThings.Connect.Cryptowatch.Tests.Live
{
    [TestClass]
    public class PairsClientTests : BaseTest
    {
        [TestMethod]
        public void Pairs_GetAllPairsAsync()
        {
            var getAllAssetsTask = TestApiClient.Pairs.GetAllPairsAsync();
            getAllAssetsTask.Wait();

            var result = getAllAssetsTask.Result;

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void Pairs_GetPairBySymbolAsync()
        {
            var getAllAssetsTask = TestApiClient.Pairs.GetPairBySymbolAsync("ethbtc");
            getAllAssetsTask.Wait();

            var result = getAllAssetsTask.Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.QuoteAsset);
            Assert.IsNotNull(result.BaseAsset);
            Assert.IsNotNull(result.Symbol);
            Assert.IsNotNull(result.Route);
            Assert.IsNotNull(result.Markets);
            Assert.AreNotEqual(0, result.Markets.Count);
        }
    }
}
