using System;
using System.Web;
using System.Web.UI.WebControls;

namespace form
{
    public partial class Success : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetLabelValue(lblcardNumber, "credit_card_no");
                SetLabelValue(lblinvoiceId, "invoice_id");

                // API yanıtını ekrana JSON formatında bastık
                string apiResponse = Request.QueryString.ToString();
                apiResponseContainer.InnerHtml = $"<pre>{HttpUtility.HtmlEncode(apiResponse)}</pre>";
            }
        }

        private void SetLabelValue(Label label, string key)
        {
            string value = Request.QueryString[key];
            if (!string.IsNullOrEmpty(value))
            {
                label.Text = value;
            }
            else
            {
                label.Visible = false; // Eğer veri yoksa label tamamen gizlenecek
            }
        }
    }
}
