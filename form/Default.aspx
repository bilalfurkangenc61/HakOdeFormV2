<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="form._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Sayfa Arkaplanı */
        .content {
            position: relative;
            width: 100%;
            height: 100vh;
            display: flex;
            flex-direction: column;
            justify-content: flex-start; /* Yazıyı üste hizalar */
            align-items: center; /* Ortalar */
            text-align: center;
            padding-top: 100px; /* Üst boşluk bırakır */
        }

        /* Arkaplan Resmi */
        .background-image {
            position: absolute;
            top: 0;
            left: 0;
           
            
            background: url('mainslider.jpg') no-repeat center center;
            background-size: contain;
            background-repeat: no-repeat;
             /* Hafif karartılmış arka plan */
        }

        /* Hoşgeldiniz Yazısı */
        .welcome-text {
            position: relative;
            z-index: 1;
            font-size: 3rem;
            font-weight: bold;
            color: white;
        }

        /* Footer Mavi Renk */
        footer {
            background-color: #180554; /* Mavi */
            color: white;
            text-align: center;
            padding: 10px 0;
            position: fixed;
            width: 100%;
            bottom: 0;
        }
    </style>

    <div class="content">
        <div class="background-image"></div>
        <h1 class="welcome-text">Hoş Geldiniz</h1>
    </div>
</asp:Content>
