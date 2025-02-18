<%@ Page Title="2D Ödeme" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="PaySmart2D.aspx.cs" Inherits="form.PaySmart2D" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="text-center mt-4">2D Ödeme Formu</h2>

        <div class="card p-4 mt-3 shadow">
            <label>Kart Sahibi Adı:</label>
            <asp:TextBox ID="cardHolderName" CssClass="form-control" runat="server"></asp:TextBox>

            <label>Kart Numarası:</label>
            <asp:TextBox ID="cardNumber" CssClass="form-control" runat="server"></asp:TextBox>

            <label>Son Kullanma Ayı:</label>
            <asp:TextBox ID="expiryMonth" CssClass="form-control" runat="server"></asp:TextBox>

            <label>Son Kullanma Yılı:</label>
            <asp:TextBox ID="expiryYear" CssClass="form-control" runat="server"></asp:TextBox>

            <label>CVV:</label>
            <asp:TextBox ID="cvv" CssClass="form-control" runat="server"></asp:TextBox>

            <label>Fatura No:</label>
            <asp:TextBox ID="invoiceId" CssClass="form-control" runat="server"></asp:TextBox>

            <label>Toplam Tutar (TRY):</label>
            <asp:TextBox ID="totalAmount" CssClass="form-control" runat="server"></asp:TextBox>

            <label>Taksit Sayısı:</label>
            <asp:DropDownList ID="installments" CssClass="form-control" runat="server">
                <asp:ListItem Value="1" Selected="True">Tek Çekim</asp:ListItem>
                <asp:ListItem Value="2">2 Taksit</asp:ListItem>
                <asp:ListItem Value="3">3 Taksit</asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="btnPay" CssClass="btn btn-primary mt-3 w-100" runat="server" Text="Ödeme Yap" OnClick="ProcessPayment_Click" />

            <div class="result mt-3">
                <asp:Label ID="lblResult" CssClass="alert alert-info d-block text-center" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
