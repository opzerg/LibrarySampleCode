using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YonggukUtils;

namespace SampleCode
{
    [TestClass]
    public class MessageBoxUtilsTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void DetailMessageBoxTest()
        {
            using var form = MessageBoxUtils.CreateDetailMessageBox("제목", "내용", "자세히");

            if (form.ShowDialog() == DialogResult.OK)
                TestContext.WriteLine("OK");
            else
                TestContext.WriteLine("Cancel");
        }
    }
}
