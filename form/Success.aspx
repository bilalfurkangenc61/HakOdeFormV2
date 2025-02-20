
<%@ Page Title="Success" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="form.Success" EnableEventValidation="false" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="text-center mt-4">Ödeme Başarılı</h2>

        <div class="card p-4 mt-3 shadow">


            <asp:label runat="server">Kart Numarası:</asp:label>
            <asp:label ID="lblcardNumber" CssClass="form-control" runat="server"></asp:label>


       

            <asp:label runat="server">Fatura No:</asp:label>
            <asp:label ID="lblinvoiceId" CssClass="form-control" runat="server"></asp:label>

          
        </div>

    
        <div id="apiResponseContainer" runat="server"></div>

    </div>
</asp:Content>
