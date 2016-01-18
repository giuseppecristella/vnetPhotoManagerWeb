<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateOrder.aspx.cs" Inherits="VnetPhotoManager.Web.PhotoOrder.CreateOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Ordine</h2>
    <div class="form-horizontal">
        <h4>Crea un nuovo ordine</h4>
        <hr />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ddlPrintFormat" CssClass="col-md-2 control-label">Formato</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList CssClass="form-control" ID="ddlPrintFormat" ClientIDMode="Static" runat="server" />
                 <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPrintFormat"
                    CssClass="text-danger" ErrorMessage="Selezionare un formato di stampa." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="imgPrintFormat" CssClass="col-md-2 control-label">Anteprima Formato</asp:Label>
            <div class="col-md-10">
                <asp:Image ID="imgPrintFormat" ImageUrl="Images/placeholder.png" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtCopies" CssClass="col-md-2 control-label">Copie</asp:Label>
            <div class="col-md-10">
                <asp:TextBox min="1" type="number" CssClass="form-control" runat="server" ID="txtCopies"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtNotes" CssClass="col-md-2 control-label">Note</asp:Label>
            <div class="col-md-10">
                <asp:TextBox CssClass="form-control" TextMode="MultiLine" Columns="5" runat="server" ID="txtNotes"></asp:TextBox>
            </div>
        </div>
        <hr />
        <h4>Pagamento</h4>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ddlPayments" CssClass="col-md-2 control-label">Tipo di pagamento</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList CssClass="form-control" ID="ddlPayments" ClientIDMode="Static" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPayments"
                    CssClass="text-danger" ErrorMessage="Selezionare un metodo di pagamento." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ddlPayments" CssClass="col-md-2 control-label"></asp:Label>
            <div class="col-md-10">
                <asp:Button runat="server" ID="btnCreateOrder" Text="Ordina" CssClass="btn btn-info" />
            </div>
        </div>
    </div>
</asp:Content>
