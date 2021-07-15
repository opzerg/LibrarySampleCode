using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YonggukUtils
{
    public static class MessageBoxUtils
    {
        /// <summary>
        /// detail message box form
        /// </summary>
        /// <param name="title">제목</param>
        /// <param name="message">메시지</param>
        /// <param name="detailMessage">자세히 메시지</param>
        /// <returns>DialogResult OK or Cancel</returns>
        public static Form CreateDetailMessageBox(string title, string message, string detailMessage)
        {
            var dialogTypeName = "System.Windows.Forms.PropertyGridInternal.GridErrorDlg";
            var dialogType = typeof(Form).Assembly.GetType(dialogTypeName);

            var dialog = (Form)Activator.CreateInstance(dialogType, new PropertyGrid());

            dialog.Text = title;
            dialogType.GetProperty("Message").SetValue(dialog, message, null);
            dialogType.GetProperty("Details").SetValue(dialog, detailMessage, null);

            return dialog;
        }
    }
}
