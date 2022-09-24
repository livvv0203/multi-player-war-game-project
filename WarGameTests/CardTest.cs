using System;
using WarGameProject.DataModels;
namespace WarGameTests
{
    [TestClass]
    public class CardTest
    {


        [TestMethod]
        [DataRow(2)]
        [DataRow(14)]
        public void CardCreation(int cardNumber)
        {

            // Arrange
            var c = Card.Create(cardNumber);

            // Act

            // Assert
            Assert.AreEqual(c.Number, cardNumber);
        }

        [TestMethod]
        [DataRow(2, 14)]
        [DataRow(3,6)]
        public void CardComparison(int cardLeft, int cardRight)
        {

            // Arrange
            var l = Card.Create(cardLeft);
            var r = Card.Create(cardRight);

            // Act

            // Assert
            Assert.AreEqual(cardLeft>cardRight, l>r);
        }

    }
}

