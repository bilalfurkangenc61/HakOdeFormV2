using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Web;
using System.Web.UI.WebControls;
using form.Generate;

namespace form
{
    public partial class PaySmart3D : System.Web.UI.Page
    {
        private const string ApiBaseUrl = "https://testapp.halkode.com.tr/ccpayment/api/";
        private const string MerchantKey = "$2y$10$XUmbnOQ0nmHsZy8WxIno4euYobTVUzxqtU1h..x32zyfG6qw7OYrq";
        private const string AppId = "f77c7d06a417638ccde51c35fd6f6c17";
        private const string AppSecret = "30296568e1d7941de4fd684dbc7203e4";

        protected async void ProcessPayment_Click(object sender, EventArgs e)
        {
            lblResult.Text = "<b>Ödeme İşleniyor...</b><br/>";

            try
            {
                string apiResponse = await ProcessPaymentAsync();
                if (!string.IsNullOrEmpty(apiResponse))
                {
                    string filePath = Server.MapPath("~/Redirect.html");
                    File.WriteAllText(filePath, apiResponse);
                    Response.Redirect("Redirect.html");
                }
                else
                {
                    lblResult.Text += "<b>Ödeme başarısız!</b><br/>";
                    RedirectToFailPage("41", "İşlem başarısız");
                }
            }
            catch (Exception ex)
            {
                lblResult.Text += $"<b>Hata:</b> {ex.Message}<br/>";
                RedirectToFailPage("500", ex.Message);
            }
        }

        private async Task<string> ProcessPaymentAsync()
        {
            string token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                RedirectToFailPage("401", "Token alınamadı!");
                return null;
            }

            if (!decimal.TryParse(totalAmount.Text, out decimal totalValue) || totalValue <= 0)
            {
                RedirectToFailPage("41", "Geçersiz toplam tutar!");
                return null;
            }

            string invoiceNumber = invoiceId.Text.Trim();
            string currencyCode = "TRY";
            string installmentsValue = installments.SelectedValue;

            HashGenerator hashGenerator = new HashGenerator();
            string hashKey = hashGenerator.GenerateHashKey(false, totalValue.ToString().Replace(",", "."), installmentsValue, currencyCode, MerchantKey, invoiceNumber);

            var data = new
            {
                cc_holder_name = cardHolderName.Text.Trim(),
                cc_no = cardNumber.Text.Trim(),
                expiry_month = expiryMonth.Text.Trim(),
                expiry_year = expiryYear.Text.Trim(),
                cvv = cvv.Text.Trim(),
                currency_code = currencyCode,
                installments_number = installmentsValue,
                invoice_id = invoiceNumber,
                invoice_description = "Ödeme Test",
                items = new[]
                {
                    new { name = "Ürün", price = totalValue, quantity = 1, description = "Satın alınan ürün" }
                },
                total = totalValue,
                merchant_key = MerchantKey,
                hash_key = hashKey,
                return_url = GetAbsoluteUrl("Success.aspx"),
                cancel_url = GetAbsoluteUrl("Fail.aspx"),
                payment_completed_by = "app",
            };

            return await SendHttpPostRequest("paySmart3D", token, data);
        }

        private async Task<string> GetTokenAsync()
        {
            var data = new { app_id = AppId, app_secret = AppSecret };
            return await SendHttpPostRequest("token", null, data);
        }

        private async Task<string> SendHttpPostRequest(string endpoint, string token, object data)
        {
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(ApiBaseUrl + endpoint, content);
                    string result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        RedirectToFailPage(response.StatusCode.ToString(), result);
                        return null;
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    RedirectToFailPage("500", ex.Message);
                    return null;
                }
            }
        }

        private void RedirectToFailPage(string errorCode, string errorMessage)
        {
            string url = $"Fail.aspx?error_code={errorCode}&error_message={HttpUtility.UrlEncode(errorMessage)}";
            Response.Redirect(url);
        }

        private string GetAbsoluteUrl(string relativePath)
        {
            var request = HttpContext.Current.Request;
            return $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/{relativePath}";
        }

        protected void cardNumber_TextChanged(object sender, EventArgs e)
        {
            string cardNo = cardNumber.Text.Trim();
            if (cardNo.StartsWith("4"))
            {
                SetInstallments(1, 2, 3);
            }
            else if (cardNo.StartsWith("5"))
            {
                SetInstallments(1, 2);
            }
            else
            {
                SetInstallments(1);
            }
        }

        private void SetInstallments(params int[] availableInstallments)
        {
            installments.Items.Clear();
            foreach (int installment in availableInstallments)
            {
                installments.Items.Add(new ListItem($"{installment} Taksit", installment.ToString()));
            }
        }
    }
}
