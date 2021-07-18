using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [TestCleanup]
        public void Cleanup()
        {

        }

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
        public void Equatable()
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
        public void ComparableMAX()
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
        public void EnumFlags()
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
    }
}
