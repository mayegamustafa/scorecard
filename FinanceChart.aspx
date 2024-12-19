<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinanceChart.aspx.cs" Inherits="FinanceChart" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finance Chart</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Chart ID="ScoresChart" runat="server" Width="2000px" Height="600px">
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="Legend1"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Series1" ChartType="Column"></asp:Series>
            </Series>
        </asp:Chart>
    </form>
</body>
</html>
