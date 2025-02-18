﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using form.Generate;  // HashGenerator sınıfını içeren namespace

namespace form
{
    public partial class confirmPayment : System.Web.UI.Page
    {
        protected async void ProcessPayment_Click(object sender, EventArgs e)
        {
            lblResult.Text = "<b>Ödeme İşleniyor...</b><br/>";

            try
            {
                await ProcessPaymentAsync();
            }
            catch (Exception ex)
            {
                lblResult.Text += "<b>Hata:</b> " + ex.Message + "<br/>";
            }
        }

        private async Task ProcessPaymentAsync()
        {
            string token = await GetToken();
            if (string.IsNullOrEmpty(token))
            {
                lblResult.Text += "<b>Hata:</b> Token alınamadı!<br/>";
                return;
            }

            string baseUrl = "https://testapp.halkode.com.tr/ccpayment/api/confirmPayment";
            string merchantKey = "$2y$10$XUmbnOQ0nmHsZy8WxIno4euYobTVUzxqtU1h..x32zyfG6qw7OYrq";

            // Kullanıcıdan gelen verileri al
            string invoiceNumber = invoiceId.Text.Trim();
            decimal totalValue;

            if (!decimal.TryParse(totalAmount.Text, out totalValue) || totalValue <= 0)
            {
                lblResult.Text = "<b>Hata:</b> Geçersiz toplam tutar!";
                return;
            }

            string status = "1"; // API'ye gönderilecek durum bilgisi

            // Hash oluştur
            HashGenerator hashGenerator = new HashGenerator();
            string hashKey = hashGenerator.GenerateHashKey(
                false,
                merchantKey,
                invoiceNumber,
                status
            );

            var data = new
            {
                total = totalValue.ToString("F2").Replace(",", "."),
                invoice_id = invoiceNumber,
                status = status,
                merchant_key = merchantKey,
                hash_key = hashKey
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

                    lblResult.Text += "<b>API Yanıtı:</b> <pre>" + System.Web.HttpUtility.HtmlEncode(paymentResult) + "</pre><br/>";
                    lblResult.Text += "<b>HTTP Durum Kodu:</b> " + response.StatusCode + "<br/>";

                    if (!response.IsSuccessStatusCode)
                    {
                        lblResult.Text += "<b>Hata Mesajı:</b> " + paymentResult + "<br/>";
                    }
                }
                catch (Exception ex)
                {
                    lblResult.Text += "<b>İstek Hatası:</b> " + ex.Message + "<br/>";
                }
            }
        }

        private async Task<string> GetToken()
        {
            string baseUrl = "https://testapp.halkode.com.tr/ccpayment/api/token";
            var data = new
            {
                app_id = "f77c7d06a417638ccde51c35fd6f6c17",
                app_secret = "30296568e1d7941de4fd684dbc7203e4"
            };
            string jsonData = JsonConvert.SerializeObject(data);

            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(baseUrl, content);
                    string result = await response.Content.ReadAsStringAsync();

                    lblResult.Text += "<b>Token Yanıtı:</b> " + result + "<br/><br/>";

                    var decodedResponse = JsonConvert.DeserializeObject<dynamic>(result);
                    if (decodedResponse?.status_code == 100)
                    {
                        return decodedResponse.data.token.ToString();
                    }
                    else
                    {
                        lblResult.Text += "<b>Token Alma Hatası:</b> " + result + "<br/>";
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    lblResult.Text += "<b>Token Hatası:</b> " + ex.Message + "<br/>";
                    return null;
                }
            }
        }
    }
}
