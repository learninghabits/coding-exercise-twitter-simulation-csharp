using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Twitter_Simulation.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ControllerTests
    {
        //Naming convention for tests : ClassName_MethodName_Condition_ExpectedResult

        [Test]
        public void Controller_Constructor_TweetObjectIsNull_ThrowsArgumentNullException()
        {
            var user = new Mock<IUser>();
            var presenter = new Mock<IDataPresenter>();
            Assert.Throws<ArgumentNullException>(() => new Controller(user.Object, null, presenter.Object));
        }

        [Test]
        public void Controller_Constructor_UserObjectIsNull_ThrowsArgumentNullException()
        {
            var tweet = new Mock<ITweet>();
            var presenter = new Mock<IDataPresenter>();
            Assert.Throws<ArgumentNullException>(() => new Controller(null, tweet.Object, presenter.Object));
        }

        [Test]
        public void Controller_Constructor_PresenterObjectIsNull_ThrowsArgumentNullException()
        {
            var tweet = new Mock<ITweet>();
            var user = new Mock<IUser>();
            Assert.Throws<ArgumentNullException>(() => new Controller(user.Object, tweet.Object, null));
        }

        [Test]
        public void Controller_Constructor_BothTweetAndUserAreValidInstances_ControllerInstanceIsCreated()
        {
            var tweet = new Mock<ITweet>();
            var user = new Mock<IUser>();
            var presenter = new Mock<IDataPresenter>();
            var controller = new Controller(user.Object, tweet.Object, presenter.Object);
            Assert.IsNotNull(controller, "Could not create an instance of the Controller class");
        }

        [Test]
        [TestCase("", "")]
        [TestCase(null, "trtrt")]
        [TestCase("eert", null)]
        [TestCase(" ", " ")]
        public void Controller_Run_InvalidArguments_ThrowsArgumentException(string path1, string path2)
        {
            var tweet = new Mock<ITweet>();
            var user = new Mock<IUser>();
            var presenter = new Mock<IDataPresenter>();
            var controller = new Controller(user.Object, tweet.Object, presenter.Object);
            Assert.Throws<ArgumentNullException>(() => controller.Run(path1, path2));
        }


        [Test]
        public void Controller_Run_UserReadReturnsNull_ThrowsAnException()
        {
            var user = new Mock<IUser>();
            user.Setup(m => m.Read(It.IsAny<string>())).Returns(() => null);
            var tweet = new Mock<ITweet>();
            tweet.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<ITweet>());
            var presenter = new Mock<IDataPresenter>();
            var controller = new Controller(user.Object, tweet.Object, presenter.Object);
            Assert.Throws<Exception>(() => controller.Run(@"C:\temp\data\tweetsimulation\user.txt", @"C:\temp\data\tweetsimulation\tweet.txt"));
        }


        [Test]
        public void Controller_Run_TweetReadReturnsNull_ThrowsAndException()
        {
            var user = new Mock<IUser>();
            user.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<IUser>());
            var tweet = new Mock<ITweet>();
            tweet.Setup(m => m.Read(It.IsAny<string>())).Returns(() => null);
            var presenter = new Mock<IDataPresenter>();
            var controller = new Controller(user.Object, tweet.Object, presenter.Object);
            Assert.Throws<Exception>(() => controller.Run(@"C:\temp\data\tweetsimulation\user.txt", @"C:\temp\data\tweetsimulation\tweet.txt"));
        }

        [Test]
        public void Controller_Run_UsersAndTweetsAreEmptyCollections_PresenterDidNotWriteData()
        {
            var user = new Mock<IUser>();
            user.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<IUser>());
            var tweet = new Mock<ITweet>();
            tweet.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<ITweet>());
            var presenter = new Mock<IDataPresenter>();        
            var controller = new Controller(user.Object, tweet.Object, presenter.Object);           
            controller.Run(@"C:\temp\data\tweetsimulation\user.txt", @"C:\temp\data\tweetsimulation\tweet.txt");
            presenter.Verify(m => m.Write(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void Controller_Run_AUserMatchesATweet_PresenterIsCalledToWrite()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@MxolisiZimu follows @TechGlobe_ZA, @apachecordova"
            }).Callback(() => {
                dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@MxolisiZimu> Coding Horrors continue to run a practical joke on devs"
            });
            });
            var userObj = new User(dataReader.Object);
            List<IUser> users = userObj.Read(@"C:\temp\data\tweetsimulation\");
            var user = new Mock<IUser>();
            user.Setup(m => m.Read(It.IsAny<string>())).Returns(() => users);
            var tweet = new Mock<ITweet>();
            var tweetObj = new Tweet(dataReader.Object);
            List<ITweet> tweets = tweetObj.Read(@"C:\temp\data\tweetsimulation\");
            tweet.Setup(m => m.Read(It.IsAny<string>())).Returns(() => tweets);
            var presenter = new Mock<IDataPresenter>();
            var controller = new Controller(user.Object, tweet.Object, presenter.Object);
            controller.Run(@"C:\temp\data\tweetsimulation\user.txt", @"C:\temp\data\tweetsimulation\tweet.txt");
            presenter.Verify(m => m.Write(It.IsAny<string>()), Times.AtLeast(2));
        }        
    }
}

