using HCM.Data;
using Microsoft.EntityFrameworkCore;

namespace HCM.Services.Data.Tests.Mocks
{
    public static class DatabaseMock
    {
        public static HcmDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<HcmDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

                return new HcmDbContext(dbContextOptions, seed: false);
            }
        }
    }
}