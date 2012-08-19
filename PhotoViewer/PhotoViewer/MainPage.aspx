<%@ Page Language="C#" AutoEventWireup="true" Inherits="MainPage" Codebehind="MainPage.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
            height: 342px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="top: 15px; left: 10px; position: absolute; height: 19px; width: 1078px">
    
        <asp:Label ID="lblTitle" runat="server" Text="Picture Set" 
            style="top: 0px; left: 0px; position: absolute; height: 19px; width: 77px"></asp:Label>
        
        <asp:Panel ID="Panel1" runat="server" 
            style="top: 24px; left: 0px; position: absolute; height: 144px; width: 176px; border-style: ridge">
            <asp:Label ID="lblChooseOne" runat="server" Text="Choose One" 
            style="top: 0px; left: 0px; position: absolute; height: 19px; width: 77px"></asp:Label><br /><br />
            <asp:RadioButton ID="rbBigSur" runat="server" Text="Big Sur" GroupName="Region" 
                oncheckedchanged="rbBigSur_CheckedChanged" /><br />    
            <asp:RadioButton ID="rbJoshuaTree" runat="server" Text="Joshua Tree" GroupName="Region"
                oncheckedchanged="rbJoshuaTree_CheckedChanged"/><br />
            <asp:RadioButton ID="rbZion" runat="server" Text="Zion" GroupName="Region"
                oncheckedchanged="rbZion_CheckedChanged"/>
        </asp:Panel>
        <br />
        <br />
    
    </div>
    
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        <asp:Button ID="btnPrevious" runat="server" OnClick="btnPrevious_Click"
            style="top: 208px; left: 16px; position: absolute; height: 26px; width: 71px" 
            Text="Previous" />
        <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click"
            style="top: 207px; left: 116px; position: absolute; height: 26px; width: 70px" 
            Text="Next" />
    </p>
    
    <p>
            <asp:Label ID="lblTravelDate" runat="server" 
    style="top: 271px; left: 15px; position: absolute; height: 19px; width: 134px; border-style: Solid; border-width: 1px; background-color: #99CCFF" 
    Text="date"></asp:Label>
    </p>

    <asp:Label ID="lblError" runat="server" 
        style="top: 321px; left: 23px; position: absolute; height: 19px; width: 253px" 
        Visible="False"></asp:Label>
    <asp:Label ID="lblCaption" runat="server" 
        style="top: 530px; left: 466px; position: absolute; height: 29px; width: 323px" 
        Text="description"></asp:Label>
    <asp:Image ID="imgMain" runat="server" 
        style="top: 26px; left: 306px; position: absolute; height: 480px" />
    </form>
</body>
</html>
