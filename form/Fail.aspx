


<%@ Page Title="Success" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="Fail.aspx.cs" Inherits="form.Fail" EnableEventValidation="false" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="text-center mt-4">Ödeme Başarısız</h2>

        <div class="card p-4 mt-3 shadow">
            <label>Kart Sahibi Adı:</label>
            <asp:label ID="cardHolderName" CssClass="form-control" runat="server"></asp:label>

            <label>Kart Numarası:</label>
            <asp:label ID="cardNumber" CssClass="form-control" runat="server"></asp:label>

            <label>Son Kullanma Ayı:</label>
            <asp:label ID="expiryMonth" CssClass="form-control" runat="server"></asp:label>

            <label>Son Kullanma Yılı:</label>
            <asp:label ID="expiryYear" CssClass="form-control" runat="server"></asp:label>

            <label>CVV:</label>
            <asp:label ID="cvv" CssClass="form-control" runat="server"></asp:label>

            <label>Fatura No:</label>
            <asp:label ID="invoiceId" CssClass="form-control" runat="server"></asp:label>

            <label>Toplam Tutar (TRY):</label>
            <asp:label ID="totalAmount" CssClass="form-control" runat="server"></asp:label>
        </div>

    
        <div id="apiResponseContainer" runat="server"></div>

    </div>
</asp:Content>
