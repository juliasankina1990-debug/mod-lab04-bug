using System;
using Stateless;

namespace BugPro
{
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

        Return,     // 🔹 универсальный "возврат"
        Reopen
    }

    public class Bug
    {
        private readonly StateMachine<BugState, BugTrigger> _machine;
        private BugState _state;

        public Bug()
        {
            _machine = new StateMachine<BugState, BugTrigger>(() => _state, s => _state = s);
            _state = BugState.NewDefect;

            // Новый дефект → разбор
            _machine.Configure(BugState.NewDefect)
                .Permit(BugTrigger.Analyze, BugState.Triage);

            // Разбор дефектов
            _machine.Configure(BugState.Triage)
                .Permit(BugTrigger.Defer, BugState.NotNow)
                .Permit(BugTrigger.WontFix, BugState.NotFix)
                .Permit(BugTrigger.SeparateSolution, BugState.SeparateSolution)
                .Permit(BugTrigger.Duplicate, BugState.Duplicate)
                .Permit(BugTrigger.OtherProduct, BugState.OtherProduct)
                .Permit(BugTrigger.NeedInfo, BugState.NeedMoreInfo)
                .Permit(BugTrigger.CannotReproduce, BugState.CannotReproduce)
                .Permit(BugTrigger.StartFix, BugState.Fix);

            // 🔁 ВСЕ "боковые" состояния возвращаются в Triage

            _machine.Configure(BugState.NotNow)
                .Permit(BugTrigger.Return, BugState.Triage);

            _machine.Configure(BugState.NotFix)
                .Permit(BugTrigger.Return, BugState.Triage);

            _machine.Configure(BugState.SeparateSolution)
                .Permit(BugTrigger.Return, BugState.Triage);

            _machine.Configure(BugState.Duplicate)
                .Permit(BugTrigger.Return, BugState.Triage);

            _machine.Configure(BugState.OtherProduct)
                .Permit(BugTrigger.Return, BugState.Triage);

            _machine.Configure(BugState.NeedMoreInfo)
                .Permit(BugTrigger.Analyze, BugState.Triage); // получили инфу → снова разбор

            _machine.Configure(BugState.CannotReproduce)
                .Permit(BugTrigger.Verify, BugState.OK) // проверяем → OK?
                .Permit(BugTrigger.Return, BugState.Triage);

            // Исправление
            _machine.Configure(BugState.Fix)
                .Permit(BugTrigger.Verify, BugState.OK);

            // OK? (проверка)
            _machine.Configure(BugState.OK)
                .Permit(BugTrigger.Yes, BugState.Closed)
                .Permit(BugTrigger.No, BugState.Fix)      // не ок → доработка
                .Permit(BugTrigger.Return, BugState.Triage); // возврат

            // Закрытие
            _machine.Configure(BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Triage);
        }

        // Методы
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
        public void Return() => _machine.Fire(BugTrigger.Return);
        public void Reopen() => _machine.Fire(BugTrigger.Reopen);

        public BugState GetState() => _state;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();

            Console.WriteLine(bug.GetState());

            bug.Analyze();
            bug.StartFix();
            bug.Verify();
            bug.No();       // не ок → обратно в Fix
            bug.Verify();
            bug.Yes();      // закрыли

            Console.WriteLine(bug.GetState());

            bug.Reopen();   // переоткрытие
            Console.WriteLine(bug.GetState());
        }
    }
}
