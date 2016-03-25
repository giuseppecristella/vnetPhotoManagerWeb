<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="VnetPhotoManager.Web.PhotoOrder.Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h2>Lista Categorie disponibili</h2>
        <p>Selezionare la categoria dalla lista seguente</p>
        <asp:ListView runat="server" ID="lvCategories" OnItemDataBound="lvCategories_OnItemDataBound">
            <LayoutTemplate>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Foto</th>
                            <th>Descrizione</th>
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
                        <asp:HiddenField runat="server" ID="hfCategoryId" Value='<%# Eval("CategoryId") %>'/>
                         <asp:Image Width="50" ID="imgPhotoCategory" runat="server"  />
                    </td>
                    <td>
                        <a href='Products.aspx?CatId=<%# Eval("CategoryId") %>&CatAdminId=<%# Eval("CategoryIdAdmin") %>'><asp:Literal ID="lblName" runat="server" Text='<%#Eval("Description") %>' /></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Content>
