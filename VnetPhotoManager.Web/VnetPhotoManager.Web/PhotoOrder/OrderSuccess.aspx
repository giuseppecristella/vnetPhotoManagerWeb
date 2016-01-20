<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderSuccess.aspx.cs" Inherits="VnetPhotoManager.Web.PhotoOrder.OrderSuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Ordine</h2>
    <div class="form-horizontal">
        <h4>Ordine n. <asp:Label runat="server" ID="lblOrderNumber"></asp:Label >Conluso con successo</h4>
        <hr />
    </div>
</asp:Content>
