using BoilerPlate.Interfaces;

namespace BoilerPlate.Services
{
    public class TestService : ITestService
    {
        public string Get()
        {
            return "test";
        }
    }
}
