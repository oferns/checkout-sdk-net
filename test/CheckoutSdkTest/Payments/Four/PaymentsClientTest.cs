using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Payments.Four.Request;
using Checkout.Payments.Four.Response;
using Moq;
using Shouldly;
using Xunit;

namespace Checkout.Payments.Four
{
    public class PaymentsClientTest : UnitTestFixture
    {
        private const string PaymentsPath = "payments";

        private readonly SdkAuthorization _authorization = new SdkAuthorization(PlatformType.Four, ValidFourSk);
        private readonly Mock<IApiClient> _apiClient = new Mock<IApiClient>();
        private readonly Mock<SdkCredentials> _sdkCredentials = new Mock<SdkCredentials>(PlatformType.Default);
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<CheckoutConfiguration> _configuration;

        public PaymentsClientTest()
        {
            _sdkCredentials.Setup(credentials => credentials.GetSdkAuthorization(SdkAuthorizationType.SecretKeyOrOAuth))
                .Returns(_authorization);

            _configuration = new Mock<CheckoutConfiguration>(_sdkCredentials.Object,
                Environment.Sandbox, _httpClientFactory.Object, Environment.Sandbox);
        }

        [Fact]
        private async Task ShouldRequestPayment()
        {
            var paymentRequest = new PaymentRequest();
            var paymentResponse = new PaymentResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<PaymentResponse>(PaymentsPath, _authorization, paymentRequest,
                        CancellationToken.None, null))
                .ReturnsAsync(() => paymentResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.RequestPayment(paymentRequest, null, CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(paymentResponse);
        }

        [Fact]
        private async Task ShouldRequestPayment_IdempotencyKey()
        {
            var paymentRequest = new PaymentRequest();
            var paymentResponse = new PaymentResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<PaymentResponse>(PaymentsPath, _authorization, paymentRequest,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => paymentResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.RequestPayment(paymentRequest, "test", CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(paymentResponse);
        }

        [Fact]
        private async Task ShouldRequestPayout()
        {
            var payoutRequest = new PayoutRequest();
            var payoutResponse = new PayoutResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<PayoutResponse>(PaymentsPath, _authorization, payoutRequest,
                        CancellationToken.None, null))
                .ReturnsAsync(() => payoutResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.RequestPayout(payoutRequest, null, CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(payoutResponse);
        }

        [Fact]
        private async Task ShouldRequestPayout_IdempotencyKey()
        {
            var payoutRequest = new PayoutRequest();
            var payoutResponse = new PayoutResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<PayoutResponse>(PaymentsPath, _authorization, payoutRequest,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => payoutResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.RequestPayout(payoutRequest, "test", CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(payoutResponse);
        }

        [Fact]
        private async Task ShouldGetPaymentDetails()
        {
            var paymentResponse = new GetPaymentResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Get<GetPaymentResponse>(PaymentsPath + "/payment_id", _authorization,
                        CancellationToken.None))
                .ReturnsAsync(() => paymentResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.GetPaymentDetails("payment_id", CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(paymentResponse);
        }

        [Fact]
        private async Task ShouldGetPaymentActions()
        {
            var paymentActions = new List<PaymentAction> {new PaymentAction(), new PaymentAction()};

            _apiClient.Setup(apiClient =>
                    apiClient.Get<IList<PaymentAction>>(PaymentsPath + "/payment_id/actions", _authorization,
                        CancellationToken.None))
                .ReturnsAsync(() => paymentActions);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.GetPaymentActions("payment_id", CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeSameAs(paymentActions);
        }

        [Fact]
        private async Task ShouldCapturePayment_Id()
        {
            var captureResponse = new CaptureResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<CaptureResponse>(PaymentsPath + "/payment_id/captures", _authorization,
                        null,
                        CancellationToken.None, null))
                .ReturnsAsync(() => captureResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.CapturePayment("payment_id", null);

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldCapturePayment_IdempotencyKey()
        {
            var captureResponse = new CaptureResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<CaptureResponse>(PaymentsPath + "/payment_id/captures", _authorization,
                        null,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => captureResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.CapturePayment("payment_id", null, "test");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldCapturePayment_Request()
        {
            var captureRequest = new CaptureRequest();
            var captureResponse = new CaptureResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<CaptureResponse>(PaymentsPath + "/payment_id/captures", _authorization,
                        captureRequest,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => captureResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response =
                await paymentsClient.CapturePayment("payment_id", captureRequest, "test");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldRefundPayment_Id()
        {
            var refundResponse = new RefundResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<RefundResponse>(PaymentsPath + "/payment_id/refunds", _authorization,
                        null,
                        CancellationToken.None, null))
                .ReturnsAsync(() => refundResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.RefundPayment("payment_id");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldRefundPayment_IdempotencyKey()
        {
            var refundResponse = new RefundResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<RefundResponse>(PaymentsPath + "/payment_id/refunds", _authorization,
                        null,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => refundResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.RefundPayment("payment_id", null, "test");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldRefundPayment_Request()
        {
            var refundRequest = new RefundRequest();
            var refundResponse = new RefundResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<RefundResponse>(PaymentsPath + "/payment_id/refunds", _authorization,
                        refundRequest,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => refundResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response =
                await paymentsClient.RefundPayment("payment_id", refundRequest, "test");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldVoidPayment_Id()
        {
            var voidResponse = new VoidResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<VoidResponse>(PaymentsPath + "/payment_id/voids", _authorization,
                        null,
                        CancellationToken.None, null))
                .ReturnsAsync(() => voidResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.VoidPayment("payment_id");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldVoidPayment_IdempotencyKey()
        {
            var voidResponse = new VoidResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<VoidResponse>(PaymentsPath + "/payment_id/voids", _authorization,
                        null,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => voidResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.VoidPayment("payment_id", null, "test");

            response.ShouldNotBeNull();
        }

        [Fact]
        private async Task ShouldVoidPayment_Request()
        {
            var voidRequest = new VoidRequest();
            var voidResponse = new VoidResponse();

            _apiClient.Setup(apiClient =>
                    apiClient.Post<VoidResponse>(PaymentsPath + "/payment_id/voids", _authorization,
                        voidRequest,
                        CancellationToken.None, "test"))
                .ReturnsAsync(() => voidResponse);

            IPaymentsClient paymentsClient = new PaymentsClient(_apiClient.Object, _configuration.Object);

            var response = await paymentsClient.VoidPayment("payment_id", voidRequest, "test");

            response.ShouldNotBeNull();
        }
    }
}