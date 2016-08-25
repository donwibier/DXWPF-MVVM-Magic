using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using NUnit.Framework;
using Webinar.ViewModels;

namespace Webinar.NUnitTests
{
    [TestFixture]
    class TrackViewModelTests
    {
        const string INITIAL_TRACKNAME = "My Test Track";
        const string MODIFIED_TRACKNAME = "My Modified Test Track";

        [SetUp]
        protected void SetUp()
        {
            // initialize your test here
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Tests ResetName with confirmation clicked no. </summary>
        ///
        /// <remarks>   Don, 25-8-2016. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void TestResetNameCommandNo()
        {
            
            var vm = TrackViewModel.Create(new TrackInfo() { Name = INITIAL_TRACKNAME});
            var serviceContainer = (vm as ISupportServices).ServiceContainer;
            
            IMessageBoxService msgSvc = new DummyServiceForMessageBox(MessageResult.No);
            serviceContainer.RegisterService(msgSvc);

            //Testing the ResetName behaviour while clicking No on the confirmation dialog...
            vm.ResetName();
            Assert.That(vm.Name, Is.EqualTo(INITIAL_TRACKNAME));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Tests ResetName with confirmation clicked yes. </summary>
        ///
        /// <remarks>   Don, 25-8-2016. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void TestResetNameCommandYes()
        {
            var vm = TrackViewModel.Create(new TrackInfo() { Name = INITIAL_TRACKNAME });
            var serviceContainer = (vm as ISupportServices).ServiceContainer;
            
            IMessageBoxService msgSvc = new DummyServiceForMessageBox(MessageResult.Yes);
            serviceContainer.RegisterService(msgSvc);

            //Testing the ResetName command while clicking Yes on the confirmation dialog...
            vm.ResetName();            
            Assert.That(vm.Name, Is.EqualTo(""));            
        }

        [Test]
        public void TestSaveCommand()
        {
            var vm = TrackViewModel.Create(new TrackInfo() { Name = INITIAL_TRACKNAME });
            vm.Name = MODIFIED_TRACKNAME;

            //Testing the Save command...
            vm.Save();
            Assert.That(vm.Name, Is.EqualTo(MODIFIED_TRACKNAME));
        }

        [Test]
        public void TestCancelCommand()
        {
            var vm = TrackViewModel.Create(new TrackInfo() { Name = INITIAL_TRACKNAME });
            vm.Name = MODIFIED_TRACKNAME;

            //Testing the Revert command...
            vm.Revert();
            Assert.That(vm.Name, Is.EqualTo(INITIAL_TRACKNAME));
        }
    }
}
