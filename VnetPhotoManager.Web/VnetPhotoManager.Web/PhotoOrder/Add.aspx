<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="VnetPhotoManager.Web.PhotoOrder.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <link href="../Scripts/dropzone/dropzone.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/dropzone/dropzone.js" type="text/javascript"></script>
    <link href="../Scripts/jcrop/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jcrop/jquery.Jcrop.js"></script>

    <script src="Scripts/PhotoOrder.js"></script>

    <h2>Upload Foto</h2>

    <div class="row">
        <div class="col-md-12">
            <div id="dZUpload" class="dropzone needsclick dz-clickable">
                <div class="dz-message needsclick">
                    <h4>Clicca e seleziona la foto da caricare.</h4>
                    <br>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12">
            <asp:Button CssClass="btn btn-info" ID="btnCrop" runat="server" Text="Salva" OnClick="btnCrop_Click" />
        </div>
    </div>
    <br />
    <div class="row">
        <asp:Panel runat="server" ClientIDMode="Static" id="pnlCrop" class="col-md-12" style="overflow: scroll;">
            <asp:Image ClientIDMode="Static" ID="imgCropped" runat="server" />
            <br />
            <asp:HiddenField ClientIDMode="Static" ID="X" runat="server" />
            <asp:HiddenField ClientIDMode="Static" ID="Y" runat="server" />
            <asp:HiddenField ClientIDMode="Static" ID="W" runat="server" />
            <asp:HiddenField ClientIDMode="Static" ID="H" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
