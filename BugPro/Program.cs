using System;
using Stateless;

namespace BugPro
{
    // Состояния workflow
    public enum BugState
    {
        NewDefect,
        Triage,
        NotNow,
        NotFix,
        SeparateSolution,
        Duplicate,
        OtherProduct,
        NeedMoreInfo,
        CannotReproduce,
        Fix,
        OK,
        Closed
    }

    // Триггеры (действия)
    public enum BugTrigger
    {
        Analyze,
        Defer,
        WontFix,
        SeparateSolution,
        Duplicate,
        OtherProduct,
        NeedInfo,
        CannotReproduce,
        StartFix,
        Verify,
        Yes,
        No,
        Reopen
    }

    public class Bug
    {
        private StateMachine<BugState, BugTrigger> _machine;
        private BugState _state;

        public Bug()
        {
            _machine = new StateMachine<BugState, BugTrigger>(() => _state, s => _state = s);
            _state = BugState.NewDefect;

            // Переходы
            _machine.Configure(BugState.NewDefect)
                .Permit(BugTrigger.Analyze, BugState.Triage);

            _machine.Configure(BugState.Triage)
                .Permit(BugTrigger.Defer, BugState.NotNow)
                .Permit(BugTrigger.WontFix, BugState.NotFix)
                .Permit(BugTrigger.SeparateSolution, BugState.SeparateSolution)
                .Permit(BugTrigger.Duplicate, BugState.Duplicate)
                .Permit(BugTrigger.OtherProduct, BugState.OtherProduct)
                .Permit(BugTrigger.NeedInfo, BugState.NeedMoreInfo)
                .Permit(BugTrigger.CannotReproduce, BugState.CannotReproduce)
                .Permit(BugTrigger.StartFix, BugState.Fix);

            _machine.Configure(BugState.Fix)
                .Permit(BugTrigger.Verify, BugState.OK);

            _machine.Configure(BugState.OK)
                .Permit(BugTrigger.Yes, BugState.Closed)
                .Permit(BugTrigger.No, BugState.Fix);

            _machine.Configure(BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Triage);
        }

        public void Analyze() => _machine.Fire(BugTrigger.Analyze);
        public void Defer() => _machine.Fire(BugTrigger.Defer);
        public void WontFix() => _machine.Fire(BugTrigger.WontFix);
        public void SeparateSolution() => _machine.Fire(BugTrigger.SeparateSolution);
        public void Duplicate() => _machine.Fire(BugTrigger.Duplicate);
        public void OtherProduct() => _machine.Fire(BugTrigger.OtherProduct);
        public void NeedInfo() => _machine.Fire(BugTrigger.NeedInfo);
        public void CannotReproduce() => _machine.Fire(BugTrigger.CannotReproduce);
        public void StartFix() => _machine.Fire(BugTrigger.StartFix);
        public void Verify() => _machine.Fire(BugTrigger.Verify);
        public void Yes() => _machine.Fire(BugTrigger.Yes);
        public void No() => _machine.Fire(BugTrigger.No);
        public void Reopen() => _machine.Fire(BugTrigger.Reopen);

        public BugState GetState() => _state;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();
            Console.WriteLine($"Initial state: {bug.GetState()}");

            bug.Analyze();
            Console.WriteLine($"After Analyze: {bug.GetState()}");

            bug.StartFix();
            Console.WriteLine($"After StartFix: {bug.GetState()}");

            bug.Verify();
            Console.WriteLine($"After Verify: {bug.GetState()}");

            bug.Yes();
            Console.WriteLine($"After Yes: {bug.GetState()}");

            bug.Reopen();
            Console.WriteLine($"After Reopen: {bug.GetState()}");
        }
    }
}
