using Checkout.Instruments.Four.Create;
using Checkout.Instruments.Four.Get;
using Checkout.Instruments.Four.Update;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Instruments.Four
{
    public class InstrumentsClientTest : UnitTestFixture
    {
        private readonly SdkAuthorization _authorization = new SdkAuthorization(PlatformType.Default, ValidDefaultSk);
        private readonly Mock<IApiClient> _apiClient = new Mock<IApiClient>();
        private readonly Mock<SdkCredentials> _sdkCredentials = new Mock<SdkCredentials>(PlatformType.Default);
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<CheckoutConfiguration> _configuration;

        public InstrumentsClientTest()
        {
            _sdkCredentials.Setup(credentials => credentials.GetSdkAuthorization(SdkAuthorizationType.SecretKeyOrOAuth))
                .Returns(_authorization);

            _configuration = new Mock<CheckoutConfiguration>(_sdkCredentials.Object,
                Environment.Sandbox, _httpClientFactory.Object, Environment.Sandbox);
        }

        [Fact]
        private async Task ShouldGetInstrument()
        {
            var instrumentResponse = new GetInstrumentResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Get<GetInstrumentResponse>("instruments/instrument_id", _authorization,
                        CancellationToken.None))
                .ReturnsAsync(() => instrumentResponse);

            IInstrumentsClient client =
                new InstrumentsClient(_apiClient.Object, _configuration.Object);

            var response = await client.Get("instrument_id");

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(instrumentResponse);
        }

        [Fact]
        private async Task ShouldCreateInstrument()
        {
            var createInstrumentRequest = new CreateBankAccountInstrumentRequest();
            var createInstrumentResponse = new CreateBankAccountInstrumentResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<CreateBankAccountInstrumentResponse>("instruments", _authorization,
                        createInstrumentRequest,
                        CancellationToken.None, null))
                .ReturnsAsync(() => createInstrumentResponse);

            IInstrumentsClient client =
                new InstrumentsClient(_apiClient.Object, _configuration.Object);

            var response = await client.Create<CreateBankAccountInstrumentResponse>(createInstrumentRequest);

            response.ShouldNotBeNull();
            response.ShouldBe(createInstrumentResponse);
        }

        [Fact]
        private async Task ShouldUpdateInstrument()
        {
            var updateInstrumentRequest = new UpdateCardInstrumentRequest();
            var updateInstrumentResponse = new UpdateCardInstrumentResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Patch<Update.UpdateInstrumentResponse>("instruments/instrument_id", _authorization,
                        updateInstrumentRequest,
                        CancellationToken.None, null))
                .ReturnsAsync(() => updateInstrumentResponse);

            IInstrumentsClient client =
                new InstrumentsClient(_apiClient.Object, _configuration.Object);

            var response = await client.Update("instrument_id", updateInstrumentRequest);

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(updateInstrumentResponse);
        }

        [Fact]
        private async Task ShouldDeleteInstrument()
        {
            _apiClient.Setup(apiClient =>
                    apiClient.Delete<object>("instruments/instrument_id", _authorization,
                        CancellationToken.None))
                .ReturnsAsync(() => new object());

            IInstrumentsClient client =
                new InstrumentsClient(_apiClient.Object, _configuration.Object);

            var response = await client.Delete("instrument_id");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldGetBankAccountFieldFormatting()
        {
            BankAccountFieldQuery bankAccountFieldQuery = new BankAccountFieldQuery();

            _sdkCredentials.Setup(credentials => credentials.GetSdkAuthorization(SdkAuthorizationType.OAuth))
              .Returns(_authorization);

            _apiClient.Setup(apiClient =>
                    apiClient.Query<BankAccountFieldResponse>("validation/bank-accounts/GB/GBP", _authorization, bankAccountFieldQuery,
                        CancellationToken.None)).ReturnsAsync(() => new BankAccountFieldResponse());

            IInstrumentsClient client =
                new InstrumentsClient(_apiClient.Object, _configuration.Object);

            var response = await client.GetBankAccountFieldFormatting(Common.CountryCode.GB, Common.Currency.GBP, bankAccountFieldQuery);

            response.ShouldNotBeNull();
        }
    }
}