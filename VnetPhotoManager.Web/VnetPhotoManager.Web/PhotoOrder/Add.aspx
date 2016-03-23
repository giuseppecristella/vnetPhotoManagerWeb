<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="VnetPhotoManager.Web.PhotoOrder.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <link href="../Scripts/dropzone/dropzone.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/dropzone/dropzone.js" type="text/javascript"></script>
    <link href="../Scripts/jcrop/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jcrop/jquery.Jcrop.js"></script>
    <script src="../Scripts/bootstrap.js"></script>
    <script src="Scripts/PhotoOrder.js"></script>

    <% if (PhotoToAdd == null)
       { %>
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
        <div class="col-md-3">
            <%--<div id="divAddAndCrop"></div>--%>
            <input type="button" value="Aggiungi" class="btn btn-info" id="btnAddandCrop" />
            <input type="button" value="Sfoglia" class="btn btn-info" id="btnAddSavedPhoto" />
        </div>
    </div>
    <% } %>
    <br />
    <div class="row">
        <h2>Il tuo carrello</h2>
        <p>Per procedere con l'ordine delle foto aggiunte, cliccare il pulsante Crea Ordine</p>
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
                            <th></th>
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
                        <asp:Label runat="server" ID="lblPrintFormat" Text='<%#Eval("FormatDescription") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lbFormatPreview" Text="Anteprima Formato" CommandName="FormatPrieview"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:Label ID="lblPrice" Text='<%#Eval("UnitPrice") %>' runat="server" />
                    </td>
                    <td>
                        <asp:TextBox min="1" type="number" Text="1" CssClass="form-control" runat="server" ID="txtCopies"></asp:TextBox>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lbDeletePhoto" CommandName="DeletePhoto">Elimina</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:Button CssClass="btn btn-info" Visible="False" ID="btnOrder" runat="server" Text="Crea Ordine" OnClick="btnOrder_OnClick" />
        <asp:Button CssClass="btn btn-info" ID="btnContinueUpload" runat="server" Text="Aggiungi altra foto" OnClick="btnContinueUpload_OnClick" />
    </div>
    <!-- Modal HTML -->
    <div id="addPhotoModal" class="modal fade" role="dialog">
        <div id="previewPhotoDialog" class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <%--   <asp:Panel runat="server" ClientIDMode="Static" ID="pnlCrop">  </asp:Panel>--%>
                <asp:Image ClientIDMode="Static" ID="imgCropped" runat="server" />

                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-info pull-right" ID="btnAddToGrid" runat="server" Text="Salva" OnClick="btnCrop_Click" />
                </div>
            </div>
        </div>
    </div>
    <div id="addSavedPhotoModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <asp:ListView runat="server" ID="lvSavedPhotos" OnItemDataBound="lvSavedPhotos_OnItemDataBound">
                    <LayoutTemplate>
                        <table class="table">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>Foto</th>
                                    <th>Nome</th>
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
                                <asp:RadioButton runat="server" ClientIDMode="Static" GroupName="rbSavedPhoto"  ID="cbSelectSavedPhoto" />
                            </td>
                            <td>
                                <asp:Image Width="50" ID="imgPhoto" runat="server" ImageUrl='<%# string.Format("images/{0}", Eval("Name")) %>' /></td>
                            <td>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' />
                                <asp:HiddenField runat="server" ID="hfFtpPath" Value='<%# Eval("FtpPath") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-info pull-right" ID="btnAddSavedPhotoToGrid" OnClick="btnAddSavedPhotoToGrid_OnClick" runat="server" Text="Salva" />
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

        function SetUniqueRadioButton(nameregex, current) {
            re = new RegExp(nameregex);
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.type == 'radio') {
                    if (re.test(elm.name)) {
                        elm.checked = false;
                    }
                }
            }
            current.checked = true;
        }
    </script>

</asp:Content>
