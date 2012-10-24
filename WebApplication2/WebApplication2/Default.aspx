<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
 <meta charset="utf-8" />
    <title>jQuery UI Selectable - Default functionality</title>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.0/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script src="http://code.jquery.com/ui/1.9.0/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />
 
    <style>
    #feedback { font-size: 1.4em; }
    #selectable .ui-selecting { background: #FECA40; }
    #selectable .ui-selected { background: #F39814; color: white; }
    #selectable { list-style-type: none; margin: 0; padding: 0; width: 60%; }
    #selectable li { margin: 3px; padding: 0.4em; font-size: 1.4em; height: 18px; }
    </style>

    <script>
        $(function () {
            $("#selectable").selectable();
        });
    </script>
   
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
       Web Application to Test Voice Recognition
    </h2>
    <p>
        <asp:FileUpload ID="FileUpload1" runat="server" Height="50px" Width="239px" /><asp:ImageButton ID="ImageButton1"
            runat="server" Height="62px" ImageUrl="~/image/submit.jpg" 
            onclick="ImageButton1_Click" ToolTip="Submit to see the status" />
    </p>
    
    <p>
       Voice Recognition using Google API : 
        
    </p>
   <div id="display" style="font-family: verdana">
   <asp:RadioButtonList ID="RadioButtonList1" runat="server">

    </asp:RadioButtonList>
  </div>
<%--<ol id="selectable">
    <li class="ui-widget-content"><asp:Label ID="Label1" runat="server"></asp:Label></li>
    
</ol>--%>
 
 

    
 
</asp:Content>
