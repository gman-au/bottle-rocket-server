using Rocket.Infrastructure;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class EmailAddressValidatorTests
    {
        private readonly TestContext _context = new();

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("john.smith@test.com", true)]
        [InlineData("john.smith@test.com.com", true)]
        [InlineData("john.smith", false)]
        [InlineData("john.smith@", false)]
        [InlineData("john.smith@test", true)]
        [InlineData("@test.com", false)]
        [InlineData("j@test.com", true)]
        [InlineData("j.@test.com", true)]
        [InlineData("john.smith.@t̴̨̧̙̗͎̼̫̪̯̻̺̠͇͇̼̄̔̔͐̽͒͋ę̸̗̫̩͖̪̙̳́̅̔̐̿̇̆͂̀̽͂̃̿̇͘̚͠ͅs̵̢͇̼̙̬͔͔̺̺̒̃̌̾́̽́́͛̚t̷̳̯̫̺̭͍̞͎͚̹̓͒̎̋̈̇̍̄̏̚̚͜͝.com", false)]
        [InlineData("j̷̛̠͇̰͖̠̮̰̟͓̼̬͔͈͋̓̂͊͜o̶̧̡̺̖̹͍̥͎͉̺̖̞̲͉̻̎̆͊́͆̿̽̾͝͝͝h̵̝͙̠͈̝̔͑̀͘ṉ̸͓̼̲̙͙͔̣̰̥̌̈̂̎͆̓́͛̒͛̅ͅ@test.com", false)]
        public void Test_Email_Validation(
            string email,
            bool isValid
        )
        {
            _context.ArrangeEmailAddress(email);
            _context.ActValidateEmail();
            _context.AssertValid(isValid);
        }

        private class TestContext
        {
            private readonly EmailAddressValidator _sut = new();
            private string _value;
            private bool _result;

            public void ArrangeEmailAddress(string email)
            {
                _value = email;
            }

            public void ActValidateEmail()
            {
                _result =
                    _sut
                        .IsValid(_value);
            }

            public void AssertValid(bool expected)
            {
                Assert
                    .Equal(
                        expected,
                        _result
                    );
            }
        }
    }
}