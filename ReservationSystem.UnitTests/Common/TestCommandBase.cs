using System;
using ReservationSystem.Domain.Contexts;
using ReservationSystem.UnitTests.Common;

namespace ReservationSystem.UnitTests.Common
{
    public class TestCommandBase : IDisposable
    {
        protected readonly ReservationDbContext Context;

        public TestCommandBase()
        {
            Context = DataFiller.Create();
        }
        public void Dispose()
        {
            DataFiller.Delete(Context);
        }
    }
}