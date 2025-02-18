<%@ Page Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="confirmPayment.aspx.cs" Inherits="form.confirmPayment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="mb-3">
            <label class="form-label">Fatura Numarası</label>
            <asp:TextBox ID="invoiceId" CssClass="form-control" runat="server" required></asp:TextBox>
        </div>

        <div class="mb-3">
            <label class="form-label">Toplam Tutar (TRY)</label>
            <asp:TextBox ID="totalAmount" CssClass="form-control" runat="server" required></asp:TextBox>
        </div>

        <div class="mb-3">
            <label class="form-label">Durum</label>
            <asp:DropDownList ID="status" CssClass="form-select" runat="server">
                <asp:ListItem Value="1" Selected="True">Başarılı</asp:ListItem>
                <asp:ListItem Value="0">Başarısız</asp:ListItem>
            </asp:DropDownList>
        </div>

        <asp:Button ID="btnConfirmPayment" CssClass="btn btn-success w-100" runat="server" Text="Ödemeyi Onayla" OnClick="ProcessPayment_Click" />

        <div class="alert alert-info mt-4">
            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <div id="apiResponseContainer" runat="server"></div>
</asp:Content>
