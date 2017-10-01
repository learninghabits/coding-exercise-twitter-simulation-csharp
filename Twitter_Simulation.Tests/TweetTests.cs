using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Twitter_Simulation.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TweetTests
    {
        //Naming convention for tests : ClassName_MethodName_Condition_ExpectedResult
        [Test]
        public void Tweet_Constructor_DataReaderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Tweet(null));
        }

        [Test]
        public void Tweet_Constructor_DataReaderIsValidInstace_CreatesATweetInstance()
        {
            var tweet = new Tweet(new Mock<IDataReader>().Object);
            Assert.NotNull(tweet, "could not create a tweet object");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Tweet_Read_FilePathIsNullOrEmpty_ThrowsArgumentNullException(string path)
        {
            var dataReader = new Mock<IDataReader>();
            var tweet = new Tweet(dataReader.Object);
            Assert.Throws<ArgumentNullException>(() => tweet.Read(path));
        }

        [Test]
        public void Tweet_Read_DataReaderReadMethodReturnsANull_ThrowsAnException()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => null);
            var tweet = new Tweet(dataReader.Object);
            Assert.Throws<Exception>(() => tweet.Read(@"C:\temp\data\tweetsimulation\"));
        }

        [Test]
        public void Tweet_Read_DataReaderReturnsAWellFormedTweetLine_CreatesATweetObjectThatRepresentsTheLine()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@tshepontlhokoa> Is it unit tests or an insurance policy?"
            });
            var tweet = new Tweet(dataReader.Object);
            List<ITweet> tweets = tweet.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(1, tweets.Count);
            Assert.AreEqual("@tshepontlhokoa", tweets[0].User, "Did not return the expected user");
            Assert.AreEqual("Is it unit tests or an insurance policy?", tweets[0].Message, "Did not return the expected message");
        }

        [Test]
        public void Tweet_Read_DataReaderReturnsMessageOver140Charaters_ATweetObjectIsNotCreated()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@tshepontlhokoa> Is it unit tests or an insurance policy? There are generally two groups who differ greatly on this topic. One group believes they are a non-negotiable while the other."
            });
            var tweet = new Tweet(dataReader.Object);
            List<ITweet> tweets = tweet.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(0, tweets.Count);
        }

        [Test]
        public void Tweet_Read_DataReaderReturnsAMalformedTweetLine_NoTweetObjectIsCreated()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@tshepontlhokoa>"
            });
            var tweet = new Tweet(dataReader.Object);
            List<ITweet> tweets = tweet.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(0, tweets.Count, "How is this possible? Somehow you managed to create a tweet object out of a marlformed line. ");
        }

        [Test]
        public void Tweet_Read_DataReaderReturns2ValidLinesFrom3ReadLines_Creates2TweetObjects()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@tshepontlhokoa> Is it unit tests or an insurance policy?",
               "@tshepontlhokoa> If I could look into the feature I may well find that programming skills are as necessary as using a computer today",
               "@MxolisiZimu >> You don't have to look a certain way, or be a certain faith, or have a certain last name in order to have a good idea" //invalid line (does not comform to pattern "@user> message"
            });
            var tweet = new Tweet(dataReader.Object);
            List<ITweet> tweets =  tweet.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(2, tweets.Count);
        }
    }
}
