using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleCode
{
    [TestClass]
    public class KnownPatternTest
    {
        public Person FirstPerson { get; set; }
        public Person SecondPerson { get; set; }
        public TestContext TestContext { get; set; }
        #region InitVariable

        [TestInitialize]
        public void Initialize()
        {
            FirstPerson = new()
            {
                Age = 29,
                Name = "kook, choi"
            };
            SecondPerson = new()
            {
                Age = 32,
                Name = "grace, lee"
            };
        }
        #endregion
        [TestMethod]
        public void EquatableTest()
        {
            //
            // new reference
            //
            Person thirdPerson = new()
            {
                Age = 29,
                Name = "kook, choi"
            };

            Assert.AreEqual(thirdPerson, FirstPerson);

            TestContext.WriteLine($"{ReferenceEquals(thirdPerson, FirstPerson)}");
        }

        [TestMethod]
        public void ComparableMAXTest()
        {
            //
            // new reference
            //
            Person thirdPerson = new()
            {
                Age = 2,
                Name = "jr, choi"
            };

            Person[] people = new[] { thirdPerson, SecondPerson, FirstPerson };

            //
            // MAX Linq가 Comparable Interface를 사용
            //
            Assert.AreEqual(people.Max(person => person), SecondPerson);
        }


        [TestMethod]
        public void EnumFlagsTest()
        {
            // 비트 연산과 enum + flags attributes를 활용
            // 0001 -> 1, 0010 -> 2, 0011 -> 3, 0100 -> 4
            // 1(0001) or 2(0010) -> 3(0011)
            // 1(0001) or 2(0010) or 4(0100) -> 7(0111)


            // 이미지 파일 이면서 암호화
            var imageEncrypt = MyStatus.Image | MyStatus.Encrypt;
            Assert.IsTrue(imageEncrypt.HasFlag(MyStatus.Encrypt));
            Assert.IsTrue(imageEncrypt.HasFlag(MyStatus.Image));
            Assert.IsFalse(imageEncrypt.HasFlag(MyStatus.Text));

            // 텍스트 파일 이면서 암호화
            var textEncrypt = MyStatus.Text | MyStatus.Encrypt;
            Assert.IsTrue(textEncrypt.HasFlag(MyStatus.Encrypt));
            Assert.IsTrue(textEncrypt.HasFlag(MyStatus.Text));
            Assert.IsFalse(textEncrypt.HasFlag(MyStatus.Image));

            // 이미지 파일이면서 텍스트일 수 없음
            // Text = 2, Image = 1이기 때문에 HasFlag에 Text만 포함됨.
            var imageText = MyStatus.Text | MyStatus.Image;
            Assert.IsFalse(textEncrypt.HasFlag(MyStatus.Image));
            Assert.IsTrue(textEncrypt.HasFlag(MyStatus.Text));
        }

        [TestMethod]
        public void EnumAttributeTest()
        {
            //
            // enum attribute 꺼내오는 방법
            //

            var status = MyStatus.Image;

            //
            // enum의 member field로는 attribute를 가져올 수 없음.
            //
            if(status.GetType().GetCustomAttribute<StatusAttribute>() is StatusAttribute fail)
            {
                Assert.Fail();
            }

            if(typeof(MyStatus).GetMember($"{status}").FirstOrDefault() is MemberInfo member)
            {
                if(member.GetCustomAttribute<StatusAttribute>() is StatusAttribute statusAttribute)
                {
                    TestContext.WriteLine(statusAttribute.Message);
                    Assert.AreEqual(statusAttribute.Message, "이미지 상태");
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void ZipTest()
        {
            //
            // System.IO.Compression & System.IO.Compression.FileSystem
            //
            string fileName = Path.Combine(TestContext.DeploymentDirectory, "sample.zip");

            //
            // zip 파일 이름으로 stream 인스턴스 생성
            //
            using FileStream fileStream = new (fileName, FileMode.Create, FileAccess.Write);
            //
            // 생성된 stream 인스턴스 zip archive에 전달
            //
            using ZipArchive zipArchive = new (fileStream, ZipArchiveMode.Create);


            foreach (var idx in Enumerable.Range(1, 10))
            {
                //
                // zip 파일 안에 entry 인스턴스 생성
                //
                var entry = zipArchive.CreateEntry($"{idx}.txt");
                //
                // entry 인스턴스 stream 전달
                //
                using StreamWriter writer = new StreamWriter(entry.Open());
                //
                // 전달받은 stream write
                //
                writer.WriteLine($"{idx}");
            }

            TestContext.WriteLine(fileName);
        }
    }
}
