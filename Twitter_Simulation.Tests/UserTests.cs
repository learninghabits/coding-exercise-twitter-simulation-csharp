using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Twitter_Simulation.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserTests
    {
        //Naming convention for tests : ClassName_MethodName_Condition_ExpectedResult
        [Test]
        public void User_Constructor_DataReaderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new User(null));
        }

        [Test]
        public void User_Constructor_DataReaderIsValidInstace_CreatesAUserInstance()
        {
            var user = new User(new Mock<IDataReader>().Object);
            Assert.NotNull(user, "could not create a user object");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void User_Read_FilePathIsNullOrEmpty_ThrowsArgumentNullException(string path)
        {
            var dataReader = new Mock<IDataReader>();
            var user = new User(dataReader.Object);
            Assert.Throws<ArgumentNullException>(() => user.Read(path));
        }

        [Test]
        public void User_Read_DataReaderReadMethodReturnsANull_ThrowsAnException()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => null);
            var user = new User(dataReader.Object);
            Assert.Throws<Exception>(() => user.Read(@"C:\temp\data\tweetsimulation\"));
        }

        [Test]
        public void User_Read_DataReaderReturnsAWellFormedUserLine_CreatesAUserObjectThatRepresentsTheLine()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@MxolisiZimu follows @TechGlobe_ZA, @apachecordova"
            });
            var user = new User(dataReader.Object);
            List<IUser> users = user.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("@MxolisiZimu", users[0].UserName, "Did not return the expected user");
            Assert.AreEqual(2, users[0].Follows.Count, "Did not return the expected number of follows");
            Assert.AreEqual("@TechGlobe_ZA", users[0].Follows[0], "Did not return the expected handle for the firsts index of the follows collection");
            Assert.AreEqual("@apachecordova", users[0].Follows[1], "Did not return the expected handle for the second index of the follows collection");
        }

        [Test]
        public void User_Read_DataReaderReturnsAMalformedUserLine_NoUserObjectIsCreated()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@MxolisiZimu doesn't follows @TechGlobe_ZA, @apachecordova"
            });
            var user = new User(dataReader.Object);
            List<IUser> users = user.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(0, users.Count, "How is this possible? Somehow you managed to create a user object out of a marlformed line. ");
        }

        [Test]
        public void User_Read_DataReaderReturns3ValidLinesFrom5ReadLines_Creates2UserObjects()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@mxo1 follows @TechGlobe_ZA, @apachecordova",
               "@mxo2 followings @TechGlobe_ZA, @apachecordova,",
               "mxo2 follows @TechGlobe_ZA, @apachecordova",
               "@mxo3 follows @TechGlobe_ZA, @apachecordova, @OdeToCode",
               "@mxo4 follows @TechGlobe_ZA @apachecordova, @OdeToCode",
            });
            var user = new User(dataReader.Object);
            List<IUser> users = user.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(3, users.Count);
        }

        [Test]
        public void User_Read_DataReaderReturnsAnUnorderedListOfValidHandles_UserHandlesAreOrdered()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@ZZZZ follows @TechGlobe_ZA, @apachecordova",
               "@SSSS follows @orange, @mango, @OdeToCode",
               "@XXXX follows @OdeToCode",
            });

            var user = new User(dataReader.Object);
            List<IUser> users = user.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(3, users.Count);
            Assert.AreEqual("@SSSS", users[0].UserName);
            Assert.AreEqual("@XXXX", users[1].UserName);
            Assert.AreEqual("@ZZZZ", users[2].UserName);
        }

        [Test]
        public void User_Read_DataReaderReturns3ValidLinesForTheSameHandle_OnlyOneUserObjectIsCreated()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@MxolisiZimu follows @TechGlobe_ZA, @apachecordova",
               "@MxolisiZimu follows @orange, @mango, @OdeToCode",
               "@MxolisiZimu follows @OdeToCode",
            });

            var user = new User(dataReader.Object);
            List<IUser> users = user.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(1, users.Count);
        }


        [Test]
        public void User_Read_DataReaderReturns3ValidLinesForTheSameHandleWithDuplicatedFollowsHandles_UserObjectsAreOnlyCreatedForUniqueHandles()
        {
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.Read(It.IsAny<string>())).Returns(() => new List<string>
            {
               "@MxolisiZimu follows @TechGlobe_ZA, @apachecordova",
               "@MxolisiZimu follows @TechGlobe_ZA, @apachecordova, @OdeToCode",
               "@MxolisiZimu follows @OdeToCode",
            });

            var user = new User(dataReader.Object);
            List<IUser> users = user.Read(@"C:\temp\data\tweetsimulation\");
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(3, users[0].Follows.Count);
            Assert.AreEqual("@TechGlobe_ZA", users[0].Follows[0]);
            Assert.AreEqual("@apachecordova", users[0].Follows[1]);
            Assert.AreEqual("@OdeToCode", users[0].Follows[2]);
        }
    }
}
