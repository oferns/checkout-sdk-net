namespace Checkout.Payments
{
    public class TokenSource : IPaymentSource
    {
        public TokenSource(string token)
        {
            Token = token;
        }

        public string Token { get; }

        public string Type => "token";
    }
}