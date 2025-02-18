using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using form.Generate;

namespace form
{
    public partial class PaySmart3D : System.Web.UI.Page
    {
        protected async void ProcessPayment_Click(object sender, EventArgs e)
        {
            lblResult.Text = "<b>Ödeme İşleniyor...</b><br/>";

            try
            {
                string apiResponse = await ProcessPaymentAsync();
                if (!string.IsNullOrEmpty(apiResponse))
                {
                    // API yanıtı HTML form içeriyorsa, yeni bir sayfa olarak kaydet ve yönlendir
                    string filePath = Server.MapPath("~/Redirect.html");
                    File.WriteAllText(filePath, apiResponse);

                    Response.Redirect("Redirect.html");
                }
                else
                {
                    lblResult.Text += "<b>Ödeme başarısız!</b><br/>";
                }
            }
            catch (Exception ex)
            {
                lblResult.Text += $"<b>Hata:</b> {ex.Message}<br/>";
            }
        }

        private async Task<string> ProcessPaymentAsync()
        {
            string token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                lblResult.Text += "<b>Hata:</b> Token alınamadı!<br/>";
                return null;
            }

            string baseUrl = "https://testapp.halkode.com.tr/ccpayment/api/paySmart3D";
            string merchantKey = "$2y$10$XUmbnOQ0nmHsZy8WxIno4euYobTVUzxqtU1h..x32zyfG6qw7OYrq";

            decimal totalValue;
            if (!decimal.TryParse(totalAmount.Text, out totalValue) || totalValue <= 0)
            {
                lblResult.Text = "<b>Hata:</b> Geçersiz toplam tutar!";
                return null;
            }

            string invoiceNumber = invoiceId.Text.Trim();
            string currencyCode = "TRY";
            string installmentsValue = installments.SelectedValue;

            // Hash oluştur
            HashGenerator hashGenerator = new HashGenerator();
            string hashKey = hashGenerator.GenerateHashKey(false, totalValue.ToString().Replace(",", "."), installmentsValue, currencyCode, merchantKey, invoiceNumber);

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
                merchant_key = merchantKey,
                hash_key = hashKey,
                return_url = "https://www.google.com/",
                cancel_url = "https://www.github.com/",
                payment_completed_by = "app",
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    lblResult.Text += "<b>API'ye istek gönderiliyor...</b><br/>";
                    HttpResponseMessage response = await client.PostAsync(baseUrl, content);
                    string paymentResult = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        lblResult.Text += $"<b>Hata Kodu:</b> {response.StatusCode}<br/>";
                        return null;
                    }

                    return paymentResult;
                }
                catch (Exception ex)
                {
                    lblResult.Text += $"<b>İstek Hatası:</b> {ex.Message}<br/>";
                    return null;
                }
            }
        }

        private async Task<string> GetTokenAsync()
        {
            string baseUrl = "https://testapp.halkode.com.tr/ccpayment/api/token";
            var data = new
            {
                app_id = "f77c7d06a417638ccde51c35fd6f6c17",
                app_secret = "30296568e1d7941de4fd684dbc7203e4"
            };

            using (HttpClient client = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(baseUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var decodedResponse = JsonConvert.DeserializeObject<dynamic>(result);

                    if (decodedResponse?.status_code == 100)
                    {
                        return decodedResponse.data.token.ToString();
                    }
                    else
                    {
                        lblResult.Text += "<b>Token Alma Hatası:</b> Yanıt geçersiz!<br/>";
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    lblResult.Text += $"<b>Token Hatası:</b> {ex.Message}<br/>";
                    return null;
                }
            }
        }
    }
}
