<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="VnetPhotoManager.Web.PhotoOrder.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <link href="../Scripts/dropzone/dropzone.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/dropzone/dropzone.js" type="text/javascript"></script>
    <link href="../Scripts/jcrop/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jcrop/jquery.Jcrop.js"></script>
    <script src="../Scripts/bootstrap.js"></script>
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
            <div id="target">
                <input type="button" value="Aggiungi" class="btn btn-info" id="btnAddandCrop" />
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <h2>Foto caricate</h2>
        <p>per procedere con l'ordine delle foto aggiunte, cliccare il pulsante Crea Ordine</p>
        <asp:ListView runat="server" ID="lvPhotos" OnItemCommand="lvPhotos_OnItemCommand" OnItemDataBound="lvPhotos_OnItemDataBound">
            <LayoutTemplate>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Foto</th>
                            <th>Nome</th>
                            <th>Formato</th>
                            <th></th>
                            <th>Prezzo Unit.</th>
                            <th>Num. Copie</th>
                            <%--   <th>Prezzo Tot.</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <tr runat="server" id="itemPlaceholder"></tr>
                    </tbody>
                </table>
            </LayoutTemplate>

            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ID="hfFtpPath" Value='<%# Eval("FtpPath") %>' />
                        <asp:Image Width="50" ID="imgPhoto" runat="server" ImageUrl='<%#Eval("Path") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>' /></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPrintFormats" AutoPostBack="True" OnSelectedIndexChanged="ddlPrintFormat_OnSelectedIndexChanged" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lbFormatPreview" Text="Anteprima Formato" CommandName="FormatPrieview"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:Label ID="lblPrice" runat="server" Text="" />
                    </td>
                    <td>
                        <asp:TextBox min="1" type="number" Text="1" CssClass="form-control" runat="server" ID="txtCopies"></asp:TextBox>
                        <%-- <asp:RequiredFieldValidator ID="rqCopies" runat="server" ControlToValidate="txtCopies" ValidationGroup="vgOrder"
                            CssClass="text-danger" ErrorMessage="Campo obbligatorio." />--%>
                    </td>
                    <%--  <td>
                         <asp:Label ID="lblTotPrice" runat="server" Text="" />
                    </td>--%>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:Button CssClass="btn btn-info" Visible="False" ID="btnOrder" runat="server" Text="Crea Ordine" OnClick="btnOrder_OnClick" />

    </div>
    <!-- Modal HTML -->
    <div id="addPhotoModal" class="modal fade" role="dialog">
        <div id="previewPhotoDialog" class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <%--   <asp:Panel runat="server" ClientIDMode="Static" ID="pnlCrop">  </asp:Panel>--%>
                <asp:Image ClientIDMode="Static" ImageUrl="Images/placeholder.png" ID="imgCropped" runat="server" />

                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-info pull-right" ID="btnAddToGrid" runat="server" Text="Salva" OnClick="btnCrop_Click" />
                </div>
            </div>
        </div> 
    </div>


    <asp:HiddenField ClientIDMode="Static" ID="X" runat="server" />
    <asp:HiddenField ClientIDMode="Static" ID="Y" runat="server" />
    <asp:HiddenField ClientIDMode="Static" ID="W" runat="server" />
    <asp:HiddenField ClientIDMode="Static" ID="H" runat="server" />


    <div id="formatPrieviewModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <asp:Image class="img-thumbnail" alt="Formato stampa" ID="imgPrintFormat" runat="server" />
            </div>
        </div>
    </div>

    <script>

        function openFormatPrieviewModal() {
            $("#formatPrieviewModal").modal('show');
        };

        //$("#target").click(function () {
        //    var a = dropZone.getUploadingFiles();

        //    $("#addPhotoModal").modal('show');

        //});
    </script>

</asp:Content>
