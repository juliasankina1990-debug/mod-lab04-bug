using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class BugWorkflowTests
    {
        [TestMethod]
        public void Test_InitialState_NewDefect()
        {
            var bug = new Bug();
            Assert.AreEqual(BugState.NewDefect, bug.GetState());
        }

        [TestMethod]
        public void Test_Analyze_TransitionsToTriage()
        {
            var bug = new Bug();
            bug.Analyze();
            Assert.AreEqual(BugState.Triage, bug.GetState());
        }

        [TestMethod]
        public void Test_Defer_FromTriage_ToNotNow()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.Defer();
            Assert.AreEqual(BugState.NotNow, bug.GetState());
        }

        [TestMethod]
        public void Test_WontFix_FromTriage_ToNotFix()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.WontFix();
            Assert.AreEqual(BugState.NotFix, bug.GetState());
        }

        [TestMethod]
        public void Test_SeparateSolution_FromTriage_ToSeparateSolution()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.SeparateSolution();
            Assert.AreEqual(BugState.SeparateSolution, bug.GetState());
        }

        [TestMethod]
        public void Test_Duplicate_FromTriage_ToDuplicate()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.Duplicate();
            Assert.AreEqual(BugState.Duplicate, bug.GetState());
        }

        [TestMethod]
        public void Test_OtherProduct_FromTriage_ToOtherProduct()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.OtherProduct();
            Assert.AreEqual(BugState.OtherProduct, bug.GetState());
        }

        [TestMethod]
        public void Test_NeedInfo_FromTriage_ToNeedMoreInfo()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.NeedInfo();
            Assert.AreEqual(BugState.NeedMoreInfo, bug.GetState());
        }

        [TestMethod]
        public void Test_CannotReproduce_FromTriage_ToCannotReproduce()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.CannotReproduce();
            Assert.AreEqual(BugState.CannotReproduce, bug.GetState());
        }

        [TestMethod]
        public void Test_StartFix_FromTriage_ToFix()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.StartFix();
            Assert.AreEqual(BugState.Fix, bug.GetState());
        }

        [TestMethod]
        public void Test_Verify_FromFix_ToOK()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.StartFix();
            bug.Verify();
            Assert.AreEqual(BugState.OK, bug.GetState());
        }

        [TestMethod]
        public void Test_Yes_FromOK_ToClosed()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.StartFix();
            bug.Verify();
            bug.Yes();
            Assert.AreEqual(BugState.Closed, bug.GetState());
        }

        [TestMethod]
        public void Test_No_FromOK_BackToFix()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.StartFix();
            bug.Verify();
            bug.No();
            Assert.AreEqual(BugState.Fix, bug.GetState());
        }

        [TestMethod]
        public void Test_Reopen_FromClosed_ToTriage()
        {
            var bug = new Bug();
            bug.Analyze();
            bug.StartFix();
            bug.Verify();
            bug.Yes();
            bug.Reopen();
            Assert.AreEqual(BugState.Triage, bug.GetState());
        }
    }
}
