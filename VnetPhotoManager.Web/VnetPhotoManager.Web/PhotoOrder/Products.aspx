<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="VnetPhotoManager.Web.PhotoOrder.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h2>Lista Prodotti disponibili categoria :
            <asp:Literal ID="ltrCategoryName" runat="server"></asp:Literal></h2>
        <p>Selezionare la categoria dalla lista seguente</p>
        <asp:ListView runat="server" ID="lvProducts" OnItemDataBound="lvProducts_OnItemDataBound">
            <LayoutTemplate>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Foto</th>
                            <th>Descrizione</th>
                            <th>Prezzo</th>
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
                        <asp:HiddenField runat="server" ID="hfProductId" Value='<%# Eval("ProductId") %>' />
                        <asp:HiddenField runat="server" ID="hfProductCode" Value='<%# Eval("Code") %>' />
                        <asp:Image Width="50" ID="imgPhotoProduct" runat="server" />
                    </td>
                    <td>
                       <a href='<%# string.Format("Add.aspx?CatId={0}&ProdId={1}", Eval("CategoryId"), Eval("ProductId")) %>'> <asp:Literal ID="lblName" runat="server" Text='<%#Eval("Description") %>' /></a>
                    </td>
                    <td>
                        <asp:Literal ID="ltlPrice" runat="server" Text='<%#Eval("Price") %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Content>
