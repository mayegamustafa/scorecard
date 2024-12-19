<%@ Page Language="C#" AutoEventWireup="true" CodeFile="score.aspx.cs" Inherits="home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #Text1 {
            width: 162px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; SCORE CARD<br />
        <br />
    
    </div>
        school&nbsp;
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="score" DataTextField="school_name" DataValueField="school_id">
        </asp:DropDownList>
        <asp:SqlDataSource ID="score" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [schools]"></asp:SqlDataSource>
        <br />
        area&nbsp;&nbsp; <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="area" DataTextField="area_name" DataValueField="area_id">
        </asp:DropDownList>
        <asp:SqlDataSource ID="area" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [areas]"></asp:SqlDataSource>
        <br />
        week <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="wk" DataTextField="week_name" DataValueField="week_id">
        </asp:DropDownList>
        <asp:SqlDataSource ID="wk" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [weeks]"></asp:SqlDataSource>
        <br />
        score&nbsp;&nbsp;
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        <br />
        &nbsp;&nbsp; <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="save" OnClick="Button1_Click" />
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; scores<br />
        <asp:SqlDataSource ID="sel" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [finance_analysis]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="ss" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [scores]"></asp:SqlDataSource>

        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; all
        <br />
        <br />
        <br />
        finance<br />
        <br />
        
        <asp:SqlDataSource ID="piv" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [finance_pivoted_view]"></asp:SqlDataSource>
        <br />
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; RANKING<br />
        <br />
        <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="False" DataSourceID="rnk" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                <asp:BoundField DataField="total_score" HeaderText="total_score" SortExpression="total_score" />
                <asp:BoundField DataField="average_score" HeaderText="average_score" SortExpression="average_score" />
                <asp:BoundField DataField="school_rank" HeaderText="school_rank" SortExpression="school_rank" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="rnk" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [finance_ranking]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; WEEKLY ANALYSIS<br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:GridView ID="GridView9" runat="server" AutoGenerateColumns="False" DataSourceID="weekly" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="week_name" HeaderText="week_name" SortExpression="week_name" />
                <asp:BoundField DataField="department_name" HeaderText="department_name" SortExpression="department_name" />
                <asp:BoundField DataField="area_name" HeaderText="area_name" SortExpression="area_name" />
                <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                <asp:BoundField DataField="avg_score" HeaderText="avg_score" ReadOnly="True" SortExpression="avg_score" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="weekly" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [week_analysis]"></asp:SqlDataSource>
        
        <br />
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; WEEKLY COMPARISON<br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="False" DataSourceID="wkly" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="Week1" HeaderText="Week1" SortExpression="Week1" />
                <asp:BoundField DataField="Week2" HeaderText="Week2" SortExpression="Week2" />
                <asp:BoundField DataField="area_name" HeaderText="area_name" SortExpression="area_name" />
                <asp:BoundField DataField="department_name" HeaderText="department_name" SortExpression="department_name" />
                <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                <asp:BoundField DataField="Week1_AvgScore" HeaderText="Week1_AvgScore" ReadOnly="True" SortExpression="Week1_AvgScore" />
                <asp:BoundField DataField="Week2_AvgScore" HeaderText="Week2_AvgScore" ReadOnly="True" SortExpression="Week2_AvgScore" />
                <asp:BoundField DataField="Score_Change" HeaderText="Score_Change" ReadOnly="True" SortExpression="Score_Change" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="wkly" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [week_comparison]"></asp:SqlDataSource>
        
    </form>
</body>
</html>
