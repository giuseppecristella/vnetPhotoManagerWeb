<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="VnetPhotoManager.Web.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <br />
                    <h4>Photo Manager Home</h4>
                    <hr />
               </div>
                <div class="form-group">
                    <asp:Button runat="server" Text="Inserisci foto" CssClass="btn btn-default" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" Text="Visualizza foto" CssClass="btn btn-default" />
                </div>
            </section>
        </div>
    </div>


    <%--    <div class="row">
        <div class="col-md-8">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" Text="Visualizza foto" CssClass="btn btn-default" />
            </div>
        </div>
    </div>--%>
</asp:Content>
