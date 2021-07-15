using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using YonggukUtils;

namespace SampleCode4._5
{
    [TestClass]
    public class YonggukUtilsTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void DetailMessageBoxTest()
        {
            using (var form = MessageBoxUtils.CreateDetailMessageBox("제목", "내용", "자세히"))
            {
                TestContext.WriteLine($"{form.ShowDialog()}");
            }
        }
    }
}
