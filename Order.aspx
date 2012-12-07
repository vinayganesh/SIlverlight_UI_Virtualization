<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="ImportTradeFiles.Web.Order" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
    html, body {
      height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    .button
        {
            border: solid 1px #c0c0c0;
            background-color: #eeeeee;
            font-family: verdana;
            font-size: 11px;
        }
        
        .modalBg
        {
            z-index: 99999999;
            background-color: #cccccc;
            filter: alpha(opacity=10);
            opacity: 0.8;
        }
        
        .modalPanel
        {
            z-index: 99999999;
            background-color: #ffffff;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 320px;
        }
    </style>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }

        function ShowOverlay(text) {

            var symbol = text[3];
            var quantity = text[2];

            document.getElementById('<%=txtSymbol.ClientID %>').value = symbol;
            document.getElementById('<%=txtQuantity.ClientID %>').value = quantity;

            $find('ModalPopupExtender1').show();
  
        
        }

        function SendData() {

            var symbol = document.getElementById('<%=txtSymbol.ClientID %>').value;
            var quantity = document.getElementById('<%=txtQuantity.ClientID %>').value;


            var control = document.getElementById("SilverlightControl");
            control.Content.SilverlightPlugin.EditedData(symbol, quantity);
        }

        

    </script>
</head>
<body>
     <form id="form1" runat="server" style="height:100%">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="silverlightControlHost">
        <object id="SilverlightControl" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/ImportTradeFiles.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="4.0.50826.0" />
		  <param name="autoUpgrade" value="true" />
          <param name="Windowless" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe>
        
          <asp:Panel ID="Panel1" runat="server" CssClass="modalPanel" >
                
                    <asp:Panel ID="Panel2" runat="server" style="border:solid 2px #cccccc; cursor: move;">Drag Panel </asp:Panel>
                      <center><asp:Button ID="btnPopup" text="" runat="server" Width="0px" Enabled="false"></asp:Button></center>
                          <div>
                           <strong>
                              Edit Required Info<br />
                           </strong>
                           <asp:Label ID="lblSymbol" Text="Symbol" runat="server"/>
                           <asp:TextBox ID="txtSymbol" runat="server" /><br /><br />

                           <asp:Label ID="lblQuantity" Text="Quantity" runat="server"/>
                           <asp:TextBox ID="txtQuantity" runat="server" /><br /><br />

                           <asp:Label ID="lblAction" Text="Action" runat="server" />
                           <asp:DropDownList ID="ddAction" runat="server" >
                               <asp:ListItem>Buy</asp:ListItem>
                               <asp:ListItem>Sell</asp:ListItem>
                           </asp:DropDownList><br /><br />

                           <asp:Label ID="lbltest1" Text="Order Type" runat="server" />
                           <asp:DropDownList ID="DropDownList1" runat="server" >
                               <asp:ListItem>Market</asp:ListItem>
                               <asp:ListItem>Item 2</asp:ListItem>
                           </asp:DropDownList><br /><br />


                           <asp:Label ID="Label1" Text="Timing" runat="server" />
                           <asp:DropDownList ID="DropDownList2" runat="server" >
                               <asp:ListItem>Day</asp:ListItem>
                               <asp:ListItem>Item 2</asp:ListItem>
                           </asp:DropDownList><br /><br />

                       </div>
                      
                        <asp:Button ID="btnSave" runat="server" Text="Save" />
                       <hr />
                      
               <asp:Button ID="btnCancel" CssClass="button" runat="server" Text="[ X ]" />
                    
                            
            </asp:Panel>

         <ajaxToolkit:ModalPopupExtender
                    ID="ModalPopupExtender1"
                    BackgroundCssClass="modalBg"
                    DropShadow="true"
                    PopupControlID="Panel1"
                    runat="server"
                    TargetControlID="btnPopup"
                    PopupDragHandleControlID="Panel2"
                    OkControlID="btnSave"
                    OnOkScript="SendData()"
                    CancelControlID="btnCancel"
                  >
           </ajaxToolkit:ModalPopupExtender>
           
        </div>

    </form>
</body>
</html>