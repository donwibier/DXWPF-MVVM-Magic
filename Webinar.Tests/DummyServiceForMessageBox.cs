using System;
using DevExpress.Mvvm;

namespace Webinar.NUnitTests
{
    internal class DummyServiceForMessageBox : IMessageBoxService
    {
        private readonly MessageResult resultToTest;
        public DummyServiceForMessageBox(MessageResult resultToTest)
        {
            this.resultToTest = resultToTest;
        }
        MessageResult IMessageBoxService.Show(string messageBoxText, string caption, 
            MessageButton button, MessageIcon icon, MessageResult defaultResult)
        {
            return this.resultToTest;
        }
    }
}